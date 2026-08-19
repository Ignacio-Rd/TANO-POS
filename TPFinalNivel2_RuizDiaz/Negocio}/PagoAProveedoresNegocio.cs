using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Negocios;
using Negocio_;
using System.Windows.Forms;

namespace Negocio_
{
   public class PagoAProveedoresNegocio
    {
        public List<PagoAProveedores> lista()
        {
            List<PagoAProveedores> list = new List<PagoAProveedores>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta("SELECT D.Id, P.Id AS IdProve, C.Id AS Nro_Compra, D.Monto, D.Fecha, M.id_metodo AS IdMetodo FROM PAGOS_PROVEEDORES D INNER JOIN proveedores P ON D.IdProveedor = P.Id INNER JOIN compras C ON D.IdCompra = C.Id INNER JOIN metodos_de_pago M ON D.IdMetodoPago = M.id_metodo");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    PagoAProveedores aux = new PagoAProveedores();

                    // Usamos los nombres exactos o los Alias del SELECT
                    aux.Id = (int)datos.Lector["Id"];

                    // Si en el SELECT pusiste "P.Id AS IdProve"
                    aux.IdProveedor = (int)datos.Lector["IdProve"];

                    // Si en el SELECT pusiste "C.Id AS IdCompra"

                    aux.Compra = new Compras();
                    aux.Compra.Id = (int)datos.Lector["Nro_Compra"];

                    aux.Monto = (decimal)datos.Lector["Monto"]; // ¡No te olvides del monto!
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];

                    // Si en el SELECT pusiste "M.id_metodo AS IdMetodo"
                    aux.IdMetodo = (int)datos.Lector["IdMetodo"];

                    list.Add(aux); // ¡Asegurate de agregarlo a tu lista!
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally { datos.CerrarConexion(); }
        }

       public List<PagoAProveedores> ListaPagosAProveedores(int id)
        {
            List<PagoAProveedores> list = new List<PagoAProveedores>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta(@"SELECT PP.Fecha, PP.Monto, MP.metodo_pago AS Metodo_Pago, P.Nombre, PP.IdCompra
               FROM PAGOS_PROVEEDORES PP 
               LEFT JOIN metodos_de_pago MP ON PP.IdMetodoPago = MP.id_metodo
               INNER JOIN PROVEEDORES P ON PP.IdProveedor = P.Id
               WHERE PP.IdProveedor = @id
                ORDER BY PP.Fecha DESC");
                datos.setearParametro("@id", id);
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    PagoAProveedores aux = new PagoAProveedores();
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];
                    aux.Monto = (decimal)datos.Lector["Monto"];
                    aux.metodopago = new Metodo_de_pago();
                    aux.metodopago.MetodoPago = datos.Lector["Metodo_Pago"] != DBNull.Value
                        ? (string)datos.Lector["Metodo_Pago"]
                        : "Sin método";
                    aux.NombreProveedor = new Proveedor();
                    aux.NombreProveedor.Nombre = (string)datos.Lector["Nombre"];
                    aux.Compra = new Compras();
                    aux.Compra.Id = datos.Lector["IdCompra"] != DBNull.Value ? (int)datos.Lector["IdCompra"] : 0;
                    list.Add(aux);
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally { datos.CerrarConexion(); }
        }

