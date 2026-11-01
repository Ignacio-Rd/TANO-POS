using System;
using System.Linq;
using Dominio;
using Negocio_;
using NUnit.Framework;

namespace POS.Pruebas.Integracion
{
    // Verifica las consultas SQL del arqueo (Venta_Por_Producto_Negocio.ObtenerResumenInforme) con datos reales.
    // Todas las filas se siembran en una fecha de 2001, así no se mezclan con los datos de la base.
    [TestFixture]
    [Category("Integracion")]
    public class ArqueoCajaIntegracionTests : BaseIntegracion
    {
        private static readonly DateTime Inicio = new DateTime(2001, 3, 5, 0, 0, 0);
        private static readonly DateTime Fin = new DateTime(2001, 3, 5, 23, 59, 59);
        private static readonly DateTime MedioDia = new DateTime(2001, 3, 5, 12, 0, 0);

        private int _efectivo, _debito, _fiado;

        [SetUp]
        public void PrepararMetodos()
        {
            _efectivo = IdMetodo("Efectivo");
            _debito = IdMetodo("Débito");
            _fiado = IdMetodo("FIADO");
        }

        private void Venta(DateTime fecha, decimal total, int metodo, int? cliente = null)
        {
            Ejecutar("INSERT INTO Ventas (Fecha, Total, id_metodo, IdCliente) VALUES (@f, @t, @m, @c)",
                     P("@f", fecha), P("@t", total), P("@m", metodo), P("@c", cliente));
        }

        private void CobroDeudor(int cliente, decimal monto, int metodo)
        {
            Ejecutar("INSERT INTO PAGOS_DEUDORES (IdCliente, Fecha, Monto, IdMetodoPago) VALUES (@c, @f, @m, @mp)",
                     P("@c", cliente), P("@f", MedioDia), P("@m", monto), P("@mp", metodo));
        }

        private void PagoProveedor(int proveedor, decimal monto, int metodo)
        {
            Ejecutar("INSERT INTO PAGOS_PROVEEDORES (IdProveedor, Monto, Fecha, IdMetodoPago) VALUES (@p, @m, @f, @mp)",
                     P("@p", proveedor), P("@m", monto), P("@f", MedioDia), P("@mp", metodo));
        }

        private void Gasto(decimal monto, int metodo)
        {
            Ejecutar("INSERT INTO PAGOS_VARIOS (IdCategoria, IdMetodoPago, Monto, Fecha) VALUES (@c, @mp, @m, @f)",
                     P("@c", IdCategoriaPagoVario()), P("@mp", metodo), P("@m", monto), P("@f", MedioDia));
        }

        [Test]
        public void SinMovimientosEnElPeriodo_ElInformeVieneEnCero()
        {
            var informe = new Venta_Por_Producto_Negocio().ObtenerResumenInforme(new DateTime(1999, 1, 1), new DateTime(1999, 1, 1, 23, 59, 59));

            Assert.That(informe.EfectivoVentas, Is.EqualTo(0m));
            Assert.That(informe.PagosAProveedores, Is.EqualTo(0m));
            Assert.That(informe.GastosVariosEfectivo, Is.EqualTo(0m));
            Assert.That(informe.CobroDeudores, Is.EqualTo(0m));
            Assert.That(informe.VentasFiadasNuevas, Is.EqualTo(0m));
            Assert.That(informe.DesgloseMetodos.Sum(m => m.Total), Is.EqualTo(0m));
        }

