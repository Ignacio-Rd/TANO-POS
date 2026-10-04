using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocios;
using Negocio_;
using Dominio;
using System.Data;
using System.Data.SqlClient;



namespace Negocio_
{
    public class Venta_Por_Producto_Negocio
    {


        public List<Venta_Por_Producto> listarProductos()
        {
            List<Venta_Por_Producto> lista = new List<Venta_Por_Producto>();

            AccesoDATOS datos = new AccesoDATOS();

            try
            {

                datos.SetearConsulta(
                    "SELECT VP.Fecha, VP.Producto, VP.Cantidad, VP.PrecioUnitario, VP.ImporteLinea " +
                    "FROM Venta_Por_Producto VP " +
                    "INNER JOIN Ventas V ON V.Id = VP.IdVenta"
                );
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Venta_Por_Producto aux = new Venta_Por_Producto();
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];
                    aux.Producto = (string)datos.Lector["Producto"];
                    aux.Cantidad = (int)datos.Lector["Cantidad"];
                    aux.PrecioUnitario = Convert.ToDecimal(datos.Lector["PrecioUnitario"]);
                    aux.ImporteLinea = Convert.ToDecimal(datos.Lector["ImporteLinea"]);
                    lista.Add(aux);
                }

                return lista;

            }

            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }

        }
        public TICKET Agregar_Venta_Producto(List<Articulos> producto_Vendidos, DateTime fecha, int idVenta, decimal TOTAL, string impuestos, decimal TOTALFINAL)
        {

            if (producto_Vendidos != null || producto_Vendidos.Count > 0)
            {
            }
            Articulos art = new Articulos();

            ArticuloNegocio artNegocio = new ArticuloNegocio();

            // Diccionario para almacenar el ID del Artículo como clave, y la Cantidad acumulada como valor.
            Dictionary<int, int> cantidadesAcumuladas = new Dictionary<int, int>();

            // Necesitamos una referencia a los detalles del artículo (Nombre, Precio)
            // Usaremos un diccionario para almacenar los datos únicos del artículo.
            Dictionary<int, Articulos> detallesArticulos = new Dictionary<int, Articulos>();


            // 1. Acumulamos cantidades y guardamos una referencia de los datos


            foreach (var producto in producto_Vendidos)
            {

                if (producto == null || producto.Id == 0) continue;

                int idArticulo = producto.Id;

                // A) Acumular Cantidad
                if (cantidadesAcumuladas.ContainsKey(idArticulo))
                {
                    cantidadesAcumuladas[idArticulo] += 1;
                }
                else
                {
                    cantidadesAcumuladas.Add(idArticulo, 1);

                    // B) Guardar una referencia del objeto Articulos (tiene Nombre y Precio)
                    if (!detallesArticulos.ContainsKey(idArticulo))
                    {
                        detallesArticulos.Add(idArticulo, producto);
                    }
                }


            }

            // 2. Recorremos los artículos únicos (por ID)
            foreach (var idArticulo in cantidadesAcumuladas.Keys)
            {
                int cantidadVendida = cantidadesAcumuladas[idArticulo];
                Articulos detalle = detallesArticulos[idArticulo];

                // 3. 🚨 OBTENEMOS EL PRECIO 🚨
                // Suponemos que tu objeto Articulos (detalle) tiene la propiedad Precio,
                // o la consultamos aquí si no la tiene:
                decimal precioUnitario = artNegocio.ObtenerPrecioPorId(idArticulo);
                decimal Importe = precioUnitario * cantidadVendida;

                // El resto de tu lógica para InsertarVenta (sin la lógica obsoleta de ActualizarVenta)

                InsertarVenta(
                    idVenta,
                    idArticulo,
                    fecha,
                    detalle.Nombre,
                    cantidadVendida,
                    precioUnitario,
                    Importe

                );


            }

            TICKET ticketFinal = FinalizarVentaYPrepararImpresion(idVenta, fecha, ObtenerDetallePorId(idVenta), TOTAL, impuestos, TOTALFINAL);

            return ticketFinal;






        }


        private int ObtenerCantidad(DateTime fecha, string producto)
        {
            AccesoDATOS datos = new AccesoDATOS();
            {
                datos.SetearConsultaClear("SELECT Cantidad FROM Venta_Por_Producto WHERE Fecha = @fecha AND Producto = @producto");
                datos.setearParametro("@fecha", fecha);
                datos.setearParametro("@producto", producto);
                datos.EjecutarLectura();

                int cantidad = 0;
                if (datos.Lector.Read())
                    cantidad = (int)datos.Lector["Cantidad"];

                datos.Lector.Close();
                return cantidad;
            }
        }

        private void ActualizarVenta(DateTime fecha, string producto, int cantidad)
        {
            AccesoDATOS datos = new AccesoDATOS();
            {
                datos.SetearConsultaClear("UPDATE Venta_Por_Producto SET Cantidad = @cantidad WHERE Fecha = @fecha AND Producto = @producto");
                datos.setearParametro("@cantidad", cantidad);
                datos.setearParametro("@fecha", fecha);
                datos.setearParametro("@producto", producto);
                datos.EjecutarAccionNUEVO();
            }
        }

        private void InsertarVenta(int idVenta, int idArticulo, DateTime fecha, string producto, int cantidad, decimal precioUnitario, decimal Importe)
        {
            AccesoDATOS datos = new AccesoDATOS();
            {
                Console.WriteLine($"Insertando -> IdVenta: {idVenta}, IdArticulo: {idArticulo}, Producto: {producto}, Cantidad: {cantidad}, Precio: {precioUnitario}");

                // 🚨 CONSULTA FINAL CON LAS NUEVAS COLUMNAS 🚨
                datos.SetearConsultaClear("INSERT INTO Venta_Por_Producto(IdVenta, IdArticulo, Fecha, Producto, Cantidad, PrecioUnitario, ImporteLinea) VALUES (@idVenta, @idArticulo, @fecha, @producto, @cantidad, @precioUnitario, @Importe)");

                // Seteo de los 6 parámetros
                datos.setearParametro("@idVenta", idVenta);
                datos.setearParametro("@idArticulo", idArticulo); // Nuevo
                datos.setearParametro("@fecha", fecha);
                datos.setearParametro("@producto", producto);
                datos.setearParametro("@cantidad", cantidad);
                datos.setearParametro("@PrecioUnitario", precioUnitario); // Nuevo
                datos.setearParametro("@Importe", Importe);

                datos.EjecutarAccionNUEVO();
            }
        }

        public List<Venta_Por_Producto> ObtenerDetallePorId(int idVenta)
        {
            AccesoDATOS datos = new AccesoDATOS();
            List<Venta_Por_Producto> productos = new List<Venta_Por_Producto>();

            try
            {
                // 1. QUERY CON JOIN: Traemos el porcentaje del método de pago de esa venta
                datos.SetearConsulta(@"
    SELECT 
        VP.Id, VP.Fecha, VP.IdVenta, VP.IdArticulo, VP.Producto, VP.Cantidad, 
        VP.PrecioUnitario, VP.ImporteLinea,
        ISNULL(M.porcentaje, 0) AS PorcentajeRecargo -- <--- ESTE NOMBRE ES CLAVE
    FROM Venta_Por_Producto VP
    INNER JOIN Ventas V ON VP.IdVenta = V.Id
    LEFT JOIN metodos_de_pago M ON V.id_metodo = M.id_metodo
    WHERE VP.IdVenta = @id");

                datos.setearParametro("@id", idVenta);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Venta_Por_Producto aux = new Venta_Por_Producto();

                    // Mapeos básicos - Usamos nombres directos para evitar errores de Ordinal
                    aux.Id = (int)datos.Lector["Id"];
                    aux.IdVenta = (int)datos.Lector["IdVenta"];
                    aux.Producto = datos.Lector["Producto"].ToString();
                    aux.Cantidad = (int)datos.Lector["Cantidad"];
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];

                    // --- LÓGICA DE RECARGO ---
                    // Leemos los valores base
                    decimal porcentaje = Convert.ToDecimal(datos.Lector["PorcentajeRecargo"]);
                    decimal precioBase = Convert.ToDecimal(datos.Lector["PrecioUnitario"]);
                    decimal importeBase = Convert.ToDecimal(datos.Lector["ImporteLinea"]);

                    // Calculamos
                    decimal montoRecargo = importeBase * (porcentaje / 100);

                    // ASIGNACIÓN AL OBJETO (Asegurate que estos nombres coincidan con tu clase Dominio)
                    aux.Recargo = Math.Round(montoRecargo, 2);
                    aux.PrecioUnitario = Math.Round(precioBase, 2);
                    aux.ImporteLinea = Math.Round(importeBase + montoRecargo, 2);

                    // UN SOLO ADD
                    productos.Add(aux);
                }

                datos.Lector.Close();
                return productos;
            }
            catch (Exception ex)
            {
                // Si el problema persiste, es un error de lectura de NULLs (DBNull).
                // Si las columnas FK (IdVenta, IdArticulo) aceptan NULLs en la BD, se necesita un check de IsDBNull.
                throw new Exception("Error al obtener detalle de venta: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public TICKET FinalizarVentaYPrepararImpresion(int id_venta, DateTime fecha, List<Venta_Por_Producto> productos, decimal total, string impuesto, decimal TotalFinal)
        {
            TICKET ticket = new TICKET();

            ticket.IdVenta = id_venta;
            ticket.fechaVenta = fecha;
            ticket.venta_Por_Productos = productos;
            ticket.total = total;
            ticket.impuestos = impuesto;
            ticket.TotalFinal = TotalFinal;

            return ticket;
        }

        public void Buscar_Producto_Cantidad(DateTime fecha)
        {
            AccesoDATOS datos = new AccesoDATOS();

            datos.SetearConsulta("select Producto, Cantidad");

        }

        // ... dentro de tu clase o evento (ej: un clic de botón)

        public DataTable CargarVentasEnDataGridView(string IdArticulo, DateTime FechaInicio, DateTime FechaFinal)
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                // 1. Unimos Venta_Por_Producto con ARTICULOS para filtrar por Código de Barras
                // 2. Unimos con Ventas para las fechas y con metodos_de_pago para los recargos
                datos.SetearConsulta(@"
            SELECT 
                ISNULL(SUM(VP.Cantidad), 0) AS UnidadesVendidas, 
                ISNULL(SUM(VP.ImporteLinea + (VP.ImporteLinea * ISNULL(M.porcentaje, 0) / 100)), 0) AS TotalRecaudado
            FROM Venta_Por_Producto VP
            INNER JOIN Ventas V ON VP.IdVenta = V.Id
            INNER JOIN ARTICULOS A ON VP.IdArticulo = A.Id
            LEFT JOIN metodos_de_pago M ON V.id_metodo = M.id_metodo
            WHERE A.Codigo = @codigoBarra 
              AND V.Fecha >= @fechainicio 
              AND V.Fecha <= @fechafinal
              AND M.id_metodo <> 11 ");

                datos.setearParametro("@codigoBarra", IdArticulo);
                datos.setearParametro("@fechainicio", FechaInicio);
                datos.setearParametro("@fechafinal", FechaFinal);

                return datos.EjecutarLecturanuevo();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar por código de barras: " + ex.Message);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void GuardarDetalleFiado(List<Articulos> productosVendidos, int idVenta)
        {
            if (productosVendidos == null || productosVendidos.Count == 0) return;

            ArticuloNegocio artNegocio = new ArticuloNegocio();

            // Acumular cantidades igual que en Agregar_Venta_Producto
            Dictionary<int, int> cantidadesAcumuladas = new Dictionary<int, int>();
            Dictionary<int, Articulos> detallesArticulos = new Dictionary<int, Articulos>();

            foreach (var producto in productosVendidos)
            {
                if (producto == null || producto.Id == 0) continue;

                int idArticulo = producto.Id;

                if (cantidadesAcumuladas.ContainsKey(idArticulo))
                {
                    cantidadesAcumuladas[idArticulo] += 1;
                }
                else
                {
                    cantidadesAcumuladas.Add(idArticulo, 1);
                    if (!detallesArticulos.ContainsKey(idArticulo))
                        detallesArticulos.Add(idArticulo, producto);
                }
            }

            // Insertar cada producto en DETALLE_COMPRAS_FIADO
            foreach (var idArticulo in cantidadesAcumuladas.Keys)
            {
                int cantidad = cantidadesAcumuladas[idArticulo];
                Articulos detalle = detallesArticulos[idArticulo];

                decimal precioCosto = artNegocio.ObtenerPrecioCostoPorId(detalle.codigo.ToString());
                decimal precioVenta = artNegocio.ObtenerPrecioPorId(idArticulo);

                AccesoDATOS datos = new AccesoDATOS();
                try
                {
                    datos.SetearConsultaClear(@"INSERT INTO DETALLE_COMPRAS_FIADO 
                (IdVenta, IdProducto, Cantidad, PrecioCostoUnitario, PrecioVentaUnitario) 
                VALUES (@idVenta, @idProducto, @cantidad, @precioCosto, @precioVenta)");

                    datos.setearParametro("@idVenta", idVenta);
                    datos.setearParametro("@idProducto", detalle.codigo.ToString());
                    datos.setearParametro("@cantidad", cantidad);
                    datos.setearParametro("@precioCosto", precioCosto);
                    datos.setearParametro("@precioVenta", precioVenta);

                    datos.EjecutarAccionNUEVO();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al guardar detalle fiado: " + ex.Message, ex);
                }
                finally
                {
                    datos.CerrarConexion();
                }
            }
        }

        public InformeCaja ObtenerResumenInforme(DateTime inicio, DateTime fin)
        {
            InformeCaja informe = new InformeCaja();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                // =============================================
                // QUERY 1: TOTALES PRINCIPALES (EFECTIVO)
                // =============================================
                datos.SetearConsulta(@"
SELECT 
    -- EFECTIVO ENTRANTE (Ventas en efectivo + Cobros deudores en efectivo)
    ((SELECT ISNULL(SUM(Total), 0) FROM Ventas V 
      INNER JOIN metodos_de_pago M ON V.id_metodo = M.id_metodo 
      WHERE M.metodo_pago = 'Efectivo' AND V.Fecha BETWEEN @inicio AND @fin)
    +
    (SELECT ISNULL(SUM(P.Monto), 0) FROM PAGOS_DEUDORES P
      INNER JOIN metodos_de_pago M ON P.IdMetodoPago = M.id_metodo
      WHERE M.metodo_pago = 'Efectivo' AND P.Fecha BETWEEN @inicio AND @fin)) as TotalEntradaEfectivo,

    -- SALIDA EFECTIVO: Proveedores
    (SELECT ISNULL(SUM(PAG.Monto), 0) 
     FROM PAGOS_PROVEEDORES PAG
     INNER JOIN metodos_de_pago M ON PAG.IdMetodoPago = M.id_metodo
     WHERE M.metodo_pago = 'Efectivo' AND PAG.Fecha BETWEEN @inicio AND @fin) as PagosProveedoresEfectivo,

    -- SALIDA EFECTIVO: Gastos Varios
    (SELECT ISNULL(SUM(PV.Monto), 0) 
     FROM PAGOS_VARIOS PV
     INNER JOIN metodos_de_pago M ON PV.IdMetodoPago = M.id_metodo
     WHERE M.metodo_pago = 'Efectivo' AND PV.Fecha BETWEEN @inicio AND @fin) as GastosVariosEfectivo,

    -- COBROS TOTALES (todos los métodos)
    (SELECT ISNULL(SUM(Monto), 0) FROM PAGOS_DEUDORES 
     WHERE Fecha BETWEEN @inicio AND @fin) as CobrosTotales,

    -- FIADO NUEVO
    (SELECT ISNULL(SUM(Total), 0) FROM Ventas V 
     INNER JOIN metodos_de_pago M ON V.id_metodo = M.id_metodo 
     WHERE M.metodo_pago = 'FIADO' AND V.Fecha BETWEEN @inicio AND @fin) as FiadoNuevo,

    -- SALDO HISTÓRICO REAL
    (SELECT ISNULL(SUM(SubSaldos.SaldoIndividual), 0)
     FROM (
        SELECT 
            (SELECT ISNULL(SUM(Total), 0) FROM VENTAS WHERE IdCliente = C.Id AND id_metodo = 11) - 
            (SELECT ISNULL(SUM(Monto), 0) FROM PAGOS_DEUDORES WHERE IdCliente = C.Id) as SaldoIndividual
        FROM Clientes C 
        WHERE C.Estado = 1
     ) as SubSaldos) as SaldoTotalReal
");
                datos.setearParametro("@inicio", inicio);
                datos.setearParametro("@fin", fin);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    decimal entradasEfectivo = (decimal)datos.Lector["TotalEntradaEfectivo"];
                    informe.PagosAProveedores = (decimal)datos.Lector["PagosProveedoresEfectivo"];
                    informe.GastosVariosEfectivo = (decimal)datos.Lector["GastosVariosEfectivo"];

                    // Neto efectivo = Entradas - Proveedores - Gastos Varios
                    informe.EfectivoVentas = entradasEfectivo - informe.PagosAProveedores - informe.GastosVariosEfectivo;
                    informe.CobroDeudores = (decimal)datos.Lector["CobrosTotales"];
                    informe.VentasFiadasNuevas = (decimal)datos.Lector["FiadoNuevo"];
                    informe.SaldoHistorico = (decimal)datos.Lector["SaldoTotalReal"];
                }
                datos.Lector.Close();
                datos.CerrarConexion();

                // =============================================
                // QUERY 2: DESGLOSE MÉTODOS DIGITALES
                // Incluye: ventas + cobros deudores - proveedores - gastos varios
                // =============================================
                datos.LimpiarParametros();
                datos.SetearConsulta(@"
SELECT 
    M.metodo_pago,
    M.fondo_inicial,

    -- Entradas: Ventas digitales
    ISNULL((SELECT SUM(V.Total) FROM Ventas V 
            WHERE V.id_metodo = M.id_metodo 
            AND V.Fecha BETWEEN @inicio AND @fin), 0) as TotalVentas,

    -- Entradas: Cobros deudores digitales
    ISNULL((SELECT SUM(P.Monto) FROM PAGOS_DEUDORES P 
            WHERE P.IdMetodoPago = M.id_metodo 
            AND P.Fecha BETWEEN @inicio AND @fin), 0) as TotalCobros,

    -- Salidas: Pagos proveedores con este método
    ISNULL((SELECT SUM(PAG.Monto) FROM PAGOS_PROVEEDORES PAG 
            WHERE PAG.IdMetodoPago = M.id_metodo 
            AND PAG.Fecha BETWEEN @inicio AND @fin), 0) as TotalProveedores,

    -- Salidas: Gastos varios con este método
    ISNULL((SELECT SUM(PV.Monto) FROM PAGOS_VARIOS PV 
            WHERE PV.IdMetodoPago = M.id_metodo 
            AND PV.Fecha BETWEEN @inicio AND @fin), 0) as TotalGastosVarios

FROM metodos_de_pago M
WHERE M.metodo_pago NOT IN ('Efectivo', 'FIADO')
AND (
    EXISTS(SELECT 1 FROM Ventas V WHERE V.id_metodo = M.id_metodo AND V.Fecha BETWEEN @inicio AND @fin)
    OR EXISTS(SELECT 1 FROM PAGOS_DEUDORES P WHERE P.IdMetodoPago = M.id_metodo AND P.Fecha BETWEEN @inicio AND @fin)
    OR EXISTS(SELECT 1 FROM PAGOS_PROVEEDORES PAG WHERE PAG.IdMetodoPago = M.id_metodo AND PAG.Fecha BETWEEN @inicio AND @fin)
    OR EXISTS(SELECT 1 FROM PAGOS_VARIOS PV WHERE PV.IdMetodoPago = M.id_metodo AND PV.Fecha BETWEEN @inicio AND @fin)
    OR M.fondo_inicial <> 0
)
");
                datos.setearParametro("@inicio", inicio);
                datos.setearParametro("@fin", fin);
                datos.EjecutarLectura();

                informe.DesgloseMetodos.Clear();
                while (datos.Lector.Read())
                {
                    decimal ventas = (decimal)datos.Lector["TotalVentas"];
                    decimal cobros = (decimal)datos.Lector["TotalCobros"];
                    decimal proveedores = (decimal)datos.Lector["TotalProveedores"];
                    decimal gastos = (decimal)datos.Lector["TotalGastosVarios"];
                    decimal fondo = (decimal)datos.Lector["fondo_inicial"];

                    informe.DesgloseMetodos.Add(new MetodoResumen
                    {
                        Nombre = datos.Lector["metodo_pago"].ToString(),
                        Total = ventas + cobros - proveedores - gastos,  // Neto
                        PagosProveedores = proveedores,
                        GastosVarios = gastos,
                        FondoInicial = fondo
                    });
                }

                datos.Lector.Close();
                datos.CerrarConexion();

                // Traer fondo inicial del efectivo
                datos.LimpiarParametros();
                datos.SetearConsulta("SELECT fondo_inicial FROM metodos_de_pago WHERE metodo_pago = 'Efectivo'");
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                    informe.FondoInicialEfectivo = (decimal)datos.Lector["fondo_inicial"];
                datos.Lector.Close();
                datos.CerrarConexion();

                // =============================================
                // CALCULAR TOTAL GENERAL RECAUDADO
                // Efectivo neto + suma de netos digitales
                // =============================================
                informe.TotalGeneralRecaudado = informe.EfectivoVentas
                    + informe.DesgloseMetodos.Sum(m => m.Total);

                return informe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}





