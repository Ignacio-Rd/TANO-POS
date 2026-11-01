using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Negocio_;
using NUnit.Framework;

namespace POS.Pruebas.Integracion
{
    // Estas pruebas NO usan el rollback automático de BaseIntegracion: lo que se prueba es justamente el
    // BEGIN / COMMIT / ROLLBACK que hace el propio código de venta, así que tienen que correr con transacciones reales
    // y limpiar a mano (en TearDown) todo lo que crean.
    [TestFixture]
    [NUnit.Framework.Category("Integracion")]
    public class VentaAtomicaIntegracionTests : BaseConexion
    {
        private readonly List<int> _articulos = new List<int>();
        private readonly List<int> _clientes = new List<int>();

        [SetUp]
        public void Preparar()
        {
            _articulos.Clear();
            _clientes.Clear();
            QuitarTriggerDeFalla();
        }

        [TearDown]
        public void Limpiar()
        {
            QuitarTriggerDeFalla();

            foreach (int articulo in _articulos)
            {
                var p = P("@a", articulo);
                Ejecutar(
                    "DECLARE @v TABLE (Id int); " +
                    "INSERT @v SELECT DISTINCT IdVenta FROM Venta_Por_Producto WHERE IdArticulo = @a; " +
                    "DELETE FROM DETALLE_COMPRAS_FIADO WHERE IdVenta IN (SELECT Id FROM @v); " +
                    "DELETE FROM Venta_Por_Producto WHERE IdVenta IN (SELECT Id FROM @v); " +
                    "DELETE FROM Ventas WHERE Id IN (SELECT Id FROM @v); " +
                    "DECLARE @au TABLE (Id int); " +
                    "INSERT @au SELECT DISTINCT IdAuditoria FROM VENTAS_ELIMINADAS_DETALLE WHERE IdArticulo = @a; " +
                    "DELETE FROM VENTAS_ELIMINADAS_DETALLE WHERE IdAuditoria IN (SELECT Id FROM @au); " +
                    "DELETE FROM VENTAS_ELIMINADAS WHERE IdAuditoria IN (SELECT Id FROM @au); " +
                    "DELETE FROM ARTICULOS WHERE Id = @a", p);
            }

            foreach (int cliente in _clientes)
            {
                Ejecutar("DELETE FROM PAGOS_DEUDORES WHERE IdCliente = @c; DELETE FROM Clientes WHERE Id = @c", P("@c", cliente));
            }
        }

        // ---------- Ayudas ----------

        private int NuevoArticulo(string codigo, decimal precio, decimal costo, int stock, string nombre = "Articulo atomico")
        {
            int id = CrearArticulo(codigo, nombre, precio, costo, stock);
            _articulos.Add(id);
            return id;
        }

        private int NuevoCliente()
        {
            int id = CrearCliente();
            _clientes.Add(id);
            return id;
        }

        private static List<Articulos> Unidades(int idArticulo, string codigo, int cantidad)
        {
            return Enumerable.Range(0, cantidad)
                             .Select(_ => new Articulos { Id = idArticulo, codigo = codigo, Nombre = "x" })
                             .ToList();
        }

        private static int CantidadDeVentas()
        {
            return Escalar<int>("SELECT COUNT(*) FROM Ventas");
        }

        private static void QuitarTriggerDeFalla()
        {
            Ejecutar("IF OBJECT_ID('dbo.trg_prueba_falla', 'TR') IS NOT NULL DROP TRIGGER dbo.trg_prueba_falla");
        }

        // ---------- Guardado ----------

        [Test]
        public void RegistrarVentaCompleta_GuardaVentaLineasYDescuentaStock()
        {
            string c1 = CodigoUnico(), c2 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10);
            int a2 = NuevoArticulo(c2, 50m, 20m, stock: 5);
            int efectivo = IdMetodo("Efectivo");

            var carrito = Unidades(a1, c1, 3).Concat(Unidades(a2, c2, 1)).ToList();
            ResultadoVenta r = new VentaNegocio().RegistrarVentaCompleta(carrito, efectivo, "Efectivo", 350m, 350m);

