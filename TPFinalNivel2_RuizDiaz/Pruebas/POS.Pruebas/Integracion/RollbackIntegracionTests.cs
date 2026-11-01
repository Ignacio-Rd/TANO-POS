using System;
using System.Transactions;
using Negocio_;
using NUnit.Framework;

namespace POS.Pruebas.Integracion
{
    // Demuestra el patrón en sí: lo escrito dentro de un TransactionScope sin Complete() no queda en la base.
    [TestFixture]
    [Category("Integracion")]
    public class RollbackIntegracionTests : BaseConexion
    {
        [Test]
        public void AlDescartarElScope_LaVentaNoQuedaGuardada()
        {
            int ventasAntes = Escalar<int>("SELECT COUNT(*) FROM Ventas");
            int metodo = IdMetodo("Efectivo");

            using (var scope = new TransactionScope())
            {
                new VentaNegocio().Agregar_venta(DateTime.Now, 123.45m, metodo);

                // Dentro del scope la venta se ve...
                Assert.That(Escalar<int>("SELECT COUNT(*) FROM Ventas"), Is.EqualTo(ventasAntes + 1));

                // ...y al salir SIN llamar a scope.Complete(), SQL Server la deshace.
            }

            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Ventas"), Is.EqualTo(ventasAntes));
        }
    }
}
