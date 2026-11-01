using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Dominio;
using NUnit.Framework;
using TPFinalNivel2_RuizDiaz;

namespace POS.Pruebas.Unitarias
{
    // Una tecla suscripta dos veces ejecuta su acción dos veces. En la pantalla de cobro eso guardaba
    // la venta duplicada con un solo Enter, así que se vigila que cada manejador quede suscripto UNA vez.
    [TestFixture]
    [NUnit.Framework.Category("Unitaria")]
    [Apartment(ApartmentState.STA)]
    public class SuscripcionesDeTecladoTests
    {
        private static int SuscriptoresDeKeyDown(Control control)
        {
            object clave = typeof(Control).GetField("EventKeyDown", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            var eventos = (EventHandlerList)typeof(Component).GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(control, null);
            Delegate manejadores = eventos[clave];
            return manejadores == null ? 0 : manejadores.GetInvocationList().Length;
        }

        [Test]
        public void PantallaDeCobro_EnterSeSuscribeUnaSolaVez()
        {
            using (var pantalla = new TOTAL_A_COBRAR(100m, 100m, new List<Articulos>(), 2, "Efectivo"))
            {
                // Se dispara el evento Load sin mostrar la ventana (ahí era donde se suscribía de más).
                typeof(Form).GetMethod("OnLoad", BindingFlags.NonPublic | BindingFlags.Instance)
                            .Invoke(pantalla, new object[] { EventArgs.Empty });

                Assert.That(SuscriptoresDeKeyDown(pantalla), Is.EqualTo(1));
            }
        }

        [Test]
        public void AltaDeArticulo_CampoCodigo_SuSuscribeUnaSolaVez()
        {
            using (var pantalla = new AltaArticulo())
            {
                var campo = (TextBox)pantalla.GetType()
                    .GetField("txtCodigo", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(pantalla);

                Assert.That(SuscriptoresDeKeyDown(campo), Is.EqualTo(1));
            }
        }
    }
}
