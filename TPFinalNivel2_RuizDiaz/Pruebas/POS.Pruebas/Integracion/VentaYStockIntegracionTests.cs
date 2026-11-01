using System;
using System.Collections.Generic;
using System.Linq;
using Dominio;
using Negocio_;
using Negocio_.Calculos;
using Negocios;
using NUnit.Framework;

namespace POS.Pruebas.Integracion
{
    // Estas pruebas usan las clases REALES de Negocio contra SQL Server (base de pruebas) y se deshacen solas.
    [TestFixture]
    [Category("Integracion")]
    public class VentaYStockIntegracionTests : BaseIntegracion
    {
        private static Articulos EnCarrito(int id, string codigo, string nombre)
        {
            return new Articulos { Id = id, codigo = codigo, Nombre = nombre };
        }

        // ---------- Guardado de la venta ----------

        [Test]
        public void AgregarVenta_GuardaLaVentaYDevuelveSuId()
        {
            int metodo = IdMetodo("Efectivo");

            int idVenta = new VentaNegocio().Agregar_venta(DateTime.Now, 1050.50m, metodo);

            Assert.That(idVenta, Is.GreaterThan(0));
            Assert.That(Escalar<decimal>("SELECT Total FROM Ventas WHERE Id = @id", P("@id", idVenta)), Is.EqualTo(1050.50m));
            Assert.That(Escalar<int>("SELECT id_metodo FROM Ventas WHERE Id = @id", P("@id", idVenta)), Is.EqualTo(metodo));
        }

        [Test]
        public void AgregarVenta_DosVentasRecibenIdsDistintos()
        {
            var negocio = new VentaNegocio();
            int metodo = IdMetodo("Efectivo");

            int primera = negocio.Agregar_venta(DateTime.Now, 10m, metodo);
            int segunda = negocio.Agregar_venta(DateTime.Now, 20m, metodo);

            Assert.That(segunda, Is.GreaterThan(primera));
        }

        // ---------- Stock ----------

        [Test]
        public void DescontarStock_RestaLaCantidadVendida()
        {
            string codigo = CodigoUnico();
            CrearArticulo(codigo, "Producto stock", 100m, 60m, stock: 10);

            new ArticuloNegocio().Descontar_Stock(codigo, 3);

            Assert.That(StockDe(codigo), Is.EqualTo(7));
        }

        [Test]
        public void DescontarStock_PuedeVenderTodoElStock()
        {
            string codigo = CodigoUnico();
            CrearArticulo(codigo, "Producto stock", 100m, 60m, stock: 4);

            new ArticuloNegocio().Descontar_Stock(codigo, 4);

            Assert.That(StockDe(codigo), Is.EqualTo(0));
        }

        [Test]
        public void DescontarStock_SoloAfectaAlArticuloIndicado()
        {
            string vendido = CodigoUnico();
            string otro = CodigoUnico();
            CrearArticulo(vendido, "Vendido", 100m, 60m, stock: 10);
            CrearArticulo(otro, "Otro", 100m, 60m, stock: 10);

            new ArticuloNegocio().Descontar_Stock(vendido, 2);

            Assert.That(StockDe(vendido), Is.EqualTo(8));
            Assert.That(StockDe(otro), Is.EqualTo(10));
        }

        [Test]
        public void DescontarStock_ConStockSuficiente_DevuelveTrue()
        {
            string codigo = CodigoUnico();
            CrearArticulo(codigo, "Con stock", 100m, 60m, stock: 5);

            Assert.That(new ArticuloNegocio().Descontar_Stock(codigo, 2), Is.True);
            Assert.That(StockDe(codigo), Is.EqualTo(3));
        }

        [Test]
        public void DescontarStock_SiNoAlcanzaElStock_DevuelveFalse_NoDescuentaYNoLanzaError()
        {
            // El stock nunca queda negativo y ahora el resultado avisa que no se pudo descontar
            // (además queda un renglón en log_errores.txt). La venta no se frena en la caja.
            string codigo = CodigoUnico();
            CrearArticulo(codigo, "Poco stock", 100m, 60m, stock: 2);
            bool resultado = true;

            Assert.DoesNotThrow(() => resultado = new ArticuloNegocio().Descontar_Stock(codigo, 5));

            Assert.That(resultado, Is.False);
            Assert.That(StockDe(codigo), Is.EqualTo(2));
        }

