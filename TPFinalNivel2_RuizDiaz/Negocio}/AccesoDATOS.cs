using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Negocios;
using System.Data;



namespace Negocios
{
    class AccesoDATOS
    {

        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDATOS()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS01; database=CATALOGO_DB_RECUPERADA; integrated security=true");
            comando = new SqlCommand();
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = "select Codigo, Nombre, A.Descripcion Descripcion, ImagenUrl, Precio, C.Descripcion Tipo, M.Descripcion Marca , Stock from ARTICULOS A, CATEGORIAS C, MARCAS M where IdMarca = M.Id and IdCategoria = C.Id";
            comando.Connection = conexion;
        }

        public void LimpiarParametros()
        {
            // comando es el objeto SqlCommand que declaraste arriba en tu clase
            if (comando != null)
            {
                comando.Parameters.Clear();
            }
        }

        public object EjecutarScalar()
        {
            // Usamos 'comando' que ya debe estar definido y tener la consulta y parámetros cargados.

            try
            {
                // 1. Abrir la conexión (Asumiendo que tienes un objeto SqlConnection 'conexion' en la clase)
                comando.Connection.Open();

                // 2. Ejecutar la consulta. ExecuteScalar() devuelve la primera columna de la primera fila.
                object resultado = comando.ExecuteScalar();

                // El resultado contendrá el ID devuelto por SCOPE_IDENTITY()
                return resultado;
            }
            catch (Exception ex)
            {
                // Si hay un error, lanzamos la excepción para que sea manejada arriba.
                throw ex;
            }
            finally
            {
                // 3. ¡MUY IMPORTANTE! Cerrar la conexión siempre, incluso si hubo un error.
                comando.Connection.Close();
            }
        }

        public void SetearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void SetearConsultaClear(string consulta)
        {
            comando.Parameters.Clear();
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }


        public void EjecutarLectura()
        {
            comando.Connection = conexion;

            try
            {

                conexion.Open();
                lector = comando.ExecuteReader();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // --- En AccesoDATOS.cs ---

        public DataTable EjecutarLecturanuevo() // CAMBIAR DE 'void' A 'DataTable'
        {
            DataTable tabla = new DataTable();
            try
            {
                comando.Connection.Open();

                // Usamos SqlDataAdapter para llenar el DataTable directamente
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                adaptador.Fill(tabla); // Llenar el DataTable con el resultado

                return tabla; // Devolver el DataTable lleno
            }
            catch (Exception ex)
            {
                // Manejar errores...
                throw new Exception("Error al ejecutar lectura: " + ex.Message, ex);
            }
            finally
            {
                // Asegurar el cierre de la conexión
                comando.Connection.Close();
            }
        }

        public void EjecutarAccionNUEVO()
        {
            comando.Connection = conexion;

            try
            {
                
                if (conexion.State == System.Data.ConnectionState.Closed)
                {
                    conexion.Open();
                }

                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void EjecutarAccion()
        {
            comando.Connection = conexion;


            try
            {

                conexion.Open();
                comando.ExecuteNonQuery();


            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }


        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);

        }

        public void CerrarConexion()
        {
            if (lector != null)
                lector.Close();
            conexion.Close();


        }


    }
}
