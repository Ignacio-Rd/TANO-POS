using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;
using Negocios;
using Negocio_;

namespace Negocio_
{
    public class ImpresionNeogcio
    {

        public string ObtenerImpresora()
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                datos.SetearConsulta("SELECT NombreImpresora FROM CONFIGURACION WHERE Id = 1");
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    return datos.Lector["NombreImpresora"].ToString();
                }

                return "Microsoft Print to PDF"; // Valor por defecto si no hay nada
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

        public void GuardarImpresora(string nombre)
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                datos.SetearConsulta("UPDATE CONFIGURACION SET NombreImpresora = @nombre WHERE Id = 1");
                datos.setearParametro("@nombre", nombre);
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
    }
}