        [Test]
        public void ArqueoCompleto_ConciliaEfectivoMetodosDigitalesYFiado()
        {
            int cliente = CrearCliente();
            int proveedor = CrearProveedor();
            var negocio = new Venta_Por_Producto_Negocio();
            var antes = negocio.ObtenerResumenInforme(Inicio, Fin);

            // Entradas
            Venta(MedioDia, 1000m, _efectivo);
            Venta(MedioDia, 500m, _debito);
            Venta(MedioDia, 300m, _fiado, cliente);
            CobroDeudor(cliente, 100m, _efectivo);
            CobroDeudor(cliente, 50m, _debito);
            // Salidas
            PagoProveedor(proveedor, 150m, _efectivo);
            PagoProveedor(proveedor, 50m, _debito);
            Gasto(80m, _efectivo);
            Gasto(20m, _debito);

            var informe = negocio.ObtenerResumenInforme(Inicio, Fin);
            var debito = informe.DesgloseMetodos.Single(m => m.Nombre == "Débito");

            // Efectivo: (1000 + 100) - 150 - 80
            Assert.That(informe.EfectivoVentas, Is.EqualTo(870m));
            Assert.That(informe.PagosAProveedores, Is.EqualTo(150m));
            Assert.That(informe.GastosVariosEfectivo, Is.EqualTo(80m));

            // Débito: (500 + 50) - 50 - 20
            Assert.That(debito.Total, Is.EqualTo(480m));
            Assert.That(debito.PagosProveedores, Is.EqualTo(50m));
            Assert.That(debito.GastosVarios, Is.EqualTo(20m));

            // Fiado: nunca aparece como "método digital" ni suma al efectivo
            Assert.That(informe.DesgloseMetodos.Any(m => m.Nombre == "FIADO" || m.Nombre == "Efectivo"), Is.False);
            Assert.That(informe.VentasFiadasNuevas, Is.EqualTo(300m));
            Assert.That(informe.CobroDeudores, Is.EqualTo(150m));
            // La deuda del cliente sube 300 - 150 = 150 respecto de antes de sembrar.
            Assert.That(informe.SaldoHistorico - antes.SaldoHistorico, Is.EqualTo(150m));

            // Total recaudado = efectivo neto + neto digital
            Assert.That(informe.TotalGeneralRecaudado, Is.EqualTo(870m + informe.DesgloseMetodos.Sum(m => m.Total)));
            Assert.That(informe.TotalGeneralRecaudado, Is.EqualTo(1350m));
        }

        [Test]
        public void ElFondoInicialDelEfectivoSeInformaPeroNoSeSumaAlTotal()
        {
            Venta(MedioDia, 200m, _efectivo);

            var informe = new Venta_Por_Producto_Negocio().ObtenerResumenInforme(Inicio, Fin);

            decimal fondoEnBase = Escalar<decimal>("SELECT fondo_inicial FROM metodos_de_pago WHERE metodo_pago = 'Efectivo'");
            Assert.That(informe.FondoInicialEfectivo, Is.EqualTo(fondoEnBase));
            Assert.That(informe.EfectivoVentas, Is.EqualTo(200m));
        }

        [Test]
        public void ElRangoDeFechasEsInclusivoEnLosExtremosYExcluyeLoDeAfuera()
        {
            Venta(Inicio, 10m, _efectivo);                       // justo al inicio: entra
            Venta(Fin, 20m, _efectivo);                          // justo al final: entra
            Venta(Inicio.AddSeconds(-1), 1000m, _efectivo);      // un segundo antes: NO entra
            Venta(Fin.AddSeconds(1), 2000m, _efectivo);          // un segundo después: NO entra

            var informe = new Venta_Por_Producto_Negocio().ObtenerResumenInforme(Inicio, Fin);

            Assert.That(informe.EfectivoVentas, Is.EqualTo(30m));
        }

        [Test]
        public void UnaVentaConMetodoDigitalAparecePorSuNombreYNoEnElEfectivo()
        {
            Venta(MedioDia, 700m, _debito);

            var informe = new Venta_Por_Producto_Negocio().ObtenerResumenInforme(Inicio, Fin);

            Assert.That(informe.EfectivoVentas, Is.EqualTo(0m));
            Assert.That(informe.DesgloseMetodos.Single(m => m.Nombre == "Débito").Total, Is.EqualTo(700m));
        }
    }
}
