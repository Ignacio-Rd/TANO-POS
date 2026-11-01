using System;
using System.IO;
using NUnit.Framework;
using TPFinalNivel2_RuizDiaz.Infraestructura;
using Negocio_;

namespace POS.Pruebas.Unitarias
{
    [TestFixture]
    [NUnit.Framework.Category("Unitaria")]
    public class InfraestructuraTests
    {
        // Cada prueba usa su propio nombre de mutex para no chocar con el programa real ni entre sí.
        private static string NombreUnico()
        {
            return @"Local\TANO_POS_PRUEBA_" + Guid.NewGuid().ToString("N");
        }

        // ---------- Instancia única ----------

        [Test]
        public void InstanciaUnica_LaPrimeraCopiaEntra()
        {
            using (InstanciaUnica primera = InstanciaUnica.Tomar(NombreUnico()))
                Assert.That(primera, Is.Not.Null);
        }

        [Test]
        public void InstanciaUnica_UnaSegundaCopiaEsRechazada()
        {
            string nombre = NombreUnico();

            using (InstanciaUnica primera = InstanciaUnica.Tomar(nombre))
            {
                Assert.That(primera, Is.Not.Null);
                Assert.That(InstanciaUnica.Tomar(nombre), Is.Null, "la segunda copia no tiene que abrir");
            }
        }

        [Test]
        public void InstanciaUnica_AlCerrarLaPrimeraSePuedeVolverAAbrir()
        {
            string nombre = NombreUnico();

            InstanciaUnica.Tomar(nombre).Dispose();

            using (InstanciaUnica otra = InstanciaUnica.Tomar(nombre))
                Assert.That(otra, Is.Not.Null);
        }

        // ---------- Manejo global de errores ----------

        [Test]
        public void ManejadorErrores_AnotaElDetalleCompletoYAvisaConUnMensajeClaro()
        {
            string marca = "ERROR-DE-PRUEBA-" + Guid.NewGuid().ToString("N");
            string avisado = null;
            bool? cierra = null;

            ManejadorErrores.Atender(new InvalidOperationException(marca), "Prueba del manejador", false,
                                     (mensaje, seCierra) => { avisado = mensaje; cierra = seCierra; });

            string log = File.ReadAllText(RegistroErrores.Ruta);
            Assert.That(log, Does.Contain(marca));
            Assert.That(log, Does.Contain("InvalidOperationException"), "se guarda el tipo y la pila, no solo el mensaje");
            Assert.That(log, Does.Contain("Prueba del manejador"));
            Assert.That(avisado, Is.EqualTo(ManejadorErrores.Mensaje(false)));
            Assert.That(avisado, Does.Not.Contain(marca), "a la persona no se le muestra el detalle técnico");
            Assert.That(cierra, Is.False);
        }

        [Test]
        public void ManejadorErrores_SiElAvisoMismoFalla_NoPropagaLaExcepcion()
        {
            Assert.DoesNotThrow(() =>
                ManejadorErrores.Atender(new Exception("x"), "Prueba", false, (m, c) => { throw new InvalidOperationException("no se pudo avisar"); }));
        }

        [Test]
        public void ManejadorErrores_SiSeVaACerrarElMensajeLoDice()
        {
            Assert.That(ManejadorErrores.Mensaje(true), Does.Contain("se va a cerrar"));
            Assert.That(ManejadorErrores.Mensaje(false), Does.Contain("seguir trabajando"));
        }

        [Test]
        public void ManejadorErrores_UnErrorMientrasSeMuestraElAvisoSoloSeAnota()
        {
            int avisos = 0;
            string marca = "ANIDADO-" + Guid.NewGuid().ToString("N");

            ManejadorErrores.Atender(new Exception("primero"), "Prueba", false, (m, c) =>
            {
                avisos++;
                // Mientras el primer aviso está abierto salta otro error: no se debe abrir un segundo cuadro.
                ManejadorErrores.Atender(new Exception(marca), "Prueba anidada", false, (m2, c2) => avisos++);
            });

            Assert.That(avisos, Is.EqualTo(1));
            Assert.That(File.ReadAllText(RegistroErrores.Ruta), Does.Contain(marca), "igual queda anotado");
        }
    }
}
