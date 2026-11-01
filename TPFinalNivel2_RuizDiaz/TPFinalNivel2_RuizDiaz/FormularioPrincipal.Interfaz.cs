using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio_;
using TPFinalNivel2_RuizDiaz.Diseno;

namespace TPFinalNivel2_RuizDiaz
{
    // Aspecto de la pantalla principal: menú lateral fijo, encabezado con datos del día y tablero central.
    // Todo se arma por código con los componentes de la carpeta Diseno.
    public partial class FormularioPrincipal
    {
        private const int AnchoMenu = 276;

        private Label _lblReloj;
        private Label _lblEfectivoEncabezado;
        private Label _lblRecaudado, _lblRecaudadoDetalle;
        private Label _lblEfectivo, _lblEfectivoDetalle;
        private Label _lblDigital, _lblDigitalDetalle;
        private Label _lblDeuda, _lblDeudaDetalle;
        private ItemMenu _primerItem;
        private System.Windows.Forms.Timer _timerReloj;
        private System.Windows.Forms.Timer _timerDatos;
        private bool _actualizando;
        private string _ultimoError;

        private void ConstruirInterfaz()
        {
            SuspendLayout();

            BackColor = Tema.Fondo;
            Font = Tema.Fuente(10f);

            var zonaPrincipal = new Panel { Dock = DockStyle.Fill, BackColor = Tema.Fondo };
            zonaPrincipal.Controls.Add(ConstruirTablero());
            zonaPrincipal.Controls.Add(ConstruirEncabezado());

            Controls.Add(zonaPrincipal);
            Controls.Add(ConstruirBarraLateral());

            // Relojes y actualización automática del tablero
            _timerReloj = new System.Windows.Forms.Timer { Interval = 15000 };
            _timerReloj.Tick += (s, e) => ActualizarReloj();
            _timerDatos = new System.Windows.Forms.Timer { Interval = 60000 };
            _timerDatos.Tick += (s, e) => { var tarea = ActualizarIndicadoresAsync(); };

            Shown += (s, e) =>
            {
                ActualizarReloj();
                _timerReloj.Start();
                _timerDatos.Start();
                if (_primerItem != null)
                    _primerItem.Focus();        // Enter ya abre "Nueva venta"
                var tarea = ActualizarIndicadoresAsync();
            };

            Disposed += (s, e) =>
            {
                _timerReloj.Dispose();
                _timerDatos.Dispose();
            };

            ResumeLayout(true);
        }

        // ================= Menú lateral =================

        private Panel ConstruirBarraLateral()
        {
            var barra = new Panel { Dock = DockStyle.Left, Width = AnchoMenu, BackColor = Tema.MenuFondo };

            var menu = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Tema.MenuFondo,
                ColumnCount = 1,
                AutoScroll = true,
                Padding = new Padding(0, 0, 0, 8)
            };
            menu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int fila = 0;

            AgregarGrupo(menu, "VENTAS", ref fila);
            _primerItem = AgregarItem(menu, "Nueva venta", "\uE7BF", "F1", Keys.F1, AbrirVenta, ref fila);

            AgregarGrupo(menu, "CAJA", ref fila);
            AgregarItem(menu, "Cierre de caja", "\uE8EF", "F2", Keys.F2, AbrirCierreDeCaja, ref fila);
            AgregarItem(menu, "Gastos varios", "\uE8F9", "F3", Keys.F3, AbrirGastosVarios, ref fila);
            AgregarItem(menu, "Métodos de pago", "\uE8C7", "F4", Keys.F4, AbrirMetodosDePago, ref fila);

            AgregarGrupo(menu, "DEUDORES", ref fila);
            AgregarItem(menu, "Fiados", "\uE716", "F5", Keys.F5, AbrirDeudores, ref fila);

            AgregarGrupo(menu, "INVENTARIO", ref fila);
            AgregarItem(menu, "Artículos", "\uE7B8", "F6", Keys.F6, AbrirArticulos, ref fila);
            AgregarItem(menu, "Ingreso de mercadería", "\uE896", "F7", Keys.F7, AbrirIngresoDeMercaderia, ref fila);
            AgregarItem(menu, "Proveedores", "\uE719", "F8", Keys.F8, AbrirProveedores, ref fila);

