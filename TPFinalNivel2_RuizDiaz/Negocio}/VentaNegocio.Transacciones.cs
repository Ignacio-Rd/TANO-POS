using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dominio;
using Negocio_.Calculos;
using Negocios;

namespace Negocio_
{
    // Ids fijos de la tabla metodos_de_pago que el programa necesita conocer.
    public static class IdsMetodoPago
    {
        public const int Fiado = 11;
    }

    public class ResultadoVenta
    {
        public int IdVenta { get; set; }

        // Listo para imprimir. Se arma con lo mismo que se guardó, sin volver a consultar la base.
        public TICKET Ticket { get; set; }

        // Avisos que NO frenan la venta (por ejemplo: "no había stock suficiente"). También quedan en log_errores.txt.
        public List<string> Advertencias { get; } = new List<string>();
    }

    public class ResultadoEliminacion
    {
        public int IdAuditoria { get; set; }
        public int IdVenta { get; set; }
        public int UnidadesRepuestas { get; set; }
    }

    // Operaciones de venta que tocan VARIAS tablas: se ejecutan dentro de UNA transacción de SQL Server.
    // Si algo falla, se deshace todo y la base queda exactamente como estaba (no quedan ventas a medias).
    public partial class VentaNegocio
    {
        private const int LargoMinimoMotivo = 3;