        [Test]
        public void ReponerStock_SumaUnaUnidad()
        {
            string codigo = CodigoUnico();
            CrearArticulo(codigo, "Reponer", 100m, 60m, stock: 3);

            new ArticuloNegocio().Reponer_Stock(codigo);

            Assert.That(StockDe(codigo), Is.EqualTo(4));
        }

        // ---------- Detalle del ticket ----------

        [Test]
        public void AgregarVentaProducto_AgrupaUnidadesRepetidasYAplicaElRecargoDelMetodo()
        {
            string codigoA = CodigoUnico(), codigoB = CodigoUnico();
            string nombreA = "Producto A " + codigoA, nombreB = "Producto B " + codigoB;
            int idA = CrearArticulo(codigoA, nombreA, 100m, 60m, stock: 10);
            int idB = CrearArticulo(codigoB, nombreB, 250m, 150m, stock: 10);
            int metodoDebito = IdMetodo("Débito");
            decimal porcentaje = PorcentajeDe("Débito");

            decimal subtotal = 100m * 3 + 250m;
            decimal totalFinal = CalculadoraVenta.AplicarRecargo(subtotal, porcentaje);
            int idVenta = new VentaNegocio().Agregar_venta(DateTime.Now, totalFinal, metodoDebito);

            var carrito = new List<Articulos>
            {
                EnCarrito(idA, codigoA, nombreA), EnCarrito(idA, codigoA, nombreA), EnCarrito(idA, codigoA, nombreA),
                EnCarrito(idB, codigoB, nombreB)
            };

            TICKET ticket = new Venta_Por_Producto_Negocio().Agregar_Venta_Producto(carrito, DateTime.Now, idVenta, subtotal, "Débito", totalFinal);

            // Ticket
            Assert.That(ticket.IdVenta, Is.EqualTo(idVenta));
            Assert.That(ticket.total, Is.EqualTo(subtotal));
            Assert.That(ticket.TotalFinal, Is.EqualTo(totalFinal));
            Assert.That(ticket.impuestos, Is.EqualTo("Débito"));
            Assert.That(ticket.venta_Por_Productos, Has.Count.EqualTo(2));

            var lineaA = ticket.venta_Por_Productos.Single(l => l.Producto == nombreA);
            var lineaB = ticket.venta_Por_Productos.Single(l => l.Producto == nombreB);

            Assert.That(lineaA.Cantidad, Is.EqualTo(3));
            Assert.That(lineaA.PrecioUnitario, Is.EqualTo(100m));
            Assert.That(lineaA.Recargo, Is.EqualTo(CalculadoraVenta.RecargoDeLinea(300m, porcentaje)));
            Assert.That(lineaA.ImporteLinea, Is.EqualTo(CalculadoraVenta.ImporteDeLineaConRecargo(300m, porcentaje)));
            Assert.That(lineaB.Cantidad, Is.EqualTo(1));
            Assert.That(lineaB.ImporteLinea, Is.EqualTo(CalculadoraVenta.ImporteDeLineaConRecargo(250m, porcentaje)));

            // Base de datos: una fila por artículo, con el importe SIN recargo
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Venta_Por_Producto WHERE IdVenta = @id", P("@id", idVenta)), Is.EqualTo(2));
            Assert.That(Escalar<decimal>("SELECT ImporteLinea FROM Venta_Por_Producto WHERE IdVenta = @id AND IdArticulo = @a", P("@id", idVenta), P("@a", idA)), Is.EqualTo(300m));
        }

        [Test]
        public void AgregarVentaProducto_ConEfectivo_NoAgregaRecargoEnElDetalle()
        {
            string codigo = CodigoUnico();
            string nombre = "Producto " + codigo;
            int id = CrearArticulo(codigo, nombre, 80m, 50m, stock: 5);
            int idVenta = new VentaNegocio().Agregar_venta(DateTime.Now, 160m, IdMetodo("Efectivo"));

            var carrito = new List<Articulos> { EnCarrito(id, codigo, nombre), EnCarrito(id, codigo, nombre) };

            TICKET ticket = new Venta_Por_Producto_Negocio().Agregar_Venta_Producto(carrito, DateTime.Now, idVenta, 160m, "Efectivo", 160m);

            var linea = ticket.venta_Por_Productos.Single();
            Assert.That(linea.Cantidad, Is.EqualTo(2));
            Assert.That(linea.Recargo, Is.EqualTo(0m));
            Assert.That(linea.ImporteLinea, Is.EqualTo(160m));
        }

