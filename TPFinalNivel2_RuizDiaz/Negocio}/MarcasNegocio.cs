using System;
using Negocio_.Calculos;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Negocios;

namespace Negocios
{
     public class MarcasNegocio
    {
        public List<Marca> listarmarca()
        {
            List<Marca> lista = new List<Marca>();

            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta("select id, descripcion from MARCAS ORDER BY descripcion ASC;");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)datos.Lector["id"];
                    aux.Descripcion = (string)datos.Lector["descripcion"];

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

        public void agregarMarca (Marca nuevo)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {

                datos.SetearConsulta("insert into MARCAS(Descripcion)values(@marca)");

                datos.setearParametro("marca", nuevo.Descripcion);

                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public void eliminarMarca(int id)
        {
            try
            {

                AccesoDATOS datos = new AccesoDATOS();
                datos.SetearConsulta("DELETE FROM MARCAS WHERE Id = @id");
                datos.setearParametro("@id", id);

                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {



                throw ex;
            }
        }

        public void AumentarPorMarca(int Idmarca, decimal porcentajeOCifra, string tipoAjuste)
        {
            string query = "";

            try
            {
                AccesoDATOS datos = new AccesoDATOS();

                query = AjusteDePrecios.SentenciaUpdate(tipoAjuste);

                query += " WHERE IdMarca = @idMarca";

                datos.SetearConsulta(query);
                datos.setearParametro("@idMarca", Idmarca);
                datos.setearParametro("@val", porcentajeOCifra);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

      


    }
}