        // Guarda la venta completa: encabezado, líneas, descuento de stock, método de pago y (si es fiado) el detalle del deudor.
        // Todo o nada. Reemplaza a la secuencia Agregar_venta + Descontar_Stock + Agregar_Venta_Producto (+ GuardarDetalleFiado),
        // que eran pasos separados y podían dejar una venta guardada sin sus líneas o sin descontar el stock.
        //
        // - 'carrito' tiene una entrada por unidad escaneada (igual que en la registradora).
        // - El precio de cada artículo se lee de la base DENTRO de la transacción.
        // - Si falta stock, la venta se registra igual (la mercadería ya está en la mano del cliente): el stock no se deja
        //   en negativo, se marca la línea para no devolverlo si después se borra la venta y se avisa en Advertencias.
        public ResultadoVenta RegistrarVentaCompleta(List<Articulos> carrito, int idMetodo, string nombreMetodo,
                                                     decimal subtotalSinRecargo, decimal totalConRecargo, int? idCliente = null)
        {
            if (carrito == null)
                throw new ArgumentNullException(nameof(carrito));

            List<Articulos> unidades = carrito.Where(a => a != null).ToList();
            if (unidades.Count == 0)
                throw new ArgumentException("La venta no tiene productos.", nameof(carrito));
            if (unidades.Any(a => a.Id <= 0))
                throw new ArgumentException("Hay productos sin identificar en la venta; no se guardó nada.", nameof(carrito));

            bool esFiado = idMetodo == IdsMetodoPago.Fiado;
            if (esFiado && !idCliente.HasValue)
                throw new ArgumentException("Una venta fiada necesita un cliente.", nameof(idCliente));
            if (!esFiado && idCliente.HasValue)
                throw new ArgumentException("Solo las ventas fiadas llevan cliente.", nameof(idCliente));

            var grupos = unidades.GroupBy(a => a.Id).Select(g => new { IdArticulo = g.Key, Cantidad = g.Count() }).ToList();

            var resultado = new ResultadoVenta();
            DateTime fecha = DateTime.Now;

            using (var conexion = new SqlConnection(ConexionConfig.Cadena))
            {
                conexion.Open();
                SqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    // 1) Método de pago (de acá sale el recargo que se muestra en el ticket)
                    object porcentajeBd = Escalar(conexion, transaccion,
                        "SELECT porcentaje FROM metodos_de_pago WHERE id_metodo = @idMetodo",
                        new SqlParameter("@idMetodo", idMetodo));
                    if (porcentajeBd == null || porcentajeBd == DBNull.Value)
                        throw new InvalidOperationException("El método de pago " + idMetodo + " no existe; no se guardó la venta.");
                    decimal porcentaje = Convert.ToDecimal(porcentajeBd);

                    // 2) Encabezado de la venta (acá queda registrado el pago: total y método)
                    object idNuevo = Escalar(conexion, transaccion,
                        "INSERT INTO Ventas (Fecha, Total, id_metodo, IdCliente) VALUES (@fecha, @total, @idMetodo, @idCliente); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS int);",
                        new SqlParameter("@fecha", fecha),
                        new SqlParameter("@total", totalConRecargo),
                        new SqlParameter("@idMetodo", idMetodo),
                        new SqlParameter("@idCliente", (object)idCliente ?? DBNull.Value));
                    int idVenta = Convert.ToInt32(idNuevo);

                    var lineasTicket = new List<Venta_Por_Producto>();

                    foreach (var grupo in grupos)
                    {
                        // 3) Datos actuales del artículo
                        string codigo, nombre;
                        decimal precio, costo;
                        using (var cmd = Comando(conexion, transaccion,
                            "SELECT Codigo, Nombre, Precio, PrecioCosto FROM ARTICULOS WHERE Id = @id",
                            new SqlParameter("@id", grupo.IdArticulo)))
                        using (var lector = cmd.ExecuteReader())
                        {
                            if (!lector.Read())
                                throw new InvalidOperationException("El artículo " + grupo.IdArticulo + " ya no existe; no se guardó la venta.");

                            codigo = lector["Codigo"] as string ?? string.Empty;
                            nombre = lector["Nombre"] as string ?? string.Empty;
                            precio = Convert.ToDecimal(lector["Precio"]);
                            costo = Convert.ToDecimal(lector["PrecioCosto"]);
                        }

                        // Lo que SQL Server va a guardar (redondea a 2 decimales): el ticket se arma con esos mismos valores.
                        decimal precioGuardado = Math.Round(precio, 2, MidpointRounding.AwayFromZero);
                        decimal importeBase = Math.Round(precio * grupo.Cantidad, 2, MidpointRounding.AwayFromZero);

                        // 4) Línea de la venta
                        int idLinea = Convert.ToInt32(Escalar(conexion, transaccion,
                            "INSERT INTO Venta_Por_Producto (IdVenta, IdArticulo, Fecha, Producto, Cantidad, PrecioUnitario, ImporteLinea) " +
                            "VALUES (@idVenta, @idArticulo, @fecha, @producto, @cantidad, @precio, @importe); " +
                            "SELECT CAST(SCOPE_IDENTITY() AS int);",
                            new SqlParameter("@idVenta", idVenta),
                            new SqlParameter("@idArticulo", grupo.IdArticulo),
                            new SqlParameter("@fecha", fecha),
                            new SqlParameter("@producto", nombre),
                            new SqlParameter("@cantidad", grupo.Cantidad),
                            new SqlParameter("@precio", precio),
                            new SqlParameter("@importe", importeBase)));

                        // 5) Stock: la condición Stock >= cantidad evita dejarlo en negativo y es segura aunque
                        //    otra caja venda lo mismo al mismo tiempo.
                        int descontadas = Ejecutar(conexion, transaccion,
                            "UPDATE ARTICULOS SET Stock = Stock - @cantidad WHERE Id = @id AND Stock >= @cantidad",
                            new SqlParameter("@cantidad", grupo.Cantidad),
                            new SqlParameter("@id", grupo.IdArticulo));

                        if (descontadas == 0)
                        {
                            resultado.Advertencias.Add("Stock insuficiente: se vendieron " + grupo.Cantidad + " unidad(es) de '" + nombre +
                                "' (código '" + codigo + "', venta " + idVenta + ") y no se descontó nada del stock.");

                            // Se anota en la línea para no "devolver" después un stock que nunca se descontó.
                            // (Condicional: sigue funcionando en una base que todavía no tiene la columna.)
                            Ejecutar(conexion, transaccion,
                                "IF COL_LENGTH('dbo.Venta_Por_Producto', 'StockDescontado') IS NOT NULL " +
                                "BEGIN " +
                                "DECLARE @orden nvarchar(200) = N'UPDATE dbo.Venta_Por_Producto SET StockDescontado = 0 WHERE Id = ' + CAST(@idLinea AS nvarchar(20)); " +
                                "EXEC (@orden); " +
                                "END",
                                new SqlParameter("@idLinea", idLinea));
                        }

                        // 6) Fiado: detalle de lo que se llevó el deudor
                        if (esFiado)
                        {
                            Ejecutar(conexion, transaccion,
                                "INSERT INTO DETALLE_COMPRAS_FIADO (IdVenta, IdProducto, Cantidad, PrecioCostoUnitario, PrecioVentaUnitario) " +
                                "VALUES (@idVenta, @codigo, @cantidad, @costo, @precio)",
                                new SqlParameter("@idVenta", idVenta),
                                new SqlParameter("@codigo", codigo),
                                new SqlParameter("@cantidad", grupo.Cantidad),
                                new SqlParameter("@costo", costo),
                                new SqlParameter("@precio", precio));
                        }

                        lineasTicket.Add(new Venta_Por_Producto
                        {
                            Id = idLinea,
                            IdVenta = idVenta,
                            Fecha = fecha,
                            Producto = nombre,
                            Cantidad = grupo.Cantidad,
                            PrecioUnitario = precioGuardado,
                            Recargo = CalculadoraVenta.RecargoDeLinea(importeBase, porcentaje),
                            ImporteLinea = CalculadoraVenta.ImporteDeLineaConRecargo(importeBase, porcentaje)
                        });
                    }

                    transaccion.Commit();

                    resultado.IdVenta = idVenta;
                    resultado.Ticket = new Venta_Por_Producto_Negocio().FinalizarVentaYPrepararImpresion(
                        idVenta, fecha, lineasTicket, subtotalSinRecargo, nombreMetodo, totalConRecargo);
                }
                catch
                {
                    Deshacer(transaccion);
                    throw;
                }
            }

