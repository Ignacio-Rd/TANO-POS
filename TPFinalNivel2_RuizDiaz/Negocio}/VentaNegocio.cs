using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Negocios;
using System.Data.SqlClient;


namespace Negocio_
{
    public class VentaNegocio
    {

        public List<Ventas> Listar_ventas()
        {
            List<Ventas> lista = new List<Ventas>();

            AccesoDATOS datos = new AccesoDATOS();

            try
            {

                datos.SetearConsulta("select V.Id, Fecha, Total, M.metodo_pago as MetodoPago, V.id_metodo AS IdMetodo from Ventas V,  metodos_de_pago M where V.id_metodo = M.id_metodo ORDER BY V.Fecha DESC;" +
                    " ");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {

                    Ventas aux = new Ventas();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];
                    aux.Total = (decimal)datos.Lector["Total"];
                    aux.Id_metodo = (int)datos.Lector["Idmetodo"];
                    aux.metododepago = new Metodo_de_pago();
                    aux.metododepago.MetodoPago = (string)datos.Lector["Metodopago"];

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



        // EN VentaNegocio.cs

        // El método cambia de void a int
        public int Agregar_venta(DateTime fecha, decimal Total, int id_metodo)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                // La consulta debe incluir SELECT SCOPE_IDENTITY()
                datos.SetearConsulta("insert into Ventas(Fecha, Total, id_metodo) values (@fecha, @total, @idmetodo); SELECT SCOPE_IDENTITY();");

                datos.setearParametro("@fecha", fecha);
                datos.setearParametro("@total", Total);
                datos.setearParametro("@idmetodo", id_metodo);

                // 🚨 REEMPLAZO: En lugar de datos.EjecutarAccion(), usa EjecutarScalar()
                object resultado = datos.EjecutarScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    // Convertimos el resultado de la base de datos (generalmente un decimal) a int
                    return Convert.ToInt32(resultado);
                }

                return 0; // Retorna 0 si no se pudo obtener el ID
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // El 'finally' ya no es necesario aquí si EjecutarScalar lo maneja internamente.
        }


        public void Agregar(Ventas nueva)
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                // Asegúrate de que los nombres de las columnas coincidan con tu DB
                datos.SetearConsulta("INSERT INTO VENTAS (Fecha, Total, Id_metodo, IdCliente) VALUES (@fecha, @total, @idmetodo, @idcliente)");

                datos.setearParametro("@fecha", nueva.Fecha);
                datos.setearParametro("@total", nueva.Total);
                datos.setearParametro("@idmetodo", nueva.Id_metodo);

                // Si la venta tiene un cliente (es fiada), mandamos el ID, sino mandamos NULL
                if (nueva.IdCliente != null)
                    datos.setearParametro("@idcliente", nueva.IdCliente);
                else
                    datos.setearParametro("@idcliente", DBNull.Value);

                datos.EjecutarAccion();
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

        public decimal deuda()
        {

            AccesoDATOS datos = new AccesoDATOS();
            decimal deudaHistorica = 0;

            datos.SetearConsulta(@"
            SELECT 
            (ISNULL((SELECT SUM(Total) FROM Ventas V INNER JOIN metodos_de_pago M ON V.id_metodo = M.id_metodo WHERE M.metodo_pago = 'FIADO'), 0) - 
             ISNULL((SELECT SUM(Monto) FROM PAGOS_DEUDORES), 0)) as SaldoHistorico");

            datos.EjecutarLectura();
            if (datos.Lector.Read())
            {
                deudaHistorica = (decimal)datos.Lector["SaldoHistorico"];
            }

            return deudaHistorica;


        }

        public List<Ventas> FiltrarFecha(DateTime fecha)
        {
            List<Ventas> lista = new List<Ventas>();
            AccesoDATOS datos = new AccesoDATOS();
            decimal deudaHistorica = 0;

            try
            {
                // Usamos UNION ALL para juntar Ventas Reales + Cobros de Deudores
                string consulta = @"
            -- 1. Traer Ventas que NO sean FIADO
            SELECT V.Id, V.Fecha, V.Total, V.id_metodo, M.metodo_pago AS Metodopago 
            FROM Ventas V 
            INNER JOIN metodos_de_pago M ON V.id_metodo = M.id_metodo 
            WHERE CONVERT(date, V.Fecha) = @fecha 
            

            UNION ALL

            -- 2. Traer Cobros de Deudores (como si fueran una venta más de ingreso)
            SELECT P.Id, P.Fecha, P.Monto as Total, P.IdMetodoPago, M.metodo_pago AS Metodopago
            FROM PAGOS_DEUDORES P
            INNER JOIN metodos_de_pago M ON P.IdMetodoPago = M.id_metodo
            WHERE CONVERT(date, P.Fecha) = @fecha
            
            ORDER BY Fecha DESC;";

                datos.SetearConsulta(consulta);
                datos.setearParametro("@fecha", fecha.Date);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Ventas aux = new Ventas();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];
                    aux.Total = (decimal)datos.Lector["Total"];
                    aux.Id_metodo = (int)datos.Lector["id_metodo"];
                    aux.metododepago = new Metodo_de_pago();
                    aux.metododepago.MetodoPago = (string)datos.Lector["Metodopago"];

                    // Opcional: Podés marcar los cobros de deuda con un texto especial si querés
                    // if (datos.Lector["Metodopago"].ToString() == "Efectivo") { ... }

                    lista.Add(aux);
                }

                // Según tu reporte de cierre de caja, el saldo real es la diferencia histórica
               

                return lista;
            }


