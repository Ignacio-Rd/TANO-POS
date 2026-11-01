using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Dominio;
using Negocios;
using NUnit.Framework;

namespace POS.Pruebas.Integracion
{
    // El texto que escribe el usuario (nombre, descripción, filtros) viaja como parámetro y nunca se pega en el SQL.
    [TestFixture]
    [Category("Integracion")]
    public class ArticuloSqlSeguroIntegracionTests : BaseIntegracion
    {
        private int _marca, _categoria;
        private string _descripcionMarca, _descripcionRubro, _token;

        [SetUp]
        public void PrepararMarcaYRubro()
        {
            _token = Guid.NewGuid().ToString("N").Substring(0, 8);
            _descripcionMarca = "Marca " + _token;
            _descripcionRubro = "Rubro " + _token;
            _marca = Insertar("INSERT INTO MARCAS (Descripcion) VALUES (@d)", P("@d", _descripcionMarca));
            _categoria = Insertar("INSERT INTO CATEGORIAS (Descripcion) VALUES (@d)", P("@d", _descripcionRubro));
        }

        private Articulos NuevoArticulo(string nombre, string descripcion = "Descripcion de prueba")
        {
            return new Articulos
            {
                codigo = CodigoUnico(),
                Nombre = nombre,
                Descripcion = descripcion,
                Marca = new Marca { Id = _marca },
                Categoria = new categoria { Id = _categoria },
                Precio = 150.50m,
                PrecioCosto = 90m,
                Stock = 7
            };
        }

        private string Crear(string nombre, decimal precio = 100m, string codigo = null)
        {
            codigo = codigo ?? CodigoUnico();
            CrearArticulo(codigo, nombre, precio, 10m, 1, _marca, _categoria);
            return nombre;
        }

        private static List<string> Nombres(List<Articulos> lista)
        {
            return lista.Select(a => a.Nombre).ToList();
        }

        // ---------- Agregar ----------

        [Test]
        public void Agregar_GuardaTodosLosCampos()
        {
            var articulo = NuevoArticulo("Producto completo " + _token, "Una descripcion cualquiera");

            new ArticuloNegocio().Agregar(articulo);

            Assert.That(Escalar<string>("SELECT Nombre FROM ARTICULOS WHERE Codigo = @c", P("@c", articulo.codigo)), Is.EqualTo(articulo.Nombre));
            Assert.That(Escalar<string>("SELECT Descripcion FROM ARTICULOS WHERE Codigo = @c", P("@c", articulo.codigo)), Is.EqualTo("Una descripcion cualquiera"));
            Assert.That(Escalar<int>("SELECT IdMarca FROM ARTICULOS WHERE Codigo = @c", P("@c", articulo.codigo)), Is.EqualTo(_marca));
            Assert.That(Escalar<int>("SELECT IdCategoria FROM ARTICULOS WHERE Codigo = @c", P("@c", articulo.codigo)), Is.EqualTo(_categoria));
            Assert.That(PrecioDe(articulo.codigo), Is.EqualTo(150.50m));
            Assert.That(CostoDe(articulo.codigo), Is.EqualTo(90m));
            Assert.That(StockDe(articulo.codigo), Is.EqualTo(7));
        }

        [Test]
        public void Agregar_ConApostrofeEnElNombre_GuardaElNombreCompleto()
        {
            // Antes un producto como "Galletitas D'Onofrio" rompía el INSERT.
            var articulo = NuevoArticulo("Galletitas D'Onofrio", "Sabor d'la casa");

            Assert.DoesNotThrow(() => new ArticuloNegocio().Agregar(articulo));

            Assert.That(Escalar<string>("SELECT Nombre FROM ARTICULOS WHERE Codigo = @c", P("@c", articulo.codigo)), Is.EqualTo("Galletitas D'Onofrio"));
            Assert.That(Escalar<string>("SELECT Descripcion FROM ARTICULOS WHERE Codigo = @c", P("@c", articulo.codigo)), Is.EqualTo("Sabor d'la casa"));
        }