            // Fuera de la transacción: el aviso nunca puede deshacer ni frenar una venta ya guardada.
            foreach (string advertencia in resultado.Advertencias)
                RegistroErrores.Registrar(advertencia, "Descuento de stock");

            return resultado;
        }

        // Borra una venta dejando constancia: QUIÉN, CUÁNDO y POR QUÉ, con una copia de la venta y de sus líneas.
        // Devuelve el stock de las líneas que lo habían descontado. Todo en una sola transacción.
        public ResultadoEliminacion EliminarVentaConAuditoria(int idVenta, string motivo, string usuario = null, string equipo = null)
        {
            string motivoLimpio = (motivo ?? string.Empty).Trim();
            if (motivoLimpio.Length < LargoMinimoMotivo)
                throw new ArgumentException("Hay que indicar el motivo por el que se elimina la venta.", nameof(motivo));
            if (motivoLimpio.Length > 300)
                motivoLimpio = motivoLimpio.Substring(0, 300);

            if (string.IsNullOrWhiteSpace(usuario))
                usuario = Environment.UserDomainName + "\\" + Environment.UserName;
            if (string.IsNullOrWhiteSpace(equipo))
                equipo = Environment.MachineName;

            var resultado = new ResultadoEliminacion { IdVenta = idVenta };

            using (var conexion = new SqlConnection(ConexionConfig.Cadena))
            {
                conexion.Open();
                SqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    // 1) La venta (bloqueada hasta el final: si dos personas la borran a la vez, la segunda no la encuentra)
                    DateTime fechaVenta;
                    decimal total;
                    object idMetodo, metodoPago, idCliente;
                    using (var cmd = Comando(conexion, transaccion,
                        "SELECT V.Fecha, V.Total, V.id_metodo, M.metodo_pago, V.IdCliente " +
                        "FROM Ventas V WITH (UPDLOCK, HOLDLOCK) LEFT JOIN metodos_de_pago M ON M.id_metodo = V.id_metodo " +
                        "WHERE V.Id = @id",
                        new SqlParameter("@id", idVenta)))
                    using (var lector = cmd.ExecuteReader())
                    {
                        if (!lector.Read())
                            throw new InvalidOperationException("La venta " + idVenta + " no existe (puede que ya haya sido eliminada).");

                        fechaVenta = (DateTime)lector["Fecha"];
                        total = Convert.ToDecimal(lector["Total"]);
                        idMetodo = lector["id_metodo"];
                        metodoPago = lector["metodo_pago"];
                        idCliente = lector["IdCliente"];
                    }

                    // 2) Sus líneas
                    var lineas = new List<LineaAEliminar>();
                    using (var cmd = Comando(conexion, transaccion,
                        "SELECT IdArticulo, Producto, Cantidad, PrecioUnitario, ImporteLinea, StockDescontado " +
                        "FROM Venta_Por_Producto WHERE IdVenta = @id",
                        new SqlParameter("@id", idVenta)))
                    using (var lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lineas.Add(new LineaAEliminar
                            {
                                IdArticulo = lector["IdArticulo"] == DBNull.Value ? (int?)null : Convert.ToInt32(lector["IdArticulo"]),
                                Producto = lector["Producto"] as string ?? string.Empty,
                                Cantidad = Convert.ToInt32(lector["Cantidad"]),
                                PrecioUnitario = Convert.ToDecimal(lector["PrecioUnitario"]),
                                ImporteLinea = lector["ImporteLinea"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(lector["ImporteLinea"]),
                                StockDescontado = Convert.ToBoolean(lector["StockDescontado"])
                            });
                        }
                    }

                    // 3) Constancia: encabezado
                    resultado.IdAuditoria = Convert.ToInt32(Escalar(conexion, transaccion,
                        "INSERT INTO VENTAS_ELIMINADAS (IdVenta, FechaVenta, Total, IdMetodo, MetodoPago, IdCliente, Usuario, Equipo, Motivo) " +
                        "VALUES (@idVenta, @fecha, @total, @idMetodo, @metodo, @idCliente, @usuario, @equipo, @motivo); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS int);",
                        new SqlParameter("@idVenta", idVenta),
                        new SqlParameter("@fecha", fechaVenta),
                        new SqlParameter("@total", total),
                        new SqlParameter("@idMetodo", idMetodo),
                        new SqlParameter("@metodo", metodoPago),
                        new SqlParameter("@idCliente", idCliente),
                        new SqlParameter("@usuario", usuario),
                        new SqlParameter("@equipo", equipo),
                        new SqlParameter("@motivo", motivoLimpio)));

                    // 4) Constancia: líneas, y devolución del stock
                    foreach (LineaAEliminar linea in lineas)
                    {
                        bool repuesto = false;
                        if (linea.IdArticulo.HasValue && linea.StockDescontado)
                        {
                            repuesto = Ejecutar(conexion, transaccion,
                                "UPDATE ARTICULOS SET Stock = Stock + @cantidad WHERE Id = @id",
                                new SqlParameter("@cantidad", linea.Cantidad),
                                new SqlParameter("@id", linea.IdArticulo.Value)) > 0;
                        }

                        if (repuesto)
                            resultado.UnidadesRepuestas += linea.Cantidad;

                        Ejecutar(conexion, transaccion,
                            "INSERT INTO VENTAS_ELIMINADAS_DETALLE (IdAuditoria, IdArticulo, Producto, Cantidad, PrecioUnitario, ImporteLinea, StockRepuesto) " +
                            "VALUES (@auditoria, @idArticulo, @producto, @cantidad, @precio, @importe, @repuesto)",
                            new SqlParameter("@auditoria", resultado.IdAuditoria),
                            new SqlParameter("@idArticulo", (object)linea.IdArticulo ?? DBNull.Value),
                            new SqlParameter("@producto", linea.Producto),
                            new SqlParameter("@cantidad", linea.Cantidad),
                            new SqlParameter("@precio", linea.PrecioUnitario),
                            new SqlParameter("@importe", (object)linea.ImporteLinea ?? DBNull.Value),
                            new SqlParameter("@repuesto", repuesto));
                    }

                    // 5) Recién ahora se borra la venta
                    Ejecutar(conexion, transaccion, "DELETE FROM DETALLE_COMPRAS_FIADO WHERE IdVenta = @id", new SqlParameter("@id", idVenta));
                    Ejecutar(conexion, transaccion, "DELETE FROM Venta_Por_Producto WHERE IdVenta = @id", new SqlParameter("@id", idVenta));
                    int borradas = Ejecutar(conexion, transaccion, "DELETE FROM Ventas WHERE Id = @id", new SqlParameter("@id", idVenta));
                    if (borradas != 1)
                        throw new InvalidOperationException("No se pudo eliminar la venta " + idVenta + ".");

                    transaccion.Commit();
                }
                catch (SqlException ex) when (ex.Number == 207 || ex.Number == 208)
                {
                    // 207/208: columna u objeto inexistente => la base es de una versión anterior.
                    Deshacer(transaccion);
                    throw new InvalidOperationException(
                        "La base de datos todavía no tiene las tablas de auditoría de ventas. " +
                        "Ejecutá el instalador (INSTALAR.bat) o el script BaseDeDatos\\CATALOGO_DB_RECUPERADA.sql y probá de nuevo.", ex);
                }
                catch
                {
                    Deshacer(transaccion);
                    throw;
                }
            }

