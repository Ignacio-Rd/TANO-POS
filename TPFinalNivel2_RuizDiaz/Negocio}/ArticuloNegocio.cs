using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;
using Negocios;

namespace Negocios
{
    public class ArticuloNegocio
    {

        public List<Articulos> listar()
        {
            List<Articulos> lista = new List<Articulos>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;


            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB_RECUPERADA; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select Codigo, Nombre, A.Descripcion Descripcion, ImagenUrl, Precio, PrecioCosto, " +
                    "C.Descripcion Tipo, M.Descripcion Marca, A.IdMarca, A.IdCategoria, A.Id IdArticulo, Stock from ARTICULOS A, CATEGORIAS C, MARCAS M where IdMarca = M.Id and IdCategoria = C.Id ORDER BY A.Nombre ASC";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {


                    Articulos aux = new Articulos();
                    aux.Id = (int)lector["IdArticulo"];
                    aux.codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];


                    if (!(lector["ImagenUrl"] is DBNull))
                    {
                        aux.URL = (string)lector["ImagenUrl"];
                    }

                    aux.Precio = Math.Round(lector.GetDecimal(4), 2, MidpointRounding.AwayFromZero);
                    aux.PrecioCosto = Math.Round(lector.GetDecimal(5), 2, MidpointRounding.AwayFromZero);
                    aux.Categoria = new categoria();
                    aux.Categoria.Descripcion = (string)lector["Tipo"];
                    aux.Categoria.Id = (int)lector["IdCategoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)lector["Marca"];
                    aux.Marca.Id = (int)lector["IdMarca"];
                    aux.Stock = (int)lector["Stock"];


                    lista.Add(aux);


                }

                conexion.Close();

                return lista;




            }
            catch (Exception ex)
            {

                throw ex;
            }




        }

        public decimal ObtenerPrecioPorId(int id)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                // 1. Corregir el parámetro y la consulta si 'id' es el código.
                // Si 'id' en el parámetro se refiere al campo 'codigo' de la tabla:
                datos.SetearConsulta("SELECT Precio FROM ARTICULOS WHERE Id = @ID");
                datos.setearParametro("@id", id);

