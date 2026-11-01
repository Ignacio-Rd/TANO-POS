using System.Collections.Generic;
using Dominio;
using NUnit.Framework;

namespace POS.Pruebas.Unitarias
{
    [TestFixture]
    [Category("Unitaria")]
    public class InformeCajaTests
    {
        [Test]
        public void InformeNuevo_ArrancaConElDesgloseVacioYTodoEnCero()
        {
            var informe = new InformeCaja();

            Assert.That(informe.DesgloseMetodos, Is.Not.Null);
            Assert.That(informe.DesgloseMetodos, Is.Empty);
            Assert.That(informe.TotalGeneralRecaudado, Is.EqualTo(0m));
        }

        [Test]
        public void TotalGeneralRecaudado_SumaEfectivoMasElDesglose()
        {
            var informe = new InformeCaja
            {
                EfectivoVentas = 1000m,
                DesgloseMetodos = new List<MetodoResumen>
                {
                    new MetodoResumen { Nombre = "Débito", Total = 300m },
                    new MetodoResumen { Nombre = "Crédito", Total = 200m }
                }
            };

            Assert.That(informe.TotalGeneralRecaudado, Is.EqualTo(1500m));
        }

        [Test]
        public void EfectivoTotalReal_EsElEfectivoDeVentas()
        {
            var informe = new InformeCaja { EfectivoVentas = 777.77m };

            Assert.That(informe.EfectivoTotalReal, Is.EqualTo(777.77m));
        }

        [Test]
        public void TotalGeneralRecaudado_EsDeSoloLectura()
        {
            // Antes tenía un setter que descartaba el valor y confundía: ahora no se puede asignar.
            var propiedad = typeof(InformeCaja).GetProperty("TotalGeneralRecaudado");

            Assert.That(propiedad.CanRead, Is.True);
            Assert.That(propiedad.CanWrite, Is.False);
        }

        [Test]
        public void TotalGeneralRecaudado_SinMetodosDigitales_EsElEfectivo()
        {
            var informe = new InformeCaja { EfectivoVentas = 870m };

            Assert.That(informe.TotalGeneralRecaudado, Is.EqualTo(870m));
        }

        [Test]
        public void TotalGeneralRecaudado_UnMetodoNegativoReduceElTotal()
        {
            var informe = new InformeCaja
            {
                EfectivoVentas = 500m,
                DesgloseMetodos = new List<MetodoResumen> { new MetodoResumen { Nombre = "Débito", Total = -100m } }
            };

            Assert.That(informe.TotalGeneralRecaudado, Is.EqualTo(400m));
        }
    }
}