        public List<PagoAProveedores> ListaPagosAProveedoresPorFecha(int id, DateTime fecha)
        {
            List<PagoAProveedores> list = new List<PagoAProveedores>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta(@"SELECT PP.Fecha, PP.Monto, MP.metodo_pago AS Metodo_Pago, P.Nombre, PP.IdCompra
               FROM PAGOS_PROVEEDORES PP 
               LEFT JOIN metodos_de_pago MP ON PP.IdMetodoPago = MP.id_metodo 
               INNER JOIN PROVEEDORES P ON PP.IdProveedor = P.Id
               WHERE PP.IdProveedor = @id 
               AND PP.Fecha >= @fechaDesde 
               AND PP.Fecha < @fechaHasta
               ORDER BY PP.Fecha DESC");
                datos.setearParametro("@id", id);
                datos.setearParametro("@fechaDesde", fecha.Date);
                datos.setearParametro("@fechaHasta", fecha.Date.AddDays(1));
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    PagoAProveedores aux = new PagoAProveedores();
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];
                    aux.Monto = (decimal)datos.Lector["Monto"];
                    aux.metodopago = new Metodo_de_pago();
                    aux.metodopago.MetodoPago = datos.Lector["Metodo_Pago"] != DBNull.Value
                        ? (string)datos.Lector["Metodo_Pago"]
                        : "Sin método";
                    aux.NombreProveedor = new Proveedor();
                    aux.NombreProveedor.Nombre = (string)datos.Lector["Nombre"];
                    aux.Compra = new Compras();
                    aux.Compra.Id = datos.Lector["IdCompra"] != DBNull.Value ? (int)datos.Lector["IdCompra"] : 0;
                    list.Add(aux);
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally { datos.CerrarConexion(); }
        }