        [Test]
        public void AgregarVentaProducto_ConListaNula_LanzaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Venta_Por_Producto_Negocio().Agregar_Venta_Producto(null, DateTime.Now, 1, 0m, "Efectivo", 0m));
        }

        [Test]
        public void AgregarVentaProducto_ConListaVacia_DevuelveElTicketSinLineas()
        {
            int idVenta = new VentaNegocio().Agregar_venta(DateTime.Now, 0m, IdMetodo("Efectivo"));

            TICKET ticket = new Venta_Por_Producto_Negocio().Agregar_Venta_Producto(new List<Articulos>(), DateTime.Now, idVenta, 0m, "Efectivo", 0m);

            Assert.That(ticket.IdVenta, Is.EqualTo(idVenta));
            Assert.That(ticket.venta_Por_Productos, Is.Empty);
        }

        // ---------- Conexiones ----------

        [Test]
        public void LasOperacionesDeEscrituraNoDejanConexionesAbiertas_YNoEscalanATransaccionDistribuida()
        {
            // Antes InsertarVenta y AumentarPor* dejaban la conexión abierta: dentro de una transacción eso obligaba
            // a usar MSDTC (transacción distribuida). Ahora cada operación cierra su conexión.
            string codigo = CodigoUnico();
            string nombre = "Conexion " + codigo;
            int id = CrearArticulo(codigo, nombre, 100m, 60m, stock: 5);
            int idVenta = new VentaNegocio().Agregar_venta(DateTime.Now, 100m, IdMetodo("Efectivo"));

            new Venta_Por_Producto_Negocio().Agregar_Venta_Producto(
                new List<Articulos> { EnCarrito(id, codigo, nombre) }, DateTime.Now, idVenta, 100m, "Efectivo", 100m);
            new ArticuloNegocio().AumentarPorCodigo(codigo, 10m, "PORC_VENTA");
            new ArticuloNegocio().Descontar_Stock(codigo, 1);
            Escalar<int>("SELECT COUNT(*) FROM ARTICULOS");

            Assert.That(System.Transactions.Transaction.Current.TransactionInformation.DistributedIdentifier, Is.EqualTo(Guid.Empty));
        }

        // ---------- Flujo completo (lo que hace el botón "Imprimir" de la pantalla de cobro) ----------

        [Test]
        public void FlujoCompletoDeVenta_GuardaLaVentaDescuentaStockYArmaElDetalle()
        {
            string codigoA = CodigoUnico(), codigoB = CodigoUnico();
            string nombreA = "Flujo A " + codigoA, nombreB = "Flujo B " + codigoB;
            int idA = CrearArticulo(codigoA, nombreA, 100m, 60m, stock: 10);
            int idB = CrearArticulo(codigoB, nombreB, 50m, 30m, stock: 5);

            var carrito = new List<Articulos>
            {
                EnCarrito(idA, codigoA, nombreA), EnCarrito(idA, codigoA, nombreA),
                EnCarrito(idB, codigoB, nombreB)
            };
            // El carrito de la registradora también trae el precio de venta de cada unidad.
            carrito[0].Precio = carrito[1].Precio = 100m;
            carrito[2].Precio = 50m;

            // 1) Total con recargo, leyendo el porcentaje real de la base (como hace la registradora)
            var cobro = new ServicioCobro(new MetodoDePagoNegocio()).Calcular(carrito, "Crédito");

            // 2) Guardar venta -> descontar stock agrupado -> guardar detalle (mismo orden que la pantalla de cobro)
            int idVenta = new VentaNegocio().Agregar_venta(DateTime.Now, cobro.Total, IdMetodo("Crédito"));
            foreach (var grupo in carrito.GroupBy(a => a.codigo))
                new ArticuloNegocio().Descontar_Stock(grupo.Key, grupo.Count());
            TICKET ticket = new Venta_Por_Producto_Negocio().Agregar_Venta_Producto(carrito, DateTime.Now, idVenta, cobro.Subtotal, "Crédito", cobro.Total);

            Assert.That(cobro.Subtotal, Is.EqualTo(250m));
            Assert.That(StockDe(codigoA), Is.EqualTo(8));
            Assert.That(StockDe(codigoB), Is.EqualTo(4));
            Assert.That(Escalar<decimal>("SELECT Total FROM Ventas WHERE Id = @id", P("@id", idVenta)),
                        Is.EqualTo(Math.Round(cobro.Total, 2, MidpointRounding.AwayFromZero)));
            Assert.That(ticket.venta_Por_Productos, Has.Count.EqualTo(2));
            Assert.That(ticket.venta_Por_Productos.Sum(l => l.Cantidad), Is.EqualTo(3));
        }
    }
}