            AgregarGrupo(menu, "REPORTES", ref fila);
            AgregarItem(menu, "Historial de ventas", "\uE81C", "F9", Keys.F9, AbrirHistorialDeVentas, ref fila);
            AgregarItem(menu, "Ventas por producto", "\uE8EC", null, null, AbrirVentasPorProducto, ref fila);
            AgregarItem(menu, "Total por día", "\uE787", null, null, AbrirTotalPorDia, ref fila);

            _atajos[Keys.Control | Keys.P] = AbrirImpresora;

            menu.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            menu.RowCount = fila + 1;

            barra.Controls.Add(menu);
            barra.Controls.Add(ConstruirMarca());
            barra.Controls.Add(ConstruirPieDelMenu());
            return barra;
        }

        private void AgregarGrupo(TableLayoutPanel menu, string titulo, ref int fila)
        {
            var etiqueta = new Label
            {
                Text = titulo,
                Dock = DockStyle.Fill,
                Height = 28,
                Margin = Padding.Empty,
                Padding = new Padding(26, 0, 0, 3),
                TextAlign = ContentAlignment.BottomLeft,
                Font = Tema.Fuente(8.5f, true),
                ForeColor = Tema.MenuTextoSuave,
                BackColor = Tema.MenuFondo,
                TabStop = false
            };

            menu.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            menu.Controls.Add(etiqueta, 0, fila);
            fila++;
        }

        private ItemMenu AgregarItem(TableLayoutPanel menu, string texto, string icono, string atajo, Keys? tecla,
                                     Action accion, ref int fila)
        {
            var item = new ItemMenu { Text = texto, Icono = icono, Atajo = atajo, Dock = DockStyle.Fill };
            item.Activado += (s, e) => accion();

            if (tecla.HasValue)
                _atajos[tecla.Value] = accion;

            menu.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            menu.Controls.Add(item, 0, fila);
            fila++;
            return item;
        }

        private Panel ConstruirMarca()
        {
            var marca = new Panel { Dock = DockStyle.Top, Height = 76, BackColor = Tema.MenuFondo };

            var logo = new Panel { Location = new Point(24, 18), Size = new Size(40, 40), BackColor = Tema.MenuFondo };
            logo.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath camino = Tema.RectanguloRedondeado(new RectangleF(0, 0, 39, 39), 10))
                using (var pincel = new SolidBrush(Tema.Primario))
                {
                    e.Graphics.FillPath(pincel, camino);
                }
                TextRenderer.DrawText(e.Graphics, "T", Tema.Fuente(16f, true), new Rectangle(0, 0, 40, 40), Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            };

            var nombre = new Label
            {
                Text = "TANO POS",
                Font = Tema.Fuente(15f, true),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(76, 16),
                BackColor = Tema.MenuFondo
            };
            var subtitulo = new Label
            {
                Text = "Punto de venta",
                Font = Tema.Fuente(9f),
                ForeColor = Tema.MenuTextoSuave,
                AutoSize = true,
                Location = new Point(78, 41),
                BackColor = Tema.MenuFondo
            };