            catch (Exception ex) { throw ex; }
            finally { datos.CerrarConexion(); }
        }
        


        public void EliminarVenta(int Id)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                

                // Iniciamos la transacción (esto requiere que tu clase AccesoDATOS 
                // permita gestionar el objeto SqlTransaction o que uses un bloque TransactionScope)
                // Aquí te muestro la lógica lógica pura de SQL que es más robusta:

                datos.SetearConsulta("BEGIN TRANSACTION; " +
                                     "DELETE FROM Venta_Por_Producto WHERE idVenta = @id; " +
                                     "DELETE FROM VENTAS WHERE Id = @id; " +
                                     "COMMIT TRANSACTION;");

                datos.setearParametro("@id", Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                // Si algo falla, podrías intentar un ROLLBACK aquí si manejas el objeto Transaction
                throw new Exception("No se pudo eliminar la venta y sus detalles: " + ex.Message);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Ventas> ConsultaRango(DateTime horainicio, DateTime horafin, out decimal deudaHistorica)
        {
            List<Ventas> lista = new List<Ventas>();
            AccesoDATOS datos = new AccesoDATOS();
            deudaHistorica = 0;

            try
            {
                // Usamos UNION ALL para juntar Ventas Reales + Cobros de Deudores en el rango horario
                string consulta = @"
            -- 1. Traer Ventas del rango
            SELECT V.Id, V.Fecha, V.Total, V.id_metodo, M.metodo_pago AS Metodopago 
            FROM Ventas V 
            INNER JOIN metodos_de_pago M ON V.id_metodo = M.id_metodo 
            WHERE V.Fecha BETWEEN @HoraInicio AND @HoraFin

            UNION ALL

            -- 2. Traer Cobros de Deudores del rango
            SELECT P.Id, P.Fecha, P.Monto as Total, P.IdMetodoPago, M.metodo_pago AS Metodopago
            FROM PAGOS_DEUDORES P
            INNER JOIN metodos_de_pago M ON P.IdMetodoPago = M.id_metodo
            WHERE P.Fecha BETWEEN @HoraInicio AND @HoraFin
            
            ORDER BY Fecha DESC;";

                datos.SetearConsulta(consulta);
                datos.setearParametro("@HoraInicio", horainicio);
                datos.setearParametro("@HoraFin", horafin);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Ventas aux = new Ventas();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];
                    aux.Total = (decimal)datos.Lector["Total"];
                    aux.Id_metodo = (int)datos.Lector["id_metodo"];
                    aux.metododepago = new Metodo_de_pago { MetodoPago = (string)datos.Lector["Metodopago"] };
                    lista.Add(aux);
                }
                datos.Lector.Close();
                datos.CerrarConexion(); // <--- HAY QUE CERRARLA PARA PODER VOLVER A ABRIRLA ABAJO
                                        // -----------------------

                // 2. QUERY DE DEUDA HISTÓRICA
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
                }

                return lista;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.CerrarConexion(); }
        }
        public decimal TotalDia(DateTime Fecha)
        {
            AccesoDATOS datos = new AccesoDATOS();
            decimal total = 0; // 1. Inicializamos en 0 por defecto

            try
            {
                // Usamos parámetros para evitar SQL Injection
                datos.SetearConsulta("SELECT SUM(total) AS TotalRecaudado FROM ventas WHERE CAST(fecha AS DATE) = @FechaSeleccionada");
                datos.setearParametro("@FechaSeleccionada", Fecha.Date); // Usamos .Date para asegurar solo la fecha
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    // 2. Verificamos si el resultado del SUM no es nulo
                    if (datos.Lector["TotalRecaudado"] != DBNull.Value)
                    {
                        total = Convert.ToDecimal(datos.Lector["TotalRecaudado"]);
                    }
                }

                return total;
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear el error antes de lanzarlo
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public int RegistrarVenta(Ventas nueva, int idCliente)
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                // Agregamos SELECT SCOPE_IDENTITY() al final para recuperar el ID
                datos.SetearConsulta("INSERT INTO VENTAS (Fecha, Total, id_metodo, IdCliente) VALUES (@fecha, @total, @idmetodo, @idcliente); SELECT SCOPE_IDENTITY();");
                datos.setearParametro("@fecha", nueva.Fecha);
                datos.setearParametro("@total", nueva.Total);
                datos.setearParametro("@idmetodo", nueva.metododepago.id);
                datos.setearParametro("@idcliente", idCliente);

                // ejecutarScalar devuelve el primer valor de la primera fila (nuestro ID)
                object resultado = datos.EjecutarScalar();
                return Convert.ToInt32(resultado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal TotalPorRango(DateTime desde, DateTime hasta)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
               string consulta = "SELECT ISNULL(SUM(Total), 0) FROM ventas WHERE fecha >= @inicio AND fecha <= @fin";
               datos.SetearConsulta(consulta);
               datos.setearParametro("@inicio", desde);
               datos.setearParametro("@fin", hasta);

                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    return (decimal)datos.Lector[0];
                }

                return 0;


            }
            catch (Exception)
            {

                throw;
            }

            finally { datos.CerrarConexion();  }
        }

    }
}




        


       