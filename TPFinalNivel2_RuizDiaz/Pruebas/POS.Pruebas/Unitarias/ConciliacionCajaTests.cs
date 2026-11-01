using System.Collections.Generic;
using Dominio;
using Negocio_.Calculos;
using NUnit.Framework;

namespace POS.Pruebas.Unitarias
{
    [TestFixture]
    [Category("Unitaria")]
    public class ConciliacionCajaTests
    {
        // ---------- Efectivo ----------

        [Test]
        public void EfectivoNeto_EntradasMenosProveedoresMenosGastos()
        {
            // Entradas: ventas en efectivo 1000 + cobro a deudores 100.
            Assert.That(ConciliacionCaja.EfectivoNeto(1100m, 150m, 80m), Is.EqualTo(870m));
        }

        [Test]
        public void EfectivoNeto_SinSalidas_EsIgualALasEntradas()
        {
            Assert.That(ConciliacionCaja.EfectivoNeto(500m, 0m, 0m), Is.EqualTo(500m));
        }

        [Test]
        public void EfectivoNeto_SiSalePlataDeMas_QuedaNegativo()
        {
            Assert.That(ConciliacionCaja.EfectivoNeto(100m, 150m, 0m), Is.EqualTo(-50m));
        }

        // ---------- Métodos digitales ----------

        [Test]
        public void NetoMetodo_VentasMasCobrosMenosSalidas()
        {
            // Débito: ventas 500 + cobros 50 - proveedores 50 - gastos 20.
            Assert.That(ConciliacionCaja.NetoMetodo(500m, 50m, 50m, 20m), Is.EqualTo(480m));
        }

        [Test]
        public void NetoMetodo_SinMovimientos_EsCero()
        {
            Assert.That(ConciliacionCaja.NetoMetodo(0m, 0m, 0m, 0m), Is.EqualTo(0m));
        }


        // ---------- Cierre completo ----------

        [Test]
        public void CierreDeCaja_ConciliaEfectivoYMetodosDigitales()
        {
            // Efectivo: ventas 1000 + cobros 100, proveedores 150, gastos 80.
            decimal efectivo = ConciliacionCaja.EfectivoNeto(1000m + 100m, 150m, 80m);
            // Débito: ventas 500 + cobros 50, proveedores 50, gastos 20.
            decimal debito = ConciliacionCaja.NetoMetodo(500m, 50m, 50m, 20m);

            var informe = new InformeCaja
            {
                EfectivoVentas = efectivo,
                FondoInicialEfectivo = 500m,
                DesgloseMetodos = new List<MetodoResumen> { new MetodoResumen { Nombre = "Débito", Total = debito } }
            };

            Assert.That(efectivo, Is.EqualTo(870m));
            Assert.That(debito, Is.EqualTo(480m));
            Assert.That(informe.TotalGeneralRecaudado, Is.EqualTo(1350m));
        }

        [Test]
        public void CierreDeCaja_ElFondoInicialNoSeSumaAlTotalRecaudado()
        {
            var sinFondo = new InformeCaja { EfectivoVentas = 870m, FondoInicialEfectivo = 0m };
            var conFondo = new InformeCaja { EfectivoVentas = 870m, FondoInicialEfectivo = 5000m };

            Assert.That(conFondo.TotalGeneralRecaudado, Is.EqualTo(sinFondo.TotalGeneralRecaudado));
        }
    }
}