                // 2. 🚨 USAR EjecutarScalar() 🚨
                // Ejecuta la consulta y devuelve el primer valor de la primera fila (el Precio).
                object resultado = datos.EjecutarScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    // Convertir el resultado (que es un decimal de SQL) a decimal de C#
                    // Puedes usar Convert.ToDecimal o un casting seguro.
                    return Convert.ToDecimal(resultado);
                }

                // Si no encuentra el precio (el artículo no existe), devuelve 0.
                return 0;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                throw new Exception("Error al obtener el precio del artículo.", ex);
            }
            finally
            {
                // Asegurarse de cerrar la conexión
                // Esto puede ser opcional si EjecutarScalar() ya cierra la conexión internamente.
                // datos.CerrarConexion(); 
            }
        }

        public decimal ObtenerPrecioVentaPorCodigo( string id)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                // 1. Corregir el parámetro y la consulta si 'id' es el código.
                // Si 'id' en el parámetro se refiere al campo 'codigo' de la tabla:
                datos.SetearConsulta("SELECT Precio FROM ARTICULOS WHERE Codigo = @ID");
                datos.setearParametro("@id", id);

                // 2. 🚨 USAR EjecutarScalar() 🚨
                // Ejecuta la consulta y devuelve el primer valor de la primera fila (el Precio).
                object resultado = datos.EjecutarScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    // Convertir el resultado (que es un decimal de SQL) a decimal de C#
                    // Puedes usar Convert.ToDecimal o un casting seguro.
                    return Convert.ToDecimal(resultado);
                }

                // Si no encuentra el precio (el artículo no existe), devuelve 0.
                return 0;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                throw new Exception("Error al obtener el precio del artículo.", ex);
            }
            finally
            {
                // Asegurarse de cerrar la conexión
                // Esto puede ser opcional si EjecutarScalar() ya cierra la conexión internamente.
                // datos.CerrarConexion(); 
            }
        }

        public decimal ObtenerPrecioCostoPorId(string codigo)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                // 1. Corregir el parámetro y la consulta si 'id' es el código.
                // Si 'id' en el parámetro se refiere al campo 'codigo' de la tabla:
                datos.SetearConsulta("SELECT PrecioCosto FROM ARTICULOS WHERE Codigo = @codigo");
                datos.setearParametro("@codigo", codigo);

                // 2. 🚨 USAR EjecutarScalar() 🚨
                // Ejecuta la consulta y devuelve el primer valor de la primera fila (el Precio).
                object resultado = datos.EjecutarScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    // Convertir el resultado (que es un decimal de SQL) a decimal de C#
                    // Puedes usar Convert.ToDecimal o un casting seguro.
                    return Convert.ToDecimal(resultado);
                }

                // Si no encuentra el precio (el artículo no existe), devuelve 0.
                return 0;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                throw new Exception("Error al obtener el precio del artículo.", ex);
            }
            finally
            {
                // Asegurarse de cerrar la conexión
                // Esto puede ser opcional si EjecutarScalar() ya cierra la conexión internamente.
                // datos.CerrarConexion(); 
            }
        }

        public string ObtenerNombrePorId(string codigo)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                // 1. Corregir el parámetro y la consulta si 'id' es el código.
                // Si 'id' en el parámetro se refiere al campo 'codigo' de la tabla:
                datos.SetearConsulta("SELECT Nombre FROM ARTICULOS WHERE Codigo = @codigo");
                datos.setearParametro("@codigo", codigo);

                // 2. 🚨 USAR EjecutarScalar() 🚨
                // Ejecuta la consulta y devuelve el primer valor de la primera fila (el Precio).
                object resultado = datos.EjecutarScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    // Convertir el resultado (que es un decimal de SQL) a decimal de C#
                    // Puedes usar Convert.ToDecimal o un casting seguro.
                    return Convert.ToString(resultado);
                }
                else
                {
                    return resultado != null ? resultado.ToString() : "No encontrado";
                }
                // Si no encuentra el precio (el artículo no existe), devuelve 0.
                
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                throw new Exception("Error al obtener el precio del artículo.", ex);
            }
            finally
            {
                datos.CerrarConexion(); 
            }
        }

        public void Agregar(Articulos nuevo)
        {

            AccesoDATOS datos = new AccesoDATOS();

            try
            {

                datos.SetearConsulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, PrecioCosto) values ('" + nuevo.codigo + "','" + nuevo.Nombre + "','" + nuevo.Descripcion + "', @idMarca, @idCategoria, @Precio, @stock, @Preciocosto )");

                datos.setearParametro("@idMarca", nuevo.Marca.Id);
                datos.setearParametro("@idCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@Precio", nuevo.Precio);
                datos.setearParametro("@stock", nuevo.Stock);
                datos.setearParametro("Preciocosto", nuevo.PrecioCosto);

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


        public void Modificar(Articulos modificar)
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                // Validación preventiva
                if (modificar.Marca == null || modificar.Categoria == null)
                    throw new Exception("La Marca o Categoría no son válidas.");

                datos.SetearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idmarca, IdCategoria = @idcategoria, Precio = @precio, Stock = @stock, PrecioCosto = @Preciocosto where id = @id");

                // ... (tus setearParametro actuales están bien) ...
                datos.setearParametro("@codigo", modificar.codigo);
                datos.setearParametro("@nombre", modificar.Nombre);
                datos.setearParametro("@descripcion", modificar.Descripcion);
                datos.setearParametro("@idmarca", modificar.Marca.Id);
                datos.setearParametro("@idcategoria", modificar.Categoria.Id);
                datos.setearParametro("@precio", modificar.Precio);
                datos.setearParametro("@id", modificar.Id);
                datos.setearParametro("@stock", modificar.Stock);
                datos.setearParametro("@Preciocosto", modificar.PrecioCosto);

                datos.EjecutarAccion();
            }
            catch (Exception ex) { throw ex; }
        }

        public void ModificarPrecios(string codigo, decimal precioCosto, decimal precioVenta)
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                datos.SetearConsulta("UPDATE ARTICULOS SET PrecioCosto = @precioCosto, Precio = @precioVenta WHERE Codigo = @codigo");
                datos.setearParametro("@precioCosto", precioCosto);
                datos.setearParametro("@precioVenta", precioVenta);
                datos.setearParametro("@codigo", codigo);
                datos.EjecutarAccion();
            }
            catch (Exception ex) { throw ex; }
        }



        public List<Articulos> filtrar(string campo, string criterio, string filtro)
        {

            //"Código","Precio","Nombre","Marca","Cateogoria")

            List<Articulos> lista = new List<Articulos>();
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                string consulta = "select Codigo, Nombre, A.Descripcion Descripcion, ImagenUrl, " +
                    "Precio, PrecioCosto, C.Descripcion Tipo, M.Descripcion Marca, A.IdMarca, A.IdCategoria, A.Id, Stock from ARTICULOS A, CATEGORIAS C, MARCAS M where IdMarca = M.Id and IdCategoria = C.Id and ";

                if (campo == "Precio ")
                {

                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += " Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += " Precio < " + filtro;
                            break;
                        default:
                            consulta += " Precio = " + filtro;
                            break;
                    }
                }
                else if (campo == "Código")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Codigo like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "Codigo like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Codigo like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Marca")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "M.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "M.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "C.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }

                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {


                    Articulos aux = new Articulos();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];


                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        aux.URL = (string)datos.Lector["ImagenUrl"];
                    }

                    aux.Precio = datos.Lector.GetDecimal(4);
                    aux.PrecioCosto = datos.Lector.GetDecimal(5);
                    aux.Categoria = new categoria();
                    aux.Categoria.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Stock = (int)datos.Lector["Stock"];


                    lista.Add(aux);


                }


                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Eliminar(int Id)
        {
            try
            {

                AccesoDATOS datos = new AccesoDATOS();
                datos.SetearConsulta("DELETE FROM ARTICULOS WHERE Id = @id");
                datos.setearParametro("@id", Id);

                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public List<Articulos> Obtener_Por_Codigo(string codigo, bool esVenta)
        {
            List<Articulos> lista = new List<Articulos>();
            AccesoDATOS datos = new AccesoDATOS();
            Articulos articulos = new Articulos();

            try
            {



                datos.SetearConsulta("select Codigo, Nombre, A.Descripcion Descripcion, ImagenUrl, Precio, PrecioCosto, C.Descripcion Tipo, M.Descripcion Marca, A.IdMarca, A.IdCategoria, A.Id, Stock from ARTICULOS A, CATEGORIAS C, MARCAS M where IdMarca = M.Id and IdCategoria = C.Id and A.Codigo = @codigo");
                datos.setearParametro("@codigo", codigo);

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {


                    Articulos aux = new Articulos();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];




                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        aux.URL = (string)datos.Lector["ImagenUrl"];
                    }
                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.PrecioCosto = (decimal)datos.Lector["PrecioCosto"];
                    aux.Stock = (int)datos.Lector["Stock"];


                    lista.Add(aux);


                }

              //  if (esVenta)
                //{
                  //  Descontar_Stock(codigo);
                //}

                return lista;

            }

            catch (Exception EX)
            {

                throw EX;
            }

            finally { datos.CerrarConexion(); }



        }

        public void Descontar_Stock(string producto, int cantidad) // Agregamos parámetro cantidad
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                // Restamos la cantidad exacta que se lleva el cliente
                datos.SetearConsulta("UPDATE ARTICULOS SET Stock = Stock - @cantidad WHERE Codigo = @codigo AND Stock >= @cantidad");
                datos.setearParametro("@codigo", producto);
                datos.setearParametro("@cantidad", cantidad);
                datos.EjecutarAccion();
            }
            catch (Exception ex) { throw ex; }
            finally { datos.CerrarConexion(); }
        }

        public void Reponer_Stock(string producto)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                // Suma 1 al stock del producto cuyo código coincide
                datos.SetearConsulta("UPDATE ARTICULOS SET Stock = Stock + 1 WHERE Codigo = @codigo");
                datos.setearParametro("@codigo", producto);
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

        public bool Obtener_Por_Codigo_Altas(string codigo) {
            try
            {
                List<Articulos> lista = new List<Articulos>();
                AccesoDATOS datos = new AccesoDATOS();
                Articulos articulos = new Articulos();


                datos.SetearConsulta("select Codigo, Nombre, A.Descripcion Descripcion, ImagenUrl, Precio, PrecioCosto, C.Descripcion Tipo, M.Descripcion Marca, A.IdMarca, A.IdCategoria, A.Id, Stock from ARTICULOS A, CATEGORIAS C, MARCAS M where IdMarca = M.Id and IdCategoria = C.Id and A.Codigo = @codigo");
                datos.setearParametro("@codigo", codigo.Trim());
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {


                    Articulos aux = new Articulos();
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];




                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        aux.URL = (string)datos.Lector["ImagenUrl"];
                    }

                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.PrecioCosto = (decimal)datos.Lector["PrecioCosto"];
                    aux.Stock = (int)datos.Lector["Stock"];


                    lista.Add(aux);


                }



                if (lista.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

            catch (Exception EX)
            {

                throw EX;
            }

        }

        public List<Articulos> ListarArticulo()
        {
            AccesoDATOS datos = new AccesoDATOS();

            List<Articulos> lista = new List<Articulos>();

            datos.SetearConsulta("select Nombre from ARTICULOS;");
            datos.EjecutarLectura();

            while (datos.Lector.Read())
            {
                Articulos aux = new Articulos();
                aux.Nombre = (string)datos.Lector["Nombre"];
                lista.Add(aux);
            }

            return lista;
        }

        public List<Articulos> buscarmarca(string Marca)
        {
            List<Articulos> lista = new List<Articulos>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                // 1. Agregamos los filtros de relación (A.IdCategoria = C.Id y A.IdMarca = M.Id)
                // 2. Dejamos A.Id sin alias para que coincida con tu mapeo abajo
                string consulta = "select Codigo, Nombre, A.Descripcion Descripcion, Precio, PrecioCosto, C.Descripcion Tipo, M.Descripcion Marca, A.Id, Stock " +
                                  "from ARTICULOS A, CATEGORIAS C, MARCAS M " +
                                  "where A.IdCategoria = C.Id AND A.IdMarca = M.Id AND M.Descripcion = @marca ORDER BY Nombre ASC";

                datos.SetearConsulta(consulta);
                datos.setearParametro("@marca", Marca);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    // Ahora "Id" sí existe en el SELECT
                    aux.Id = (int)datos.Lector["Id"];
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new categoria();
                    aux.Categoria.Descripcion = (string)datos.Lector["Tipo"];
                    

                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.PrecioCosto = (decimal)datos.Lector["PrecioCosto"];
                    aux.Stock = (int)datos.Lector["Stock"];


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

        public List<Articulos> buscarrubro(string cateogoria)
        {
            List<Articulos> lista = new List<Articulos>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                string consulta = "select Codigo, Nombre, A.Descripcion Descripcion, Precio, PrecioCosto, C.Descripcion Tipo, M.Descripcion Marca, C.Descripcion Categoria, A.Id, Stock " +
                  "from ARTICULOS A, CATEGORIAS C, MARCAS M " +
                  "where A.IdCategoria = C.Id AND A.IdMarca = M.Id AND C.Descripcion = @categoria ORDER BY Nombre ASC";

                datos.SetearConsulta(consulta);
                datos.setearParametro("categoria", cateogoria);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    // Ahora "Id" sí existe en el SELECT
                    aux.Id = (int)datos.Lector["Id"];
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    aux.Marca = new Marca();
                    aux.Categoria = new categoria();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.PrecioCosto = (decimal)datos.Lector["PrecioCosto"];
                    aux.Stock = (int)datos.Lector["Stock"];


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

        public void SumarSoloCantidadStock(int cantidad, string codigo)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta("update articulos set stock = stock + @cantidad where codigo = @codigo");
                datos.setearParametro("@cantidad", cantidad);
                datos.setearParametro("@codigo", codigo);

                datos.EjecutarAccion();
            }
            catch (Exception)
            {

                throw;
            }

            finally { datos.CerrarConexion(); }
        }

   public void SumarStock (int Cantidad, string codigo, decimal PrecioCosto, decimal PrecioVenta)
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                datos.SetearConsulta("update articulos set stock = stock + @cantidad, PrecioCosto = @preciocosto, Precio = @precioventa where codigo = @codigo");
                datos.setearParametro("@cantidad", Cantidad);
                datos.setearParametro("@preciocosto", PrecioCosto);
                datos.setearParametro("@precioventa", PrecioVenta);
                datos.setearParametro("@codigo", codigo);

                datos.EjecutarAccion();
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

        public void AumentarPorCodigo(string codigo, decimal porcentajeOCifra, string tipoAjuste)
        {
            string query = "";

            try
            {
                AccesoDATOS datos = new AccesoDATOS();

                // La lógica de actualización de precios es la misma
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

                // El cambio clave: filtramos por la columna Codigo
                query += " WHERE Codigo = @codigo";

                datos.SetearConsulta(query);
                datos.setearParametro("@codigo", codigo);
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

    

