using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Negocios;


namespace Negocio_
{
    public class MetodoDePagoNegocio
    {

        public List<Metodo_de_pago> Listametodo()
        {
            List<Metodo_de_pago> lista = new List<Metodo_de_pago>();

            AccesoDATOS datos = new AccesoDATOS();

            try
            {

                datos.SetearConsulta("select id_metodo, metodo_pago, porcentaje from metodos_de_pago WHERE metodo_pago <> 'FIADO'");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Metodo_de_pago aux = new Metodo_de_pago();
                    aux.id = (int)datos.Lector["id_metodo"];
                    aux.MetodoPago = (string)datos.Lector["metodo_pago"];
                    aux.porcentaje = (decimal)datos.Lector["porcentaje"];
                   

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

        public void modificarMetodoPago(int id_metodo, string metodo_pago, decimal porcentaje)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                string query = "UPDATE metodos_de_pago SET metodo_pago = @metodo_pago, porcentaje = @porcentaje WHERE id_metodo = @id_metodo";

                datos.SetearConsulta(query);
                /*datos.setearParametro("@fondo_inicial", fondo_inicial);*/
                datos.setearParametro("@porcentaje", porcentaje);
                datos.setearParametro("@metodo_pago", metodo_pago);
                datos.setearParametro("@id_metodo", id_metodo);

                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }

           
        }

        public void agregarMetodopago(Metodo_de_pago nuevo)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta("INSERT INTO metodos_de_pago (metodo_pago, porcentaje) VALUES (@metodo_pago, @porcentaje, @fondo_inicial)");
                datos.setearParametro("@metodo_pago", nuevo.MetodoPago);
                datos.setearParametro("@porcentaje", nuevo.porcentaje);
                

                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void EliminarMetodoPago(int id)
        {
            try
            {
                AccesoDATOS datos = new AccesoDATOS();
                datos.SetearConsulta("delete from metodos_de_pago where id_metodo = @id");
                datos.setearParametro("@id", id);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public decimal ObtenerPorcentaje(string metodo)
        {
            AccesoDATOS datos = new AccesoDATOS();
            // 1. Inicializamos con un valor por defecto
            decimal porcentaje = 0;

            try
            {
                datos.SetearConsulta("select porcentaje from metodos_de_pago where metodo_pago = @metodo");
                datos.setearParametro("@metodo", metodo);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    // Verificamos que no sea nulo en la DB antes de convertir
                    if (!(datos.Lector["porcentaje"] is DBNull))
                        porcentaje = (decimal)datos.Lector["porcentaje"];
                }

                return porcentaje;
            }
            catch (Exception ex)
            {
                // Es buena práctica loguear el error o lanzarlo
                throw ex;
            }
            finally
            {
                // 2. IMPORTANTE: Siempre cerrar la conexión
                datos.CerrarConexion();
            }
        }


        public decimal Elegir_metodopago(decimal precio, string metodo)
        {
            decimal porcentaje = 0;


            if (!string.IsNullOrEmpty(metodo))
            {
                AccesoDATOS datos = new AccesoDATOS();

                try
                {
                    datos.SetearConsulta("select porcentaje from metodos_de_pago where metodo_pago = @metodo");
                    datos.setearParametro("@metodo", metodo);

                    datos.EjecutarLectura();


                    if (datos.Lector.Read())
                    {
                        porcentaje = (decimal)datos.Lector["porcentaje"];
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    datos.CerrarConexion();
                }

                precio += precio * (porcentaje / 100);

                return precio;


            }

            return precio;
        }



        public int IdDelMetodo(string metodo)
        {
            int metodoDePago;

            if (!string.IsNullOrEmpty(metodo))  //este condicional me dice que va a pasar la barrera del if si no es nulo o vacío
            {

                AccesoDATOS datos = new AccesoDATOS(); // inicio una instancia de mi acceso a datos

                try
                {

                    datos.SetearConsulta("select id_metodo from metodos_de_pago where metodo_pago = @metodo"); //Hago la consulta a mi base de datos con la funcion setear consulta
                    datos.setearParametro("@metodo", metodo); //seteo el parametro
                    datos.EjecutarLectura();

                    if (datos.Lector.Read())
                    {
                        metodoDePago = (int)datos.Lector["id_metodo"]; //agrego a mi variable metodo de pago el id del metodo de pago que me arroja mi consulta sql

                        return metodoDePago; //devuelvo el metodo de pago
                    }

                    else
                    {

                        return -1;
                    }



                }
                catch (Exception)
                {

                    return - 1;
                }
                finally
                {
                    datos.CerrarConexion();
                }



            }

            return -1;

        }


       /* public void SumarAlFondo(int idMetodo, decimal monto)
        {
            AccesoDATOS datos = new AccesoDATOS();
            datos.SetearConsulta("UPDATE metodos_de_pago SET fondo_inicial = fondo_inicial + @monto WHERE id_metodo = @idMetodo");
            datos.setearParametro("@monto", monto);
            datos.setearParametro("@idMetodo", idMetodo);
            datos.EjecutarAccion();
        }

        public void restarAlFondo(int idMetodo, decimal monto)
        {
            AccesoDATOS datos = new AccesoDATOS();
            datos.SetearConsulta("UPDATE metodos_de_pago SET fondo_inicial = fondo_inicial - @monto WHERE id_metodo = @idMetodo");
            datos.setearParametro("@monto", monto);
            datos.setearParametro("@idMetodo", idMetodo);
            datos.EjecutarAccion();
        }*/


    }
    }


