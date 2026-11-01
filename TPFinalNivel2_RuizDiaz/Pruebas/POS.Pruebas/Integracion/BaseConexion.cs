using System;
using System.Configuration;
using System.Data.SqlClient;
using NUnit.Framework;

namespace POS.Pruebas.Integracion
{
    // Acceso directo a SQL Server para preparar datos y verificar resultados.
    // Cada método abre y cierra su propia conexión: así, dentro de un TransactionScope nunca hay
    // dos conexiones abiertas a la vez de nuestra parte (evita escalar a transacción distribuida).
    [Category("Integracion")]
    public abstract class BaseConexion
    {
        protected static string Cadena
        {
            get { return ConfigurationManager.ConnectionStrings["CatalogoDB"].ConnectionString; }
        }

        [OneTimeSetUp]
        public void VerificarQueExisteLaBaseDePruebas()
        {
            try
            {
                Escalar<int>("SELECT COUNT(*) FROM metodos_de_pago");
            }
            catch (Exception ex)
            {
                string mensaje = "No se pudo usar la base de pruebas (" + Cadena + "): " + ex.Message;

                // En CI (POS_REQUIRE_DB=1) la falta de base es un FALLO; en tu PC, las pruebas de integración se omiten.
                if (Environment.GetEnvironmentVariable("POS_REQUIRE_DB") == "1")
                    Assert.Fail(mensaje);

                Assert.Ignore(mensaje);
            }
        }

        // ---------- Helpers de SQL ----------

        protected static SqlParameter P(string nombre, object valor)
        {
            return new SqlParameter(nombre, valor ?? DBNull.Value);
        }

        protected static int Ejecutar(string sql, params SqlParameter[] parametros)
        {
            using (var conexion = new SqlConnection(Cadena))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddRange(parametros);
                conexion.Open();
                return comando.ExecuteNonQuery();
            }
        }

        protected static T Escalar<T>(string sql, params SqlParameter[] parametros)
        {
            using (var conexion = new SqlConnection(Cadena))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddRange(parametros);
                conexion.Open();
                object resultado = comando.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                    return default(T);

                return (T)Convert.ChangeType(resultado, typeof(T));
            }
        }

        // INSERT que devuelve el Id generado.
        protected static int Insertar(string sql, params SqlParameter[] parametros)
        {
            return Escalar<int>(sql + "; SELECT CAST(SCOPE_IDENTITY() AS int);", parametros);
        }

        // ---------- Datos de prueba ----------

        protected static string CodigoUnico()
        {
            return "TEST-" + Guid.NewGuid().ToString("N").Substring(0, 12);
        }

        protected static int IdMetodo(string nombre)
        {
            int id = Escalar<int>("SELECT id_metodo FROM metodos_de_pago WHERE metodo_pago = @n", P("@n", nombre));
            Assert.That(id, Is.GreaterThan(0), "No existe el método de pago '" + nombre + "' en la base de pruebas.");
            return id;
        }

        protected static decimal PorcentajeDe(string metodo)
        {
            return Escalar<decimal>("SELECT porcentaje FROM metodos_de_pago WHERE metodo_pago = @n", P("@n", metodo));
        }

        protected static int CrearArticulo(string codigo, string nombre, decimal precio, decimal costo, int stock,
                                           int? idMarca = null, int? idCategoria = null)
        {
            return Insertar(
                "INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio, PrecioCosto, Stock) " +
                "VALUES (@codigo, @nombre, 'articulo de prueba', @marca, @categoria, @precio, @costo, @stock)",
                P("@codigo", codigo), P("@nombre", nombre), P("@marca", idMarca), P("@categoria", idCategoria),
                P("@precio", precio), P("@costo", costo), P("@stock", stock));
        }

        protected static decimal PrecioDe(string codigo)
        {
            return Escalar<decimal>("SELECT Precio FROM ARTICULOS WHERE Codigo = @c", P("@c", codigo));
        }

        protected static decimal CostoDe(string codigo)
        {
            return Escalar<decimal>("SELECT PrecioCosto FROM ARTICULOS WHERE Codigo = @c", P("@c", codigo));
        }

        protected static int StockDe(string codigo)
        {
            return Escalar<int>("SELECT Stock FROM ARTICULOS WHERE Codigo = @c", P("@c", codigo));
        }

        protected static int CrearCliente()
        {
            return Insertar("INSERT INTO Clientes (Nombre, Estado) VALUES (@n, 1)", P("@n", "Cliente prueba " + Guid.NewGuid().ToString("N").Substring(0, 8)));
        }

        protected static int CrearProveedor()
        {
            return Insertar("INSERT INTO PROVEEDORES (Nombre, Estado) VALUES (@n, 1)", P("@n", "Proveedor prueba " + Guid.NewGuid().ToString("N").Substring(0, 8)));
        }

        protected static int IdCategoriaPagoVario()
        {
            int id = Escalar<int>("SELECT TOP 1 IdCategoria FROM CATEGORIAS_PAGOS_VARIOS ORDER BY IdCategoria");
            if (id == 0)
                id = Insertar("INSERT INTO CATEGORIAS_PAGOS_VARIOS (Nombre) VALUES ('Prueba')");
            return id;
        }
    }
}