        [Test]
        public void Agregar_ConSqlMalicioso_LoGuardaComoTextoYNoEjecutaNada()
        {
            Crear("Testigo " + _token);   // si se ejecutara el DELETE, este articulo desapareceria
            int articulosAntes = Escalar<int>("SELECT COUNT(*) FROM ARTICULOS");
            int rubrosAntes = Escalar<int>("SELECT COUNT(*) FROM CATEGORIAS");

            string nombre = "x'); DELETE FROM ARTICULOS; --";
            string descripcion = "'); DROP TABLE CATEGORIAS; --";
            var articulo = NuevoArticulo(nombre, descripcion);

            new ArticuloNegocio().Agregar(articulo);

            Assert.That(Escalar<int>("SELECT COUNT(*) FROM ARTICULOS"), Is.EqualTo(articulosAntes + 1));
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM CATEGORIAS"), Is.EqualTo(rubrosAntes));
            Assert.That(Escalar<string>("SELECT Nombre FROM ARTICULOS WHERE Codigo = @c", P("@c", articulo.codigo)), Is.EqualTo(nombre));
            Assert.That(Escalar<string>("SELECT Descripcion FROM ARTICULOS WHERE Codigo = @c", P("@c", articulo.codigo)), Is.EqualTo(descripcion));
        }

        [Test]
        public void Agregar_ConDescripcionNula_GuardaTextoVacioComoAntes()
        {
            var articulo = NuevoArticulo("Sin descripcion " + _token, null);

            new ArticuloNegocio().Agregar(articulo);

            Assert.That(Escalar<string>("SELECT Descripcion FROM ARTICULOS WHERE Codigo = @c", P("@c", articulo.codigo)), Is.EqualTo(""));
        }

        // ---------- filtrar: por texto ----------

        [Test]
        public void Filtrar_PorNombre_ComienzaConTerminaConYContiene()
        {
            string alfa = Crear("Alfa" + _token);
            string beta = Crear("Beta" + _token);
            var negocio = new ArticuloNegocio();

            Assert.That(Nombres(negocio.filtrar("Nombre", "Comienza con", "Alfa" + _token)), Is.EquivalentTo(new[] { alfa }));
            Assert.That(Nombres(negocio.filtrar("Nombre", "Termina con", "a" + _token)), Is.EquivalentTo(new[] { alfa, beta }));
            Assert.That(Nombres(negocio.filtrar("Nombre", "Contiene", "lfa" + _token)), Is.EquivalentTo(new[] { alfa }));
        }

        [Test]
        public void Filtrar_PorCodigo()
        {
            string codigo = "COD-" + _token;
            Crear("Con codigo " + _token, codigo: codigo);
            Crear("Otro " + _token);

            var resultado = new ArticuloNegocio().filtrar("Código", "Comienza con", codigo);

            Assert.That(resultado.Select(a => a.codigo), Is.EquivalentTo(new[] { codigo }));
        }

        [Test]
        public void Filtrar_PorMarcaYPorRubro()
        {
            string uno = Crear("Uno " + _token);
            string dos = Crear("Dos " + _token);
            var negocio = new ArticuloNegocio();

            Assert.That(Nombres(negocio.filtrar("Marca", "Contiene", _descripcionMarca)), Is.EquivalentTo(new[] { uno, dos }));
            Assert.That(Nombres(negocio.filtrar("Categoría", "Contiene", _descripcionRubro)), Is.EquivalentTo(new[] { uno, dos }));
            Assert.That(negocio.filtrar("Marca", "Contiene", "no existe " + _token), Is.Empty);
        }

        // ---------- filtrar: por precio ----------