            Assert.That(r.IdVenta, Is.GreaterThan(0));
            Assert.That(Escalar<decimal>("SELECT Total FROM Ventas WHERE Id = @v", P("@v", r.IdVenta)), Is.EqualTo(350m));
            Assert.That(Escalar<int>("SELECT id_metodo FROM Ventas WHERE Id = @v", P("@v", r.IdVenta)), Is.EqualTo(efectivo));
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Venta_Por_Producto WHERE IdVenta = @v", P("@v", r.IdVenta)), Is.EqualTo(2));
            Assert.That(Escalar<int>("SELECT Cantidad FROM Venta_Por_Producto WHERE IdVenta = @v AND IdArticulo = @a", P("@v", r.IdVenta), P("@a", a1)), Is.EqualTo(3));
            Assert.That(StockDe(c1), Is.EqualTo(7));
            Assert.That(StockDe(c2), Is.EqualTo(4));
            Assert.That(r.Advertencias, Is.Empty);
        }

        [Test]
        public void RegistrarVentaCompleta_ElTicketCoincideConLoQueDespuesSeLeeDeLaBase()
        {
            // Precios con medios centavos para comprobar que el redondeo del ticket es el mismo que el de SQL Server.
            string c1 = CodigoUnico(), c2 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 33.335m, 10m, stock: 10, nombre: "Alfajor");
            int a2 = NuevoArticulo(c2, 12.5m, 5m, stock: 10, nombre: "Gaseosa");
            int debito = IdMetodo("Débito");
            decimal pct = PorcentajeDe("Débito");

            var carrito = Unidades(a1, c1, 3).Concat(Unidades(a2, c2, 2)).ToList();
            decimal subtotal = 3 * 33.335m + 2 * 12.5m;
            decimal total = subtotal + subtotal * pct / 100;

            ResultadoVenta r = new VentaNegocio().RegistrarVentaCompleta(carrito, debito, "Débito", subtotal, total);
            List<Venta_Por_Producto> leidas = new Venta_Por_Producto_Negocio().ObtenerDetallePorId(r.IdVenta).OrderBy(l => l.Producto).ToList();
            List<Venta_Por_Producto> delTicket = r.Ticket.venta_Por_Productos.OrderBy(l => l.Producto).ToList();

            Assert.That(r.Ticket.IdVenta, Is.EqualTo(r.IdVenta));
            Assert.That(r.Ticket.total, Is.EqualTo(subtotal));
            Assert.That(r.Ticket.TotalFinal, Is.EqualTo(total));
            Assert.That(r.Ticket.impuestos, Is.EqualTo("Débito"));
            Assert.That(delTicket.Count, Is.EqualTo(leidas.Count));

            for (int i = 0; i < leidas.Count; i++)
            {
                Assert.That(delTicket[i].Producto, Is.EqualTo(leidas[i].Producto));
                Assert.That(delTicket[i].Cantidad, Is.EqualTo(leidas[i].Cantidad));
                Assert.That(delTicket[i].PrecioUnitario, Is.EqualTo(leidas[i].PrecioUnitario), "precio de " + leidas[i].Producto);
                Assert.That(delTicket[i].Recargo, Is.EqualTo(leidas[i].Recargo), "recargo de " + leidas[i].Producto);
                Assert.That(delTicket[i].ImporteLinea, Is.EqualTo(leidas[i].ImporteLinea), "importe de " + leidas[i].Producto);
            }
        }

        [Test]
        public void RegistrarVentaCompleta_SiAlgoFallaAMitad_NoQuedaNadaGuardado()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10);
            int efectivo = IdMetodo("Efectivo");
            int ventasAntes = CantidadDeVentas();

            // El primer artículo es válido (se inserta la venta, su línea y se descuenta el stock); el segundo no existe.
            var carrito = Unidades(a1, c1, 2).Concat(Unidades(2000000000, "NO-EXISTE", 1)).ToList();

            Assert.Throws<InvalidOperationException>(() =>
                new VentaNegocio().RegistrarVentaCompleta(carrito, efectivo, "Efectivo", 300m, 300m));

            Assert.That(CantidadDeVentas(), Is.EqualTo(ventasAntes), "no debe quedar una venta guardada");
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Venta_Por_Producto WHERE IdArticulo = @a", P("@a", a1)), Is.EqualTo(0), "no deben quedar líneas");
            Assert.That(StockDe(c1), Is.EqualTo(10), "el stock no debe haberse descontado");
        }

        [Test]
        public void RegistrarVentaCompleta_ConMetodoDePagoInexistente_NoGuardaNada()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10);
            int ventasAntes = CantidadDeVentas();

            Assert.Throws<InvalidOperationException>(() =>
                new VentaNegocio().RegistrarVentaCompleta(Unidades(a1, c1, 1), 999999, "Inventado", 100m, 100m));

            Assert.That(CantidadDeVentas(), Is.EqualTo(ventasAntes));
            Assert.That(StockDe(c1), Is.EqualTo(10));
        }

        [Test]
        public void RegistrarVentaCompleta_ConEntradasInvalidas_LanzaExcepcionYNoGuardaNada()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10);
            int efectivo = IdMetodo("Efectivo");
            int fiado = IdsMetodoPago.Fiado;
            int cliente = NuevoCliente();
            var negocio = new VentaNegocio();
            int ventasAntes = CantidadDeVentas();

            Assert.Throws<ArgumentNullException>(() => negocio.RegistrarVentaCompleta(null, efectivo, "Efectivo", 1m, 1m));
            Assert.Throws<ArgumentException>(() => negocio.RegistrarVentaCompleta(new List<Articulos>(), efectivo, "Efectivo", 0m, 0m));
            Assert.Throws<ArgumentException>(() => negocio.RegistrarVentaCompleta(new List<Articulos> { new Articulos { Id = 0, codigo = "x" } }, efectivo, "Efectivo", 1m, 1m));
            Assert.Throws<ArgumentException>(() => negocio.RegistrarVentaCompleta(Unidades(a1, c1, 1), fiado, "FIADO", 100m, 100m), "fiado sin cliente");
            Assert.Throws<ArgumentException>(() => negocio.RegistrarVentaCompleta(Unidades(a1, c1, 1), efectivo, "Efectivo", 100m, 100m, cliente), "cliente sin fiado");

            Assert.That(CantidadDeVentas(), Is.EqualTo(ventasAntes));
            Assert.That(StockDe(c1), Is.EqualTo(10));
        }

        [Test]
        public void RegistrarVentaCompleta_SinStockSuficiente_GuardaLaVentaPeroNoDejaStockNegativoYAvisa()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 2);

            ResultadoVenta r = new VentaNegocio().RegistrarVentaCompleta(Unidades(a1, c1, 5), IdMetodo("Efectivo"), "Efectivo", 500m, 500m);

            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Ventas WHERE Id = @v", P("@v", r.IdVenta)), Is.EqualTo(1), "la venta se registra igual");
            Assert.That(StockDe(c1), Is.EqualTo(2), "el stock no se toca ni queda negativo");
            Assert.That(r.Advertencias.Count, Is.EqualTo(1));
            Assert.That(Escalar<bool>("SELECT StockDescontado FROM Venta_Por_Producto WHERE IdVenta = @v", P("@v", r.IdVenta)), Is.False);
        }

        [Test]
        public void RegistrarVentaCompleta_DosCajasVendiendoLaUltimaUnidad_NuncaDejanStockNegativo()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 1);
            int efectivo = IdMetodo("Efectivo");

            ResultadoVenta[] resultados = Task.WhenAll(Enumerable.Range(0, 2).Select(_ =>
                Task.Run(() => new VentaNegocio().RegistrarVentaCompleta(Unidades(a1, c1, 1), efectivo, "Efectivo", 100m, 100m)))).Result;

            Assert.That(StockDe(c1), Is.EqualTo(0));
            Assert.That(resultados.Sum(x => x.Advertencias.Count), Is.EqualTo(1), "solo una de las dos ventas se quedó sin stock");
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Venta_Por_Producto WHERE IdArticulo = @a", P("@a", a1)), Is.EqualTo(2));
        }

        // ---------- Fiado ----------

        [Test]
        public void RegistrarVentaCompleta_Fiado_GuardaElClienteYElDetalleDelDeudor()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10);
            int cliente = NuevoCliente();

            ResultadoVenta r = new VentaNegocio().RegistrarVentaCompleta(Unidades(a1, c1, 2), IdsMetodoPago.Fiado, "FIADO", 200m, 200m, cliente);

            Assert.That(Escalar<int>("SELECT IdCliente FROM Ventas WHERE Id = @v", P("@v", r.IdVenta)), Is.EqualTo(cliente));
            Assert.That(Escalar<int>("SELECT id_metodo FROM Ventas WHERE Id = @v", P("@v", r.IdVenta)), Is.EqualTo(IdsMetodoPago.Fiado));
            Assert.That(Escalar<string>("SELECT IdProducto FROM DETALLE_COMPRAS_FIADO WHERE IdVenta = @v", P("@v", r.IdVenta)), Is.EqualTo(c1));
            Assert.That(Escalar<int>("SELECT Cantidad FROM DETALLE_COMPRAS_FIADO WHERE IdVenta = @v", P("@v", r.IdVenta)), Is.EqualTo(2));
            Assert.That(Escalar<decimal>("SELECT PrecioCostoUnitario FROM DETALLE_COMPRAS_FIADO WHERE IdVenta = @v", P("@v", r.IdVenta)), Is.EqualTo(60m));
            Assert.That(StockDe(c1), Is.EqualTo(8));
        }

        // ---------- Eliminación con auditoría ----------

        [Test]
        public void EliminarVentaConAuditoria_BorraDevuelveElStockYDejaConstancia()
        {
            string c1 = CodigoUnico(), c2 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10, nombre: "Producto uno");
            int a2 = NuevoArticulo(c2, 50m, 20m, stock: 5, nombre: "Producto dos");
            var negocio = new VentaNegocio();

            var carrito = Unidades(a1, c1, 3).Concat(Unidades(a2, c2, 1)).ToList();
            ResultadoVenta venta = negocio.RegistrarVentaCompleta(carrito, IdMetodo("Efectivo"), "Efectivo", 350m, 350m);
            Assert.That(StockDe(c1), Is.EqualTo(7));

            ResultadoEliminacion r = negocio.EliminarVentaConAuditoria(venta.IdVenta, "  error de carga  ", "TIENDA\\cajero1", "CAJA-02");

            Assert.That(r.UnidadesRepuestas, Is.EqualTo(4));
            Assert.That(StockDe(c1), Is.EqualTo(10), "volvió el stock");
            Assert.That(StockDe(c2), Is.EqualTo(5));
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Ventas WHERE Id = @v", P("@v", venta.IdVenta)), Is.EqualTo(0));
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Venta_Por_Producto WHERE IdVenta = @v", P("@v", venta.IdVenta)), Is.EqualTo(0));

            // Constancia: quién, cuándo, por qué, y una copia de la venta
            Assert.That(Escalar<string>("SELECT Usuario FROM VENTAS_ELIMINADAS WHERE IdAuditoria = @a", P("@a", r.IdAuditoria)), Is.EqualTo("TIENDA\\cajero1"));
            Assert.That(Escalar<string>("SELECT Equipo FROM VENTAS_ELIMINADAS WHERE IdAuditoria = @a", P("@a", r.IdAuditoria)), Is.EqualTo("CAJA-02"));
            Assert.That(Escalar<string>("SELECT Motivo FROM VENTAS_ELIMINADAS WHERE IdAuditoria = @a", P("@a", r.IdAuditoria)), Is.EqualTo("error de carga"));
            Assert.That(Escalar<int>("SELECT IdVenta FROM VENTAS_ELIMINADAS WHERE IdAuditoria = @a", P("@a", r.IdAuditoria)), Is.EqualTo(venta.IdVenta));
            Assert.That(Escalar<decimal>("SELECT Total FROM VENTAS_ELIMINADAS WHERE IdAuditoria = @a", P("@a", r.IdAuditoria)), Is.EqualTo(350m));
            Assert.That(Escalar<string>("SELECT MetodoPago FROM VENTAS_ELIMINADAS WHERE IdAuditoria = @a", P("@a", r.IdAuditoria)), Is.EqualTo("Efectivo"));
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM VENTAS_ELIMINADAS_DETALLE WHERE IdAuditoria = @a AND StockRepuesto = 1", P("@a", r.IdAuditoria)), Is.EqualTo(2));
            Assert.That(Escalar<DateTime>("SELECT FechaEliminacion FROM VENTAS_ELIMINADAS WHERE IdAuditoria = @a", P("@a", r.IdAuditoria)),
                        Is.EqualTo(DateTime.Now).Within(TimeSpan.FromMinutes(2)));
        }

        [Test]
        public void EliminarVentaConAuditoria_SinUsuarioUsaElDeWindows()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10);
            var negocio = new VentaNegocio();
            ResultadoVenta venta = negocio.RegistrarVentaCompleta(Unidades(a1, c1, 1), IdMetodo("Efectivo"), "Efectivo", 100m, 100m);

            ResultadoEliminacion r = negocio.EliminarVentaConAuditoria(venta.IdVenta, "prueba");

            Assert.That(Escalar<string>("SELECT Usuario FROM VENTAS_ELIMINADAS WHERE IdAuditoria = @a", P("@a", r.IdAuditoria)),
                        Is.EqualTo(Environment.UserDomainName + "\\" + Environment.UserName));
            Assert.That(Escalar<string>("SELECT Equipo FROM VENTAS_ELIMINADAS WHERE IdAuditoria = @a", P("@a", r.IdAuditoria)),
                        Is.EqualTo(Environment.MachineName));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("ab")]
        public void EliminarVentaConAuditoria_SinMotivo_NoBorraNada(string motivo)
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10);
            var negocio = new VentaNegocio();
            ResultadoVenta venta = negocio.RegistrarVentaCompleta(Unidades(a1, c1, 1), IdMetodo("Efectivo"), "Efectivo", 100m, 100m);

            Assert.Throws<ArgumentException>(() => negocio.EliminarVentaConAuditoria(venta.IdVenta, motivo));

            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Ventas WHERE Id = @v", P("@v", venta.IdVenta)), Is.EqualTo(1));
            Assert.That(StockDe(c1), Is.EqualTo(9));
        }

        [Test]
        public void EliminarVentaConAuditoria_VentaInexistente_LanzaExcepcionYNoDejaConstancia()
        {
            int antes = Escalar<int>("SELECT COUNT(*) FROM VENTAS_ELIMINADAS");

            Assert.Throws<InvalidOperationException>(() => new VentaNegocio().EliminarVentaConAuditoria(2000000000, "no existe"));

            Assert.That(Escalar<int>("SELECT COUNT(*) FROM VENTAS_ELIMINADAS"), Is.EqualTo(antes));
        }

        [Test]
        public void EliminarVentaConAuditoria_NoSePuedeEliminarDosVeces()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10);
            var negocio = new VentaNegocio();
            ResultadoVenta venta = negocio.RegistrarVentaCompleta(Unidades(a1, c1, 2), IdMetodo("Efectivo"), "Efectivo", 200m, 200m);

            negocio.EliminarVentaConAuditoria(venta.IdVenta, "primera vez");
            Assert.Throws<InvalidOperationException>(() => negocio.EliminarVentaConAuditoria(venta.IdVenta, "segunda vez"));

            Assert.That(StockDe(c1), Is.EqualTo(10), "el stock se devuelve una sola vez");
        }

        [Test]
        public void EliminarVentaConAuditoria_SiFallaLaConstancia_NoSeBorraNiSeTocaElStock()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10);
            var negocio = new VentaNegocio();
            ResultadoVenta venta = negocio.RegistrarVentaCompleta(Unidades(a1, c1, 3), IdMetodo("Efectivo"), "Efectivo", 300m, 300m);
            int constanciasAntes = Escalar<int>("SELECT COUNT(*) FROM VENTAS_ELIMINADAS");

            // Falla simulada DESPUÉS de haber devuelto el stock de la línea: todo lo anterior tiene que deshacerse.
            Ejecutar("CREATE TRIGGER dbo.trg_prueba_falla ON dbo.VENTAS_ELIMINADAS_DETALLE AFTER INSERT AS BEGIN THROW 50001, 'falla simulada', 1; END");

            Assert.Throws<SqlException>(() => negocio.EliminarVentaConAuditoria(venta.IdVenta, "esto va a fallar"));
            QuitarTriggerDeFalla();

            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Ventas WHERE Id = @v", P("@v", venta.IdVenta)), Is.EqualTo(1), "la venta sigue");
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Venta_Por_Producto WHERE IdVenta = @v", P("@v", venta.IdVenta)), Is.EqualTo(1), "sus líneas siguen");
            Assert.That(StockDe(c1), Is.EqualTo(7), "el stock no se devolvió a medias");
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM VENTAS_ELIMINADAS"), Is.EqualTo(constanciasAntes), "no quedó constancia de algo que no pasó");
        }

        [Test]
        public void EliminarVentaConAuditoria_SiLaVentaNoHabiaDescontadoStock_NoLoInventaAlDevolver()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 2);
            var negocio = new VentaNegocio();
            ResultadoVenta venta = negocio.RegistrarVentaCompleta(Unidades(a1, c1, 5), IdMetodo("Efectivo"), "Efectivo", 500m, 500m);
            Assert.That(StockDe(c1), Is.EqualTo(2));

            ResultadoEliminacion r = negocio.EliminarVentaConAuditoria(venta.IdVenta, "venta sin stock");

            Assert.That(r.UnidadesRepuestas, Is.EqualTo(0));
            Assert.That(StockDe(c1), Is.EqualTo(2), "no se devuelve un stock que nunca se descontó");
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM VENTAS_ELIMINADAS_DETALLE WHERE IdAuditoria = @a AND StockRepuesto = 0", P("@a", r.IdAuditoria)), Is.EqualTo(1));
        }

        [Test]
        public void EliminarVentaConAuditoria_Fiado_QuitaLaDeudaYGuardaElCliente()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10);
            int cliente = NuevoCliente();
            var negocio = new VentaNegocio();
            ResultadoVenta venta = negocio.RegistrarVentaCompleta(Unidades(a1, c1, 2), IdsMetodoPago.Fiado, "FIADO", 200m, 200m, cliente);

            ResultadoEliminacion r = negocio.EliminarVentaConAuditoria(venta.IdVenta, "el cliente no se llevó nada");

            Assert.That(Escalar<int>("SELECT COUNT(*) FROM DETALLE_COMPRAS_FIADO WHERE IdVenta = @v", P("@v", venta.IdVenta)), Is.EqualTo(0));
            Assert.That(Escalar<int>("SELECT COUNT(*) FROM Ventas WHERE IdCliente = @c", P("@c", cliente)), Is.EqualTo(0));
            Assert.That(Escalar<int>("SELECT IdCliente FROM VENTAS_ELIMINADAS WHERE IdAuditoria = @a", P("@a", r.IdAuditoria)), Is.EqualTo(cliente));
            Assert.That(StockDe(c1), Is.EqualTo(10));
        }

        [Test]
        public void ListarVentasEliminadas_MuestraLaVentaBorradaPrimero()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10, nombre: "Galletitas");
            var negocio = new VentaNegocio();
            ResultadoVenta venta = negocio.RegistrarVentaCompleta(Unidades(a1, c1, 2), IdMetodo("Efectivo"), "Efectivo", 200m, 200m);
            negocio.EliminarVentaConAuditoria(venta.IdVenta, "listado de prueba", "TIENDA\\ana", "CAJA-01");

            System.Data.DataTable tabla = negocio.ListarVentasEliminadas(5);

            Assert.That(tabla.Rows.Count, Is.GreaterThan(0));
            System.Data.DataRow primera = tabla.Rows[0];
            Assert.That(primera["IdVenta"], Is.EqualTo(venta.IdVenta));
            Assert.That(primera["Usuario"], Is.EqualTo("TIENDA\\ana"));
            Assert.That(primera["Motivo"], Is.EqualTo("listado de prueba"));
            Assert.That(Convert.ToInt32(primera["UnidadesRepuestas"]), Is.EqualTo(2));
            Assert.That((string)primera["Productos"], Does.Contain("2 x Galletitas"));
        }

        // ---------- Cobros de deudores mezclados con ventas ----------

        [Test]
        public void FiltrarFecha_MarcaLosCobrosDeDeudoresParaQueNoSeBorrenComoVentas()
        {
            string c1 = CodigoUnico();
            int a1 = NuevoArticulo(c1, 100m, 60m, stock: 10);
            int cliente = NuevoCliente();
            var negocio = new VentaNegocio();

            ResultadoVenta venta = negocio.RegistrarVentaCompleta(Unidades(a1, c1, 1), IdMetodo("Efectivo"), "Efectivo", 100m, 100m);
            Ejecutar("INSERT INTO PAGOS_DEUDORES (IdCliente, Fecha, Monto, Observaciones, IdMetodoPago) VALUES (@c, GETDATE(), 4321.09, 'prueba', @m)",
                     P("@c", cliente), P("@m", IdMetodo("Efectivo")));

            List<Ventas> hoy = negocio.FiltrarFecha(DateTime.Today);

            Assert.That(hoy.Single(v => v.Id == venta.IdVenta && !v.EsCobroDeuda).Total, Is.EqualTo(100m));
            Assert.That(hoy.Any(v => v.Total == 4321.09m && v.EsCobroDeuda), Is.True, "el cobro viene marcado");
        }
    }
}
