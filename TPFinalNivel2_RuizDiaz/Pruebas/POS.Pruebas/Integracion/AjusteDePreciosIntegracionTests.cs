using System;
using Negocios;
using NUnit.Framework;

namespace POS.Pruebas.Integracion
{
    // Recálculo de precios de venta / costo por porcentaje o por cifra fija (lo que usa "Cambiar Precio Por Marca").
    [TestFixture]
    [Category("Integracion")]
    public class AjusteDePreciosIntegracionTests : BaseIntegracion
    {
        private string _codigo;

        [SetUp]
        public void PrepararArticulo()
        {
            _codigo = CodigoUnico();
            CrearArticulo(_codigo, "Ajuste " + _codigo, precio: 100m, costo: 60m, stock: 1);
        }

        [TestCase("PORC_VENTA", 10, 110, 60)]
        [TestCase("PORC_COSTO", 10, 100, 66)]
        [TestCase("PORC_COSTOVENTA", 10, 110, 66)]
        [TestCase("CIFRA_VENTA", 15, 115, 60)]
        [TestCase("CIFRA_COSTO", 5, 100, 65)]
        [TestCase("CIFRA_COSTOVENTA", 10, 110, 70)]
        public void AumentarPorCodigo_AjustaSoloLoQueIndicaElTipo(string tipo, int valor, int precioEsperado, int costoEsperado)
        {
            new ArticuloNegocio().AumentarPorCodigo(_codigo, valor, tipo);

            Assert.That(PrecioDe(_codigo), Is.EqualTo((decimal)precioEsperado));
            Assert.That(CostoDe(_codigo), Is.EqualTo((decimal)costoEsperado));
        }

        [Test]
        public void AumentarPorCodigo_NoTocaOtrosArticulos()
        {
            string otro = CodigoUnico();
            CrearArticulo(otro, "Otro", 100m, 60m, 1);

            new ArticuloNegocio().AumentarPorCodigo(_codigo, 50m, "PORC_COSTOVENTA");

            Assert.That(PrecioDe(otro), Is.EqualTo(100m));
            Assert.That(CostoDe(otro), Is.EqualTo(60m));
        }

        [Test]
        public void AumentarPorCodigo_ElMargenSobreElCostoSeMantieneConAjusteDeCostoYVentaJuntos()
        {
            // Si costo y venta suben el mismo %, el margen porcentual sobre el costo no cambia.
            decimal margenAntes = (PrecioDe(_codigo) - CostoDe(_codigo)) / CostoDe(_codigo);

            new ArticuloNegocio().AumentarPorCodigo(_codigo, 20m, "PORC_COSTOVENTA");

            decimal margenDespues = (PrecioDe(_codigo) - CostoDe(_codigo)) / CostoDe(_codigo);
            Assert.That(margenDespues, Is.EqualTo(margenAntes).Within(0.0001m));
        }

        [Test]
        public void AumentarPorCodigo_ConTipoDesconocido_LanzaArgumentExceptionYNoCambiaPrecios()
        {
            // Antes armaba un UPDATE inválido y fallaba recién en SQL Server; ahora se rechaza antes de tocar la base.
            Assert.Throws<ArgumentException>(() => new ArticuloNegocio().AumentarPorCodigo(_codigo, 10m, "TIPO_INEXISTENTE"));

            Assert.That(PrecioDe(_codigo), Is.EqualTo(100m));
            Assert.That(CostoDe(_codigo), Is.EqualTo(60m));
        }

        [Test]
        public void AumentarPorMarcaYRubro_ConTipoDesconocido_LanzanArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new MarcasNegocio().AumentarPorMarca(1, 10m, "TIPO_INEXISTENTE"));
            Assert.Throws<ArgumentException>(() => new CategoriaNegocio().AumentarPorRubro(1, 10m, "TIPO_INEXISTENTE"));
        }

        [Test]
        public void AumentarPorMarca_AjustaSoloLosArticulosDeEsaMarca()
        {
            int marca = Insertar("INSERT INTO MARCAS (Descripcion) VALUES ('Marca de prueba')");
            int otraMarca = Insertar("INSERT INTO MARCAS (Descripcion) VALUES ('Otra marca de prueba')");
            string codigoDeLaMarca = CodigoUnico(), codigoDeOtra = CodigoUnico();
            CrearArticulo(codigoDeLaMarca, "De la marca", 200m, 100m, 1, idMarca: marca);
            CrearArticulo(codigoDeOtra, "De otra marca", 200m, 100m, 1, idMarca: otraMarca);

            new MarcasNegocio().AumentarPorMarca(marca, 10m, "PORC_COSTOVENTA");

            Assert.That(PrecioDe(codigoDeLaMarca), Is.EqualTo(220m));
            Assert.That(CostoDe(codigoDeLaMarca), Is.EqualTo(110m));
            Assert.That(PrecioDe(codigoDeOtra), Is.EqualTo(200m));
            Assert.That(CostoDe(codigoDeOtra), Is.EqualTo(100m));
        }

        [Test]
        public void AumentarPorRubro_AjustaSoloLosArticulosDeEsaCategoria()
        {
            int rubro = Insertar("INSERT INTO CATEGORIAS (Descripcion) VALUES ('Rubro de prueba')");
            int otroRubro = Insertar("INSERT INTO CATEGORIAS (Descripcion) VALUES ('Otro rubro de prueba')");
            string codigoDelRubro = CodigoUnico(), codigoDeOtro = CodigoUnico();
            CrearArticulo(codigoDelRubro, "Del rubro", 200m, 100m, 1, idCategoria: rubro);
            CrearArticulo(codigoDeOtro, "De otro rubro", 200m, 100m, 1, idCategoria: otroRubro);

            new CategoriaNegocio().AumentarPorRubro(rubro, 25m, "PORC_VENTA");

            Assert.That(PrecioDe(codigoDelRubro), Is.EqualTo(250m));
            Assert.That(PrecioDe(codigoDeOtro), Is.EqualTo(200m));
        }
    }
}
