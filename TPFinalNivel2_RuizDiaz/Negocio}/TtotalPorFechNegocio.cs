using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocios;
using Dominio;

namespace Negocio_
{
    public class TtotalPorFechNegocio
    {
        public List<TotalPorFecha> listarFecha()
        {
            List<TotalPorFecha> lista = new List<TotalPorFecha>();

            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta("select Fecha, Total from GananciasDiarias");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TotalPorFecha aux = new TotalPorFecha();
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];
                    aux.Total = (decimal)datos.Lector["Total"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception)
            {

                throw;
            }

            finally
            {
                datos.CerrarConexion();
            }

        }

        public void Agregar_Total_Diario(DateTime fecha, decimal total)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta(@"
            IF NOT EXISTS (
                SELECT 1 FROM GananciasDiarias 
                WHERE CONVERT(date, Fecha) = @fecha
            )
            BEGIN
                INSERT INTO GananciasDiarias (Fecha, Total) 
                VALUES (@fecha, @total)
            END
        ");
                datos.setearParametro("@fecha", fecha);
                datos.setearParametro("@total", total);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar la ganancia diaria: " + ex.Message);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public decimal CalcularTotalDelDia(DateTime fecha)
        {
            decimal total = 0;
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta("SELECT SUM(Total) FROM Ventas WHERE CONVERT(date, Fecha) = @fecha");
                datos.setearParametro("@fecha", fecha.Date);
                datos.EjecutarLectura();

                if (datos.Lector.Read() && !datos.Lector.IsDBNull(0))
                {
                    total = datos.Lector.GetDecimal(0);
                }
                return total;
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

       public List<TotalPorFecha> TotalPorFechas (DateTime fecha)
        {

            List<TotalPorFecha> lista = new List<TotalPorFecha>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                string consulta = "SELECT Fecha, Total FROM GananciasDiarias WHERE CONVERT(date, Fecha) = @fecha";

                datos.SetearConsulta(consulta);
                datos.setearParametro("@fecha", fecha.Date);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TotalPorFecha aux = new TotalPorFecha();
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];
                    aux.Total = (decimal)datos.Lector["Total"];

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


        public List<Venta_Por_Producto> CargarDetalleProducto (DateTime fecha)
        {
            AccesoDATOS datos = new AccesoDATOS();
            List<Venta_Por_Producto> lista = new List<Venta_Por_Producto>();
            try
            {
                datos.SetearConsulta("SELECT Producto, Cantidad FROM Venta_Por_Producto WHERE Fecha = @fecha");
                datos.setearParametro("@fecha", fecha);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Venta_Por_Producto aux = new Venta_Por_Producto();
                    aux.Producto = (string)datos.Lector["Producto"];
                    aux.Cantidad = (int)datos.Lector["Cantidad"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception)
            {

                throw;
            }

            finally
            {
                datos.CerrarConexion();
            }
        

            

        }

    }
}