        public List<PagoAProveedores> listaPagos(DateTime fecha)
        {
            List<PagoAProveedores> list = new List<PagoAProveedores>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta(@"SELECT D.Id AS Folio_Pago,    P.Nombre AS Proveedor,
    C.Id AS Nro_Compra,
    C.Fecha AS Fecha_Compra,
    C.TotalCompra AS Total_Compra, -- ← AGREGAR ESTO
    D.Monto AS Monto_Pagado,
    D.Fecha AS Fecha_Pago,
    M.metodo_pago AS Metodo_Pago
FROM PAGOS_PROVEEDORES D
INNER JOIN PROVEEDORES P ON D.IdProveedor = P.Id
INNER JOIN COMPRAS C ON D.IdCompra = C.Id
LEFT JOIN metodos_de_pago M ON D.IdMetodoPago = M.id_metodo
WHERE CONVERT(DATE, D.Fecha) = @fecha");
                datos.setearParametro("@fecha", fecha.Date);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    PagoAProveedores aux = new PagoAProveedores();

                    // 1. Usar "Folio_Pago" en lugar de "Id"
                    aux.Id = (int)datos.Lector["Folio_Pago"];

                    // 2. Usar "Nro_Compra" para el ID de la compra
                    aux.Compra = new Compras();
                    aux.Compra.Id = (int)datos.Lector["Nro_Compra"];

                    // 3. Usar "Proveedor" en lugar de "Nombre"
                    aux.NombreProveedor = new Proveedor();
                    aux.NombreProveedor.Nombre = (string)datos.Lector["Proveedor"];

                    // 4. Usar "Monto_Pagado" y "Fecha_Pago"
                    aux.Compra.Total = datos.Lector["Total_Compra"] == DBNull.Value
         ? 0
         : (decimal)datos.Lector["Total_Compra"];  // ← ACÁ
                    aux.Monto = (decimal)datos.Lector["Monto_Pagado"];
                    aux.Fecha = (DateTime)datos.Lector["Fecha_Pago"];

                    // 5. Usar "Metodo_Pago" (el alias del SELECT)
                    aux.metodopago = new Metodo_de_pago();
                    aux.metodopago.MetodoPago = datos.Lector["Metodo_Pago"] != DBNull.Value
                        ? (string)datos.Lector["Metodo_Pago"]
                        : "Sin método";

                    list.Add(aux);
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally { datos.CerrarConexion(); }
        }





        

        public List<PagoAProveedores> ConsultaRango(DateTime horainicio, DateTime horafin)
        {
            List<PagoAProveedores> lista = new List<PagoAProveedores>();
            AccesoDATOS datos = new AccesoDATOS();
           ;

            try
            {

                string consulta = @"SELECT D.Id AS Folio_Pago, P.Nombre AS Proveedor, C.Id AS Nro_Compra, 
                    C.Fecha AS Fecha_Compra, C.TotalCompra AS Total_Compra, 
                    D.Monto AS Monto_Pagado, D.Fecha AS Fecha_Pago, M.metodo_pago AS Metodo_Pago 
                    FROM PAGOS_PROVEEDORES D 
                    INNER JOIN PROVEEDORES P ON D.IdProveedor = P.Id 
                    INNER JOIN COMPRAS C ON D.IdCompra = C.Id 
                    LEFT JOIN metodos_de_pago M ON D.IdMetodoPago = M.id_metodo 
                    WHERE D.Fecha BETWEEN @HoraInicio AND @HoraFin
                    ORDER BY D.Fecha DESC";

                datos.SetearConsulta(consulta);
                datos.setearParametro("@HoraInicio", horainicio);
                datos.setearParametro("@HoraFin", horafin);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    PagoAProveedores aux = new PagoAProveedores();

                    // 1. Usar "Folio_Pago" en lugar de "Id"
                    aux.Id = (int)datos.Lector["Folio_Pago"];

                    // 2. Usar "Nro_Compra" para el ID de la compra
                    aux.Compra = new Compras();
                    aux.Compra.Id = (int)datos.Lector["Nro_Compra"];

                    // 3. Usar "Proveedor" en lugar de "Nombre"
                    aux.NombreProveedor = new Proveedor();
                    aux.NombreProveedor.Nombre = (string)datos.Lector["Proveedor"];

                    // 4. Usar "Monto_Pagado" y "Fecha_Pago"
                    aux.Compra.Total = datos.Lector["Total_Compra"] == DBNull.Value
         ? 0
         : (decimal)datos.Lector["Total_Compra"];  // ← ACÁ
                    aux.Monto = (decimal)datos.Lector["Monto_Pagado"];
                    aux.Fecha = (DateTime)datos.Lector["Fecha_Pago"];

                    // 5. Usar "Metodo_Pago" (el alias del SELECT)
                    aux.metodopago = new Metodo_de_pago();
                    aux.metodopago.MetodoPago = datos.Lector["Metodo_Pago"] != DBNull.Value
                        ? (string)datos.Lector["Metodo_Pago"]
                        : "Sin método";

                    lista.Add(aux);
                }

                return lista;

                /*2. QUERY DE DEUDA HISTÓRICA
                datos.LimpiarParametros(); // Limpiamos por seguridad
                // 3. Obtener la Deuda Histórica Total (Saldo real acumulado)
                datos.SetearConsulta(@"
            SELECT 
            (ISNULL((SELECT SUM(Total) FROM Ventas V INNER JOIN metodos_de_pago M ON V.id_metodo = M.id_metodo WHERE M.metodo_pago = 'FIADO'), 0) - 
             ISNULL((SELECT SUM(Monto) FROM PAGOS_DEUDORES), 0)) as SaldoHistorico");

                datos.EjecutarLectura();
                if (datos.Lector.Read())
                {
                    deudaHistorica = (decimal)datos.Lector["SaldoHistorico"];
                }*/

               
            }
            catch (Exception ex) { throw ex; }
            finally { datos.CerrarConexion(); }
        }

        public List<DetalleCompra> listadetalle(int idCompra)
        {
            AccesoDATOS datos = new AccesoDATOS();
            List<DetalleCompra> detalleCompras = new List<DetalleCompra>();

            try
            {
                // IMPORTANTE: Agregamos P.Codigo AS IdProducto
                string consulta = @"
    SELECT 
        P.Codigo AS IdProducto, 
        P.Nombre, 
        D.cantidad, 
        D.PrecioCostoUnitario AS PrecioCosto, 
        (D.cantidad * D.PrecioCostoUnitario) AS Monto 
    FROM DETALLE_COMPRAS D
    INNER JOIN ARTICULOS P ON CAST(D.IdProducto AS VARCHAR) = CAST(P.Codigo AS VARCHAR)
    WHERE D.IdCompra = @idCompra";

                datos.SetearConsulta(consulta);
                datos.setearParametro("@idCompra", idCompra);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    DetalleCompra aux = new DetalleCompra();
                    aux.IdProducto = datos.Lector["IdProducto"].ToString();
                    aux.NombreProducto = datos.Lector["Nombre"].ToString();
                    aux.Cantidad = (int)datos.Lector["cantidad"];
                    
                    aux.PrecioCosto = (decimal)datos.Lector["PrecioCosto"];
                    aux.Monto = (decimal)datos.Lector["Monto"];
                    detalleCompras.Add(aux);
                }

               

                return detalleCompras;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return detalleCompras;
            }
            finally { datos.CerrarConexion(); }
        }

        public int BuscarIdPorNombre(string nombre)
        {
            AccesoDATOS datos = new AccesoDATOS();
            int idP = 0;
            try
            {
                datos.SetearConsulta("select id from Proveedores where Nombre = @nombre");
                datos.setearParametro("@nombre", nombre);
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                {
                    idP = (int)datos.Lector["id"];
                }

                return idP;
            }

            catch (Exception)
            {

                throw;
            }
            finally { datos.CerrarConexion(); }




        }

        public List<PagoAProveedores> buscarSoloPorNombre(string nombre)
        {
            List<PagoAProveedores> list = new List<PagoAProveedores>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta(@"SELECT D.Id AS Folio_Pago, P.Nombre AS Proveedor, C.Id AS Nro_Compra, 
                    C.Fecha AS Fecha_Compra, C.TotalCompra AS Total_Compra, 
                    D.Monto AS Monto_Pagado, D.Fecha AS Fecha_Pago, M.metodo_pago AS Metodo_Pago 
                    FROM PAGOS_PROVEEDORES D 
                    INNER JOIN PROVEEDORES P ON D.IdProveedor = P.Id 
                    INNER JOIN COMPRAS C ON D.IdCompra = C.Id 
                    LEFT JOIN metodos_de_pago M ON D.IdMetodoPago = M.id_metodo 
                    WHERE P.Nombre = @nombre
                    ORDER BY D.Fecha DESC");
                datos.setearParametro("@nombre", nombre);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    PagoAProveedores aux = new PagoAProveedores();

                    // 1. Usar "Folio_Pago" en lugar de "Id"
                    aux.Id = (int)datos.Lector["Folio_Pago"];

                    // 2. Usar "Nro_Compra" para el ID de la compra
                    aux.Compra = new Compras();
                    aux.Compra.Id = (int)datos.Lector["Nro_Compra"];

                    // 3. Usar "Proveedor" en lugar de "Nombre"
                    aux.NombreProveedor = new Proveedor();
                    aux.NombreProveedor.Nombre = (string)datos.Lector["Proveedor"];

                    // 4. Usar "Monto_Pagado" y "Fecha_Pago"
                    aux.Compra.Total = datos.Lector["Total_Compra"] == DBNull.Value
       ? 0
       : (decimal)datos.Lector["Total_Compra"];  // ← ACÁ
                    aux.Monto = (decimal)datos.Lector["Monto_Pagado"];
                    aux.Fecha = (DateTime)datos.Lector["Fecha_Pago"];

                    // 5. Usar "Metodo_Pago" (el alias del SELECT)
                    aux.metodopago = new Metodo_de_pago();
                    aux.metodopago.MetodoPago = datos.Lector["Metodo_Pago"] != DBNull.Value
                        ? (string)datos.Lector["Metodo_Pago"]
                        : "Sin método";

                    list.Add(aux);
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally { datos.CerrarConexion(); }
        }

        public List<PagoAProveedores> buscarPorNombreYFecha(DateTime fecha, string nombre)
        {
            List<PagoAProveedores> list = new List<PagoAProveedores>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta(@"SELECT D.Id AS Folio_Pago, P.Nombre AS Proveedor, C.Id AS Nro_Compra, 
                    C.Fecha AS Fecha_Compra, C.TotalCompra AS Total_Compra, 
                    D.Monto AS Monto_Pagado, D.Fecha AS Fecha_Pago, M.metodo_pago AS Metodo_Pago 
                    FROM PAGOS_PROVEEDORES D 
                    INNER JOIN PROVEEDORES P ON D.IdProveedor = P.Id 
                    INNER JOIN COMPRAS C ON D.IdCompra = C.Id 
                    LEFT JOIN metodos_de_pago M ON D.IdMetodoPago = M.id_metodo 
                    WHERE CONVERT(DATE, D.Fecha) = @fecha AND P.Nombre = @nombre
                    ORDER BY D.Fecha DESC");
                datos.setearParametro("@fecha", fecha.Date);
                datos.setearParametro("@nombre", nombre);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    PagoAProveedores aux = new PagoAProveedores();

                    // 1. Usar "Folio_Pago" en lugar de "Id"
                    aux.Id = (int)datos.Lector["Folio_Pago"];

                    // 2. Usar "Nro_Compra" para el ID de la compra
                    aux.Compra = new Compras();
                    aux.Compra.Id = (int)datos.Lector["Nro_Compra"];

                    // 3. Usar "Proveedor" en lugar de "Nombre"
                    aux.NombreProveedor = new Proveedor();
                    aux.NombreProveedor.Nombre = (string)datos.Lector["Proveedor"];

                    // 4. Usar "Monto_Pagado" y "Fecha_Pago"
                    aux.Compra.Total = datos.Lector["Total_Compra"] == DBNull.Value
        ? 0
        : (decimal)datos.Lector["Total_Compra"];  // ← ACÁ
                    aux.Monto = (decimal)datos.Lector["Monto_Pagado"];
                    aux.Fecha = (DateTime)datos.Lector["Fecha_Pago"];

                    // 5. Usar "Metodo_Pago" (el alias del SELECT)
                    aux.metodopago = new Metodo_de_pago();
                    aux.metodopago.MetodoPago = datos.Lector["Metodo_Pago"] != DBNull.Value
                        ? (string)datos.Lector["Metodo_Pago"]
                        : "Sin método";

                    list.Add(aux);
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally { datos.CerrarConexion(); }
        }

        public decimal obtenerDeudaProveedor(string nombre)
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                // La query que calcula (Total Compras - Total Pagos)
                string consulta = "SELECT (ISNULL(SUM(C.TotalCompra), 0) - ISNULL((SELECT SUM(PAG.Monto) FROM PAGOS_PROVEEDORES PAG INNER JOIN PROVEEDORES PRO ON PAG.IdProveedor = PRO.Id WHERE PRO.Nombre = @nombre), 0)) AS Deuda FROM COMPRAS C INNER JOIN PROVEEDORES P ON C.IdProveedor = P.Id WHERE P.Nombre = @nombre";

                datos.SetearConsulta(consulta);
                datos.setearParametro("@nombre", nombre);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    // Retornamos el resultado, si es nulo devolvemos 0
                    return datos.Lector["Deuda"] != DBNull.Value ? (decimal)datos.Lector["Deuda"] : 0;
                }
                return 0;
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

        public void PagarAProveedor(int idProveedor, decimal DineroAProveedor, int idmetodo)
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                // Solo registrás el pago, sin generar una compra nueva
                datos.SetearConsulta("INSERT INTO PAGOS_PROVEEDORES (IdProveedor, Monto, Fecha, IdMetodoPago) VALUES (@proveedor, @monto, GETDATE(), @idmetodo)");
                datos.setearParametro("@proveedor", idProveedor);
                datos.setearParametro("@monto", DineroAProveedor);
                datos.setearParametro("@idmetodo", idmetodo);
                datos.EjecutarAccion();
            }
            catch (Exception) { throw; }
            finally { datos.CerrarConexion(); }
        }

     

        

    }
}
