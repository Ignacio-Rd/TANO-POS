using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Negocios;

namespace Negocios
{
   public class CategoriaNegocio
    {
        public List<categoria> listarCategoria()
        {
            List<categoria> lista = new List<categoria>();

            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta("SELECT id, descripcion FROM CATEGORIAS ORDER BY descripcion ASC;");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    categoria aux = new categoria();
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

        public void agregarCategoria (categoria nuevo)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {

                datos.SetearConsulta("insert into CATEGORIAS(Descripcion)values(@categoria)");

                datos.setearParametro("categoria", nuevo.Descripcion);

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

        public void eliminarCategoria(int id)
        {
            try
            {

                AccesoDATOS datos = new AccesoDATOS();
                datos.SetearConsulta("DELETE FROM CATEGORIAS WHERE Id = @id");
                datos.setearParametro("@id", id);

                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {



                throw ex;
            }
        }


        public void AumentarPorRubro(int IdCategoria, decimal porcentajeOCifra, string tipoAjuste)
        {

            string query = "";

            try
            {
                AccesoDATOS datos = new AccesoDATOS();

                switch (tipoAjuste)
                {
                    case "PORC_COSTOVENTA":
                        query = "UPDATE Articulos SET PrecioCosto *= (1 + @val/100), Precio *= (1 + @val/100)";
                        break;
                    case "CIFRA_COSTOVENTA":
                        query = "UPDATE Articulos SET PrecioCosto += @val, Precio += @val";
                        break;
                    case "PORC_VENTA":
                        query = "UPDATE Articulos SET Precio *= (1 + @val/100)";
                        break;
                    case "CIFRA_VENTA":
                        query = "UPDATE Articulos SET Precio += @val";
                        break;
                    case "PORC_COSTO":
                        query = "UPDATE Articulos SET PrecioCosto *= (1 + @val/100)";
                        break;
                    case "CIFRA_COSTO":
                        query = "UPDATE Articulos SET PrecioCosto += @val";
                        break;
                }

                query += " WHERE IdCategoria = @IdCategoria";

                datos.SetearConsulta(query);
                datos.setearParametro("@IdCategoria", IdCategoria);
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