            return resultado;
        }

        // Las ventas eliminadas, de la más reciente a la más vieja (para la pantalla de auditoría).
        public System.Data.DataTable ListarVentasEliminadas(int maximo = 500)
        {
            var datos = new AccesoDATOS();
            try
            {
                datos.SetearConsulta(
                    "SELECT TOP (@maximo) E.FechaEliminacion, E.Usuario, E.Equipo, E.IdVenta, E.FechaVenta, E.Total, E.MetodoPago, E.Motivo, " +
                    "  ISNULL((SELECT SUM(D.Cantidad) FROM VENTAS_ELIMINADAS_DETALLE D WHERE D.IdAuditoria = E.IdAuditoria AND D.StockRepuesto = 1), 0) AS UnidadesRepuestas, " +
                    "  STUFF((SELECT ', ' + CAST(D2.Cantidad AS varchar(10)) + ' x ' + D2.Producto " +
                    "         FROM VENTAS_ELIMINADAS_DETALLE D2 WHERE D2.IdAuditoria = E.IdAuditoria " +
                    "         FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 2, '') AS Productos " +
                    "FROM VENTAS_ELIMINADAS E ORDER BY E.FechaEliminacion DESC, E.IdAuditoria DESC");
                datos.setearParametro("@maximo", maximo);
                return datos.EjecutarLecturanuevo();
            }
            catch (Exception ex)
            {
                SqlException sql = ex.InnerException as SqlException;
                if (sql != null && (sql.Number == 207 || sql.Number == 208))
                    throw new InvalidOperationException(
                        "La base de datos todavía no tiene las tablas de auditoría de ventas. " +
                        "Ejecutá el instalador (INSTALAR.bat) o el script BaseDeDatos\\CATALOGO_DB_RECUPERADA.sql y probá de nuevo.", ex);
                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        private class LineaAEliminar
        {
            public int? IdArticulo;
            public string Producto;
            public int Cantidad;
            public decimal PrecioUnitario;
            public decimal? ImporteLinea;
            public bool StockDescontado;
        }

        // ---------- Ayudas para trabajar sobre la conexión/transacción abierta ----------

        private static SqlCommand Comando(SqlConnection conexion, SqlTransaction transaccion, string sql, params SqlParameter[] parametros)
        {
            var cmd = new SqlCommand(sql, conexion, transaccion);
            foreach (SqlParameter p in parametros)
            {
                if (p.Value == null)
                    p.Value = DBNull.Value;
                cmd.Parameters.Add(p);
            }
            return cmd;
        }

        private static object Escalar(SqlConnection conexion, SqlTransaction transaccion, string sql, params SqlParameter[] parametros)
        {
            using (SqlCommand cmd = Comando(conexion, transaccion, sql, parametros))
                return cmd.ExecuteScalar();
        }

        private static int Ejecutar(SqlConnection conexion, SqlTransaction transaccion, string sql, params SqlParameter[] parametros)
        {
            using (SqlCommand cmd = Comando(conexion, transaccion, sql, parametros))
                return cmd.ExecuteNonQuery();
        }

        // Si el rollback también falla (conexión cortada), SQL Server igual deshace la transacción al cerrarse la conexión.
        private static void Deshacer(SqlTransaction transaccion)
        {
            try { transaccion.Rollback(); }
            catch { }
        }
    }
}