        [Test]
        public void Filtrar_PorPrecio_MayorMenorEIgual()
        {
            string barato = Crear("Barato " + _token, 100m);
            string medio = Crear("Medio " + _token, 200m);
            string caro = Crear("Caro " + _token, 300m);
            var negocio = new ArticuloNegocio();

            var mayor = Nombres(negocio.filtrar("Precio ", "Mayor a", "150"));
            var menor = Nombres(negocio.filtrar("Precio ", "Menor a", "250"));
            var igual = Nombres(negocio.filtrar("Precio ", "Igual a", "200"));

            Assert.That(mayor, Does.Contain(medio).And.Contain(caro).And.Not.Contain(barato));
            Assert.That(menor, Does.Contain(barato).And.Contain(medio).And.Not.Contain(caro));
            Assert.That(igual, Does.Contain(medio).And.Not.Contain(barato).And.Not.Contain(caro));
        }

        [TestCase("200.5")]
        [TestCase("200,5")]
        public void Filtrar_PorPrecio_AceptaPuntoOComaDecimal(string texto)
        {
            var cultura = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");   // en Argentina la coma es el decimal
            try
            {
                string exacto = Crear("Exacto " + _token, 200.50m);
                Crear("Otro " + _token, 200m);

                var resultado = new ArticuloNegocio().filtrar("Precio ", "Igual a", texto);

                Assert.That(Nombres(resultado), Does.Contain(exacto));
                Assert.That(Nombres(resultado), Does.Not.Contain("Otro " + _token));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = cultura;
            }
        }

        [TestCase("abc")]
        [TestCase("1 OR 1=1")]
        [TestCase("0; DELETE FROM ARTICULOS")]
        [TestCase("")]
        public void Filtrar_PorPrecio_ConUnTextoQueNoEsNumero_LanzaArgumentExceptionSinTocarLaBase(string texto)
        {
            Crear("Testigo " + _token);
            int antes = Escalar<int>("SELECT COUNT(*) FROM ARTICULOS");

            Assert.Throws<ArgumentException>(() => new ArticuloNegocio().filtrar("Precio ", "Mayor a", texto));

            Assert.That(Escalar<int>("SELECT COUNT(*) FROM ARTICULOS"), Is.EqualTo(antes));
        }

        // ---------- filtrar: texto especial ----------

        [Test]
        public void Filtrar_ConApostrofe_FuncionaYEncuentraElArticulo()
        {
            string nombre = Crear("Galletitas D'Onofrio " + _token);

            var resultado = new ArticuloNegocio().filtrar("Nombre", "Contiene", "D'Onofrio " + _token);

            Assert.That(Nombres(resultado), Is.EquivalentTo(new[] { nombre }));
        }

        [TestCase("' OR 1=1 --")]
        [TestCase("'; DELETE FROM ARTICULOS; --")]
        [TestCase("%' OR Precio > 0 OR Nombre LIKE '")]
        public void Filtrar_ConSqlMalicioso_NoDevuelveTodoNiBorraNada(string filtro)
        {
            string testigo = Crear("Testigo " + _token);
            int antes = Escalar<int>("SELECT COUNT(*) FROM ARTICULOS");

            var resultado = new ArticuloNegocio().filtrar("Nombre", "Contiene", filtro);

            Assert.That(Nombres(resultado), Does.Not.Contain(testigo));
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM ARTICULOS"), Is.EqualTo(antes));
        }

        [Test]
        public void Filtrar_LosComodinesDeLikeSeBuscanComoTextoComun()
        {
            string conPorcentaje = Crear("Jugo 100% " + _token);
            string sinPorcentaje = Crear("Jugo 100 " + _token);
            string conGuion = Crear("a_b " + _token);
            string sinGuion = Crear("axb " + _token);
            var negocio = new ArticuloNegocio();

            // "%" solo debe encontrar el que tiene un "%" de verdad (no todos).
            var porcentaje = Nombres(negocio.filtrar("Nombre", "Contiene", "% " + _token));
            Assert.That(porcentaje, Does.Contain(conPorcentaje).And.Not.Contain(sinPorcentaje));

            // "_" solo debe encontrar el que tiene un "_" de verdad (no cualquier letra).
            var guion = Nombres(negocio.filtrar("Nombre", "Contiene", "a_b " + _token));
            Assert.That(guion, Does.Contain(conGuion).And.Not.Contain(sinGuion));
        }
    }
}
