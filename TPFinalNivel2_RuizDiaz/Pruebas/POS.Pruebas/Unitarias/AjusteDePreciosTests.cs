using System;
using Negocio_;
using Negocio_.Calculos;
using NUnit.Framework;

namespace POS.Pruebas.Unitarias
{
    [TestFixture]
    [Category("Unitaria")]
    public class AjusteDePreciosTests
    {
        [TestCase("PORC_COSTOVENTA", "UPDATE Articulos SET PrecioCosto *= (1 + @val/100), Precio *= (1 + @val/100)")]
        [TestCase("CIFRA_COSTOVENTA", "UPDATE Articulos SET PrecioCosto += @val, Precio += @val")]
        [TestCase("PORC_VENTA", "UPDATE Articulos SET Precio *= (1 + @val/100)")]
        [TestCase("CIFRA_VENTA", "UPDATE Articulos SET Precio += @val")]
        [TestCase("PORC_COSTO", "UPDATE Articulos SET PrecioCosto *= (1 + @val/100)")]
        [TestCase("CIFRA_COSTO", "UPDATE Articulos SET PrecioCosto += @val")]
        public void SentenciaUpdate_DevuelveElUpdateDeCadaTipo(string tipo, string esperado)
        {
            Assert.That(AjusteDePrecios.SentenciaUpdate(tipo), Is.EqualTo(esperado));
        }

        [TestCase("TIPO_INEXISTENTE")]
        [TestCase("porc_venta")]   // distingue mayúsculas: los tags del formulario son exactos
        [TestCase("")]
        [TestCase(null)]
        public void SentenciaUpdate_ConTipoDesconocido_LanzaArgumentException(string tipo)
        {
            var error = Assert.Throws<ArgumentException>(() => AjusteDePrecios.SentenciaUpdate(tipo));

            Assert.That(error.ParamName, Is.EqualTo("tipoAjuste"));
        }
    }

    [TestFixture]
    [Category("Unitaria")]
    public class VentaPorProductoNegocioTests
    {
        [Test]
        public void AgregarVentaProducto_ConListaNula_LanzaArgumentNullException_SinTocarLaBase()
        {
            // La guarda va antes de cualquier consulta, así que no hace falta base de datos.
            var error = Assert.Throws<ArgumentNullException>(() =>
                new Venta_Por_Producto_Negocio().Agregar_Venta_Producto(null, DateTime.Now, 1, 0m, "Efectivo", 0m));

            Assert.That(error.ParamName, Is.EqualTo("producto_Vendidos"));
        }
    }
}
