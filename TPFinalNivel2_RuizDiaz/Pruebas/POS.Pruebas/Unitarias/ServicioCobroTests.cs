using System;
using System.Collections.Generic;
using Dominio;
using Moq;
using Negocio_;
using Negocio_.Calculos;
using NUnit.Framework;

namespace POS.Pruebas.Unitarias
{
    // Estas pruebas NO usan la base de datos: el porcentaje de cada método de pago lo entrega un mock.
    [TestFixture]
    [Category("Unitaria")]
    public class ServicioCobroTests
    {
        private Mock<IPorcentajeMetodoPago> _porcentajes;
        private ServicioCobro _servicio;

        [SetUp]
        public void Preparar()
        {
            _porcentajes = new Mock<IPorcentajeMetodoPago>();
            _porcentajes.Setup(p => p.ObtenerPorcentaje("Efectivo")).Returns(0m);
            _porcentajes.Setup(p => p.ObtenerPorcentaje("Débito")).Returns(5m);
            _porcentajes.Setup(p => p.ObtenerPorcentaje("Crédito")).Returns(10m);
            _porcentajes.Setup(p => p.ObtenerPorcentaje("Transferencia")).Returns(3m);

            _servicio = new ServicioCobro(_porcentajes.Object);
        }

        private static List<Articulos> Carrito(params decimal[] precios)
        {
            var lista = new List<Articulos>();
            foreach (decimal precio in precios)
                lista.Add(new Articulos { Precio = precio });
            return lista;
        }

        // ---------- AplicarRecargo ----------

        [TestCase("Efectivo", 1000, 1000)]
        [TestCase("Débito", 1000, 1050)]
        [TestCase("Crédito", 1000, 1100)]
        [TestCase("Transferencia", 1000, 1030)]
        public void AplicarRecargo_SegunElMetodoDePago(string metodo, int importe, int esperado)
        {
            Assert.That(_servicio.AplicarRecargo(importe, metodo), Is.EqualTo((decimal)esperado));
        }

        [Test]
        public void AplicarRecargo_ConsultaElPorcentajeUnaSolaVez()
        {
            _servicio.AplicarRecargo(500m, "Crédito");

            _porcentajes.Verify(p => p.ObtenerPorcentaje("Crédito"), Times.Once());
            _porcentajes.VerifyNoOtherCalls();
        }

        [TestCase(null)]
        [TestCase("")]
        public void AplicarRecargo_SinMetodo_DevuelveElImporteYNoConsultaNada(string metodo)
        {
            Assert.That(_servicio.AplicarRecargo(1234.56m, metodo), Is.EqualTo(1234.56m));

            _porcentajes.Verify(p => p.ObtenerPorcentaje(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void AplicarRecargo_MetodoDesconocido_NoAgregaRecargo()
        {
            // El mock devuelve 0 para lo que no configuramos, igual que MetodoDePagoNegocio cuando no encuentra el método.
            Assert.That(_servicio.AplicarRecargo(800m, "Bitcoin"), Is.EqualTo(800m));
        }

        [Test]
        public void AplicarRecargo_SiLaBaseFalla_LaExcepcionLlegaAlLlamador()
        {
            _porcentajes.Setup(p => p.ObtenerPorcentaje("Débito")).Throws(new InvalidOperationException("sin conexión"));

            Assert.Throws<InvalidOperationException>(() => _servicio.AplicarRecargo(100m, "Débito"));
        }

        // ---------- Calcular (subtotal + recargo + total) ----------

        [Test]
        public void Calcular_ArmaSubtotalRecargoYTotal()
        {
            var resultado = _servicio.Calcular(Carrito(100m, 100m, 50m), "Crédito");

            Assert.That(resultado.Subtotal, Is.EqualTo(250m));
            Assert.That(resultado.Porcentaje, Is.EqualTo(10m));
            Assert.That(resultado.Recargo, Is.EqualTo(25m));
            Assert.That(resultado.Total, Is.EqualTo(275m));
        }

        [TestCase("Efectivo")]
        [TestCase("Débito")]
        [TestCase("Crédito")]
        [TestCase("Transferencia")]
        public void Calcular_ElTotalSiempreEsSubtotalMasRecargo(string metodo)
        {
            var resultado = _servicio.Calcular(Carrito(19.99m, 5.50m, 120m), metodo);

            Assert.That(resultado.Total, Is.EqualTo(resultado.Subtotal + resultado.Recargo));
        }

        [Test]
        public void Calcular_Efectivo_NoTieneRecargo()
        {
            var resultado = _servicio.Calcular(Carrito(100m, 200m), "Efectivo");

            Assert.That(resultado.Recargo, Is.EqualTo(0m));
            Assert.That(resultado.Total, Is.EqualTo(resultado.Subtotal));
        }

        [Test]
        public void Calcular_CarritoVacio_TodoEnCero()
        {
            var resultado = _servicio.Calcular(new List<Articulos>(), "Crédito");

            Assert.That(resultado.Subtotal, Is.EqualTo(0m));
            Assert.That(resultado.Recargo, Is.EqualTo(0m));
            Assert.That(resultado.Total, Is.EqualTo(0m));
        }

        [Test]
        public void Calcular_SinMetodo_NoAplicaRecargoNiConsulta()
        {
            var resultado = _servicio.Calcular(Carrito(100m), null);

            Assert.That(resultado.Total, Is.EqualTo(100m));
            _porcentajes.Verify(p => p.ObtenerPorcentaje(It.IsAny<string>()), Times.Never());
        }

        // ---------- Contrato ----------

        [Test]
        public void Constructor_ConDependenciaNula_Lanza()
        {
            Assert.Throws<ArgumentNullException>(() => new ServicioCobro(null));
        }

        [Test]
        public void MetodoDePagoNegocio_ImplementaLaInterfazQueUsaElServicio()
        {
            // Garantiza que la clase real (la que lee la base) puede reemplazar al mock en producción.
            Assert.That(typeof(IPorcentajeMetodoPago).IsAssignableFrom(typeof(MetodoDePagoNegocio)), Is.True);
        }
    }
}
