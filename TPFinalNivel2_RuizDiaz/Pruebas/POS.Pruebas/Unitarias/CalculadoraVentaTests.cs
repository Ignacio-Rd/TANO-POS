using System.Collections.Generic;
using Dominio;
using Negocio_.Calculos;
using NUnit.Framework;

namespace POS.Pruebas.Unitarias
{
    [TestFixture]
    [Category("Unitaria")]
    public class CalculadoraVentaTests
    {
        private static Articulos Articulo(decimal precio)
        {
            return new Articulos { Precio = precio };
        }

        // ---------- Subtotal ----------

        [Test]
        public void Subtotal_SumaElPrecioDeCadaUnidadCargada()
        {
            var carrito = new List<Articulos> { Articulo(100m), Articulo(100m), Articulo(250.50m) };

            Assert.That(CalculadoraVenta.Subtotal(carrito), Is.EqualTo(450.50m));
        }

        [Test]
        public void Subtotal_CarritoVacio_EsCero()
        {
            Assert.That(CalculadoraVenta.Subtotal(new List<Articulos>()), Is.EqualTo(0m));
        }

        [Test]
        public void Subtotal_ListaNula_EsCero()
        {
            Assert.That(CalculadoraVenta.Subtotal(null), Is.EqualTo(0m));
        }

        [Test]
        public void Subtotal_IgnoraLosElementosNulos()
        {
            var carrito = new List<Articulos> { Articulo(10m), null, Articulo(5m) };

            Assert.That(CalculadoraVenta.Subtotal(carrito), Is.EqualTo(15m));
        }

        // ---------- Recargo por método de pago ----------
        // Porcentajes de la base de datos original: Efectivo 0, Débito 5, Crédito 10, Transferencia 3.

        [TestCase("Efectivo", 0, 1000, 1000)]
        [TestCase("Débito", 5, 1000, 1050)]
        [TestCase("Crédito", 10, 1000, 1100)]
        [TestCase("Transferencia", 3, 1000, 1030)]
        public void AplicarRecargo_DeLosMetodosDePagoHabituales(string metodo, int porcentaje, int importe, int esperado)
        {
            Assert.That(CalculadoraVenta.AplicarRecargo(importe, porcentaje), Is.EqualTo((decimal)esperado), metodo);
        }

        [Test]
        public void AplicarRecargo_ImporteCero_SigueEnCero()
        {
            Assert.That(CalculadoraVenta.AplicarRecargo(0m, 10m), Is.EqualTo(0m));
        }

        [Test]
        public void AplicarRecargo_NoRedondea()
        {
            // 99,99 + 10% = 109,989. El redondeo lo hace la base al guardar (DECIMAL(18,2)).
            Assert.That(CalculadoraVenta.AplicarRecargo(99.99m, 10m), Is.EqualTo(109.989m));
        }

        [Test]
        public void AplicarRecargo_ConPorcentajeNegativo_ActuaComoDescuento()
        {
            Assert.That(CalculadoraVenta.AplicarRecargo(100m, -10m), Is.EqualTo(90m));
        }

        // ---------- Recargo por línea del ticket ----------

        [TestCase(300, 5, 15)]
        [TestCase(250, 5, 12.5)]
        [TestCase(100, 0, 0)]
        public void RecargoDeLinea_CalculaElPorcentajeSobreElImporteBase(int importeBase, int porcentaje, double esperado)
        {
            Assert.That(CalculadoraVenta.RecargoDeLinea(importeBase, porcentaje), Is.EqualTo((decimal)esperado));
        }

        [TestCase(300, 5, 315)]
        [TestCase(250, 5, 262.5)]
        [TestCase(100, 10, 110)]
        [TestCase(100, 0, 100)]
        public void ImporteDeLineaConRecargo_SumaImporteMasRecargo(int importeBase, int porcentaje, double esperado)
        {
            Assert.That(CalculadoraVenta.ImporteDeLineaConRecargo(importeBase, porcentaje), Is.EqualTo((decimal)esperado));
        }

        [Test]
        public void RecargoDeLinea_RedondeaLasMitadesHaciaArriba_IgualQueSqlServer()
        {
            // 2,50 al 5% => recargo 0,125 => 0,13 (antes salía 0,12 por el redondeo "al par").
            Assert.That(CalculadoraVenta.RecargoDeLinea(2.50m, 5m), Is.EqualTo(0.13m));
            Assert.That(CalculadoraVenta.ImporteDeLineaConRecargo(2.50m, 5m), Is.EqualTo(2.63m));
        }

        [TestCase(0.125, 0.13)]
        [TestCase(0.135, 0.14)]
        [TestCase(0.145, 0.15)]
        [TestCase(0.155, 0.16)]
        public void RecargoDeLinea_UnaMitadSiempreSubeAlCentavoSiguiente(double recargoExacto, double esperado)
        {
            // Importe 100 con el porcentaje que produce exactamente ese recargo.
            decimal porcentaje = (decimal)recargoExacto;
            Assert.That(CalculadoraVenta.RecargoDeLinea(100m, porcentaje), Is.EqualTo((decimal)esperado));
        }
    }
}