            marca.Controls.Add(logo);
            marca.Controls.Add(nombre);
            marca.Controls.Add(subtitulo);
            return marca;
        }

        private Panel ConstruirPieDelMenu()
        {
            var pie = new Panel { Dock = DockStyle.Bottom, Height = 64, BackColor = Tema.MenuFondo };
            pie.Paint += (s, e) =>
            {
                using (var pluma = new Pen(Tema.MenuFondoActivo))
                    e.Graphics.DrawLine(pluma, 16, 0, pie.Width - 16, 0);
            };

            string usuario = Environment.UserName ?? "usuario";
            string inicial = usuario.Length > 0 ? usuario.Substring(0, 1).ToUpperInvariant() : "U";

            var avatar = new Panel { Location = new Point(18, 12), Size = new Size(40, 40), BackColor = Tema.MenuFondo };
            avatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pincel = new SolidBrush(Tema.MenuFondoActivo))
                    e.Graphics.FillEllipse(pincel, 0, 0, 38, 38);
                TextRenderer.DrawText(e.Graphics, inicial, Tema.Fuente(12f, true), new Rectangle(0, 0, 40, 40), Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            };

            var nombre = new Label
            {
                Text = usuario,
                Font = Tema.Fuente(10f, true),
                ForeColor = Color.White,
                AutoSize = false,
                AutoEllipsis = true,
                Size = new Size(AnchoMenu - 18 - 58 - 88, 20),
                Location = new Point(66, 13),
                BackColor = Tema.MenuFondo
            };
            var detalle = new Label
            {
                Text = "Sesión de Windows",
                Font = Tema.Fuente(8.5f),
                ForeColor = Tema.MenuTextoSuave,
                AutoSize = true,
                Location = new Point(66, 34),
                BackColor = Tema.MenuFondo
            };

            var ayuda = new ToolTip();
            var botonImpresora = new ItemMenu { SoloIcono = true, Icono = "\uE749", Text = "Impresora (Ctrl+P)", Size = new Size(36, 36), Location = new Point(AnchoMenu - 16 - 36 - 4 - 36, 14) };
            var botonSalir = new ItemMenu { SoloIcono = true, Icono = "\uE7E8", Text = "Salir", Size = new Size(36, 36), Location = new Point(AnchoMenu - 16 - 36, 14) };
            botonImpresora.Activado += (s, e) => AbrirImpresora();
            botonSalir.Activado += (s, e) => Close();
            ayuda.SetToolTip(botonImpresora, "Impresora (Ctrl+P)");
            ayuda.SetToolTip(botonSalir, "Salir del sistema");
            pie.Disposed += (s, e) => ayuda.Dispose();

            pie.Controls.Add(botonImpresora);
            pie.Controls.Add(botonSalir);
            pie.Controls.Add(avatar);
            pie.Controls.Add(nombre);
            pie.Controls.Add(detalle);
            return pie;
        }

        // ================= Encabezado =================

        private Panel ConstruirEncabezado()
        {
            var encabezado = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.White };
            var lineaInferior = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Tema.Borde };

            var titulo = new Label
            {
                Text = "Inicio",
                Font = Tema.Fuente(18f, true),
                ForeColor = Tema.Texto,
                AutoSize = true,
                Location = new Point(28, 12),
                BackColor = Color.White
            };
            var fecha = new Label
            {
                Text = Capitalizar(DateTime.Now.ToString("dddd d 'de' MMMM 'de' yyyy", CultureInfo.CurrentCulture)),
                Font = Tema.Fuente(10f),
                ForeColor = Tema.TextoSuave,
                AutoSize = true,
                Location = new Point(30, 46),
                BackColor = Color.White
            };

            var chips = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0, 14, 24, 0),
                BackColor = Color.White
            };

            // De derecha a izquierda: usuario, fecha y hora, estado de caja.
            Label etiquetaUsuario;
            chips.Controls.Add(CrearChip("USUARIO", Environment.UserName, 170, out etiquetaUsuario));
            chips.Controls.Add(CrearChip("FECHA Y HORA", "--", 170, out _lblReloj));
            chips.Controls.Add(CrearChip("EFECTIVO NETO HOY", "--", 200, out _lblEfectivoEncabezado));

            encabezado.Controls.Add(chips);
            encabezado.Controls.Add(titulo);
            encabezado.Controls.Add(fecha);
            encabezado.Controls.Add(lineaInferior);
            return encabezado;
        }

        private static TarjetaPanel CrearChip(string titulo, string valor, int ancho, out Label etiquetaValor)
        {
            var chip = new TarjetaPanel
            {
                Size = new Size(ancho, 52),
                Margin = new Padding(12, 0, 0, 0),
                Padding = new Padding(14, 7, 10, 4),
                ColorFondo = Tema.Fondo,
                Radio = 8
            };
            chip.BackColor = Tema.Fondo;

            etiquetaValor = new Label
            {
                Text = valor,
                Font = Tema.Fuente(11.5f, true),
                ForeColor = Tema.Texto,
                Dock = DockStyle.Fill,
                AutoEllipsis = true,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft
            };
            var caption = new Label
            {
                Text = titulo,
                Font = Tema.Fuente(7.5f, true),
                ForeColor = Tema.TextoSuave,
                Dock = DockStyle.Top,
                Height = 16,
                BackColor = Color.Transparent
            };

            chip.Controls.Add(etiquetaValor);
            chip.Controls.Add(caption);
            return chip;
        }

        // ================= Tablero =================

        private Control ConstruirTablero()
        {
            var contenido = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Tema.Fondo,
                Padding = new Padding(28, 24, 12, 8),
                AutoScroll = true
            };

            var rejilla = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Tema.Fondo,
                ColumnCount = 4,
                RowCount = 2
            };
            for (int i = 0; i < 4; i++)
                rejilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            rejilla.RowStyles.Add(new RowStyle(SizeType.Absolute, 164));
            rejilla.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            rejilla.Controls.Add(CrearKpi("Recaudado hoy", out _lblRecaudado, out _lblRecaudadoDetalle), 0, 0);
            rejilla.Controls.Add(CrearKpi("Efectivo neto", out _lblEfectivo, out _lblEfectivoDetalle), 1, 0);
            rejilla.Controls.Add(CrearKpi("Medios digitales", out _lblDigital, out _lblDigitalDetalle), 2, 0);
            rejilla.Controls.Add(CrearKpi("Deuda de clientes", out _lblDeuda, out _lblDeudaDetalle), 3, 0);

            TarjetaPanel accesos = ConstruirAccesosRapidos();
            rejilla.Controls.Add(accesos, 0, 1);
            rejilla.SetColumnSpan(accesos, 2);

            TarjetaPanel atajos = ConstruirAtajos();
            rejilla.Controls.Add(atajos, 2, 1);
            rejilla.SetColumnSpan(atajos, 2);

            contenido.Controls.Add(rejilla);
            return contenido;
        }

        private static TarjetaPanel CrearKpi(string titulo, out Label valor, out Label detalle)
        {
            var tarjeta = new TarjetaPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 16),
                Padding = new Padding(22, 18, 18, 14)
            };

            detalle = new Label
            {
                Text = " ",
                Font = Tema.Fuente(9f),
                ForeColor = Tema.TextoSuave,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.TopLeft
            };
            valor = new Label
            {
                Text = "--",
                Font = Tema.Fuente(22f, true),
                ForeColor = Tema.Texto,
                Dock = DockStyle.Top,
                Height = 50,
                AutoEllipsis = true,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft
            };
            var caption = new Label
            {
                Text = titulo.ToUpperInvariant(),
                Font = Tema.Fuente(9f, true),
                ForeColor = Tema.TextoSuave,
                Dock = DockStyle.Top,
                Height = 24,
                BackColor = Color.Transparent
            };

            tarjeta.Controls.Add(detalle);
            tarjeta.Controls.Add(valor);
            tarjeta.Controls.Add(caption);
            return tarjeta;
        }

        private TarjetaPanel ConstruirAccesosRapidos()
        {
            var tarjeta = new TarjetaPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 16),
                Titulo = "Accesos rápidos"
            };

            var pila = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                BackColor = Color.Transparent
            };
            pila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            AgregarAcceso(pila, "Nueva venta", "F1", VarianteBoton.Primario, AbrirVenta);
            AgregarAcceso(pila, "Cierre de caja", "F2", VarianteBoton.Secundario, AbrirCierreDeCaja);
            AgregarAcceso(pila, "Fiados", "F5", VarianteBoton.Secundario, AbrirDeudores);
            AgregarAcceso(pila, "Ingreso de mercadería", "F7", VarianteBoton.Secundario, AbrirIngresoDeMercaderia);

            tarjeta.Controls.Add(pila);
            return tarjeta;
        }

        private static void AgregarAcceso(TableLayoutPanel pila, string texto, string atajo, VarianteBoton variante, Action accion)
        {
            var boton = new BotonPlano
            {
                Text = texto,
                Atajo = atajo,
                Variante = variante,
                Dock = DockStyle.Fill,
                Height = 50,
                Margin = new Padding(0, 0, 0, 10),
                Font = Tema.Fuente(11f, true)
            };
            boton.Click += (s, e) => accion();

            pila.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pila.Controls.Add(boton, 0, pila.RowStyles.Count - 1);
        }

        private TarjetaPanel ConstruirAtajos()
        {
            var tarjeta = new TarjetaPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 16),
                Titulo = "Teclado"
            };

            var lista = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                BackColor = Color.Transparent
            };
            lista.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112));
            lista.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            AgregarAtajo(lista, "F1 – F9", "Abrir un módulo del menú");
            AgregarAtajo(lista, "↑  ↓  Enter", "Moverse por el menú y abrir");
            AgregarAtajo(lista, "Ctrl+P", "Configurar la impresora");
            AgregarAtajo(lista, "En ventas:", "");
            AgregarAtajo(lista, "Enter", "Agregar artículo (vacío = cobrar)");
            AgregarAtajo(lista, "5 *", "Cantidad antes del código");
            AgregarAtajo(lista, "↑ ↓   Supr", "Elegir y quitar un artículo");
            AgregarAtajo(lista, "←  →", "Cambiar la forma de pago");
            AgregarAtajo(lista, "Ctrl+F   Esc", "Fiados   /   salir de la venta");

            tarjeta.Controls.Add(lista);
            return tarjeta;
        }

        private static void AgregarAtajo(TableLayoutPanel lista, string tecla, string descripcion)
        {
            bool esTitulo = descripcion.Length == 0;

            var etiquetaTecla = new Label
            {
                Text = tecla,
                Font = esTitulo ? Tema.Fuente(9f, true) : Tema.Fuente(9.5f, true),
                ForeColor = esTitulo ? Tema.TextoSuave : Tema.SeleccionTexto,
                AutoSize = false,
                Height = esTitulo ? 30 : 26,
                Dock = DockStyle.Fill,
                TextAlign = esTitulo ? ContentAlignment.BottomLeft : ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };
            var etiquetaDescripcion = new Label
            {
                Text = descripcion,
                Font = Tema.Fuente(9.5f),
                ForeColor = Tema.Texto,
                AutoSize = false,
                Height = 26,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            int fila = lista.RowStyles.Count;
            lista.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            lista.Controls.Add(etiquetaTecla, 0, fila);
            lista.Controls.Add(etiquetaDescripcion, 1, fila);
        }

        // ================= Datos =================

        private void ActualizarReloj()
        {
            if (_lblReloj != null)
                _lblReloj.Text = DateTime.Now.ToString("dd/MM/yyyy  HH:mm", CultureInfo.CurrentCulture);
        }

        private static string Capitalizar(string texto)
        {
            return string.IsNullOrEmpty(texto) ? texto : char.ToUpper(texto[0], CultureInfo.CurrentCulture) + texto.Substring(1);
        }

        // Consulta el cierre de caja del día actual (las mismas cuentas del informe de caja) sin trabar la pantalla.
        private async Task ActualizarIndicadoresAsync()
        {
            if (_actualizando || IsDisposed)
                return;

            _actualizando = true;
            try
            {
                DateTime hoy = DateTime.Today;
                InformeCaja informe = await Task.Run(() =>
                    new Venta_Por_Producto_Negocio().ObtenerResumenInforme(hoy, hoy.AddDays(1).AddSeconds(-1)));

                if (IsDisposed)
                    return;

                decimal digital = informe.DesgloseMetodos.Sum(m => m.Total);

                _lblRecaudado.Text = informe.TotalGeneralRecaudado.ToString("C");
                _lblRecaudadoDetalle.Text = "Neto de pagos a proveedores" + Environment.NewLine + "y gastos varios";

                _lblEfectivo.Text = informe.EfectivoVentas.ToString("C");
                _lblEfectivoDetalle.Text = "Fondo inicial: " + informe.FondoInicialEfectivo.ToString("C") + Environment.NewLine + "Ventas y cobros menos salidas";

                _lblDigital.Text = digital.ToString("C");
                _lblDigitalDetalle.Text = informe.DesgloseMetodos.Count == 0
                    ? "Sin movimientos hoy"
                    : "En " + informe.DesgloseMetodos.Count + " medio(s) de pago" + Environment.NewLine + "Débito, crédito, transferencias";

                _lblDeuda.Text = informe.SaldoHistorico.ToString("C");
                _lblDeudaDetalle.Text = "Fiado hoy: " + informe.VentasFiadasNuevas.ToString("C")
                                        + Environment.NewLine + "Cobrado hoy: " + informe.CobroDeudores.ToString("C");

                _lblEfectivoEncabezado.Text = informe.EfectivoVentas.ToString("C");
                _ultimoError = null;
            }
            catch (Exception ex)
            {
                if (IsDisposed)
                    return;

                MostrarSinDatos();

                // Se registra una sola vez por tipo de falla, para no llenar el log cada minuto si la base está caída.
                if (_ultimoError != ex.Message)
                {
                    _ultimoError = ex.Message;
                    RegistroErrores.Registrar(ex.Message, "Tablero de la pantalla principal");
                }
            }
            finally
            {
                _actualizando = false;
            }
        }

        private void MostrarSinDatos()
        {
            const string sinDato = "--";
            const string aviso = "No se pudo consultar la base";

            _lblRecaudado.Text = sinDato; _lblRecaudadoDetalle.Text = aviso;
            _lblEfectivo.Text = sinDato; _lblEfectivoDetalle.Text = aviso;
            _lblDigital.Text = sinDato; _lblDigitalDetalle.Text = aviso;
            _lblDeuda.Text = sinDato; _lblDeudaDetalle.Text = aviso;
            _lblEfectivoEncabezado.Text = sinDato;
        }
    }
}
