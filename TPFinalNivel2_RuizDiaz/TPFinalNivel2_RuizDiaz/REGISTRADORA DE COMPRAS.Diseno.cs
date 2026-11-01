using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TPFinalNivel2_RuizDiaz.Diseno;

namespace TPFinalNivel2_RuizDiaz
{
    // Aspecto de la pantalla de ventas. Los controles de siempre (codigo_Producto, DgvRegistradora, cbx_metododepago,
    // lbl_precio, Btn_Total, ...) se reubican dentro de tarjetas, sin cambiar sus nombres ni sus eventos:
    // por eso toda la operatoria por teclado (Enter, flechas, Supr, *, Ctrl+F, Esc) funciona exactamente igual.
    public partial class REGISTRADORA_DE_COMPRAS
    {
        private Label _lblCantidadArticulos;
        private Panel _campoCodigo;
        private Panel _campoPago;

        private void AplicarDisenioModerno()
        {
            SuspendLayout();

            // ---- Ventana ----
            AutoScaleMode = AutoScaleMode.None;
            Text = "Nueva venta";
            BackColor = Tema.Fondo;
            Font = Tema.Fuente(10f);
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(1100, 640);
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            // Se sacan los controles de la ventana (sin destruirlos) para ubicarlos de nuevo dentro de tarjetas.
            Controls.Clear();

            Panel cuerpo = new Panel { Dock = DockStyle.Fill, BackColor = Tema.Fondo, Padding = new Padding(24, 18, 24, 12) };
            cuerpo.Controls.Add(ConstruirZonaCentral());

            Controls.Add(cuerpo);
            Controls.Add(ConstruirBarraDeAtajos());
            Controls.Add(ConstruirEncabezadoVenta());

            // ---- Lista de artículos ----
            Tema.EstilizarGrilla(DgvRegistradora, 46);
            DgvRegistradora.DataBindingComplete += (s, e) => AjustarColumnasDeLaVenta();

            // Mensaje de ayuda mientras la venta no tiene artículos (se dibuja sobre la grilla, no la oculta).
            DgvRegistradora.Paint += (s, e) =>
            {
                if (DgvRegistradora.Rows.Count > 0)
                    return;

                var zona = new Rectangle(0, 0, DgvRegistradora.Width, DgvRegistradora.Height);
                TextRenderer.DrawText(e.Graphics, "Todavía no hay artículos en la venta" + Environment.NewLine + "Escaneá o escribí un código y presioná Enter",
                    Tema.Fuente(12f), zona, Tema.TextoSuave,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
            };

            ResumeLayout(true);
        }

        // ================= Encabezado y barra de atajos =================

        private Panel ConstruirEncabezadoVenta()
        {
            var encabezado = new Panel { Dock = DockStyle.Top, Height = 76, BackColor = Color.White };
            var lineaInferior = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Tema.Borde };

            encabezado.Controls.Add(lineaInferior);
            encabezado.Controls.Add(new Label
            {
                Text = "Nueva venta",
                Font = Tema.Fuente(18f, true),
                ForeColor = Tema.Texto,
                AutoSize = true,
                Location = new Point(28, 10),
                BackColor = Color.White
            });
            encabezado.Controls.Add(new Label
            {
                Text = "Escaneá o escribí el código del producto",
                Font = Tema.Fuente(10f),
                ForeColor = Tema.TextoSuave,
                AutoSize = true,
                Location = new Point(30, 44),
                BackColor = Color.White
            });

            // Contador de artículos a la derecha
            var chip = new TarjetaPanel
            {
                Size = new Size(150, 52),
                Padding = new Padding(14, 7, 10, 4),
                ColorFondo = Tema.Fondo,
                Radio = 8,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            chip.BackColor = Tema.Fondo;
            chip.Location = new Point(encabezado.Width - 150 - 28, 12);

            _lblCantidadArticulos = new Label
            {
                Text = "0",
                Font = Tema.Fuente(14f, true),
                ForeColor = Tema.Texto,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft
            };
            chip.Controls.Add(_lblCantidadArticulos);
            chip.Controls.Add(new Label
            {
                Text = "ARTÍCULOS",
                Font = Tema.Fuente(7.5f, true),
                ForeColor = Tema.TextoSuave,
                Dock = DockStyle.Top,
                Height = 16,
                BackColor = Color.Transparent
            });
            encabezado.Controls.Add(chip);
            encabezado.Resize += (s, e) => chip.Location = new Point(encabezado.Width - chip.Width - 28, 12);

            return encabezado;
        }

        private Panel ConstruirBarraDeAtajos()
        {
            var barra = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            var lineaSuperior = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Tema.Borde };

            var flujo = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = false,
                Padding = new Padding(24, 9, 0, 0),
                BackColor = Color.White
            };

            AgregarAtajoABarra(flujo, "Enter", "agregar / cobrar");
            AgregarAtajoABarra(flujo, "↑ ↓", "elegir");
            AgregarAtajoABarra(flujo, "Supr", "quitar");
            AgregarAtajoABarra(flujo, "← →", "forma de pago");
            AgregarAtajoABarra(flujo, "5 *", "cantidad");
            AgregarAtajoABarra(flujo, "Ctrl+F", "fiados");
            AgregarAtajoABarra(flujo, "Esc", "salir");

            barra.Controls.Add(flujo);
            barra.Controls.Add(lineaSuperior);
            return barra;
        }

        private static void AgregarAtajoABarra(FlowLayoutPanel flujo, string tecla, string accion)
        {
            flujo.Controls.Add(new Label
            {
                Text = tecla,
                Font = Tema.Fuente(9f, true),
                ForeColor = Tema.SeleccionTexto,
                BackColor = Tema.Seleccion,
                AutoSize = true,
                Padding = new Padding(6, 2, 6, 2),
                Margin = new Padding(0, 0, 6, 0)
            });
            flujo.Controls.Add(new Label
            {
                Text = accion,
                Font = Tema.Fuente(9.5f),
                ForeColor = Tema.TextoSuave,
                BackColor = Color.White,
                AutoSize = true,
                Padding = new Padding(0, 2, 0, 2),
                Margin = new Padding(0, 0, 22, 0)
            });
        }

        // ================= Zona central =================

        private Control ConstruirZonaCentral()
        {
            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Tema.Fondo,
                ColumnCount = 2,
                RowCount = 1
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 430));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Izquierda: la lista de la venta
            var tarjetaLista = new TarjetaPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 20, 0),
                Titulo = "Artículos de la venta",
                Padding = new Padding(2, 46, 2, 2)
            };
            DgvRegistradora.Dock = DockStyle.Fill;
            DgvRegistradora.Margin = Padding.Empty;
            tarjetaLista.Controls.Add(DgvRegistradora);
            raiz.Controls.Add(tarjetaLista, 0, 0);

            // Derecha: código, forma de pago, total y acciones
            var derecha = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Tema.Fondo,
                ColumnCount = 1,
                RowCount = 4,
                Margin = Padding.Empty
            };
            derecha.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            derecha.RowStyles.Add(new RowStyle(SizeType.Absolute, 118));
            derecha.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
            derecha.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            derecha.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            derecha.Controls.Add(ConstruirTarjetaCodigo(), 0, 0);
            derecha.Controls.Add(ConstruirTarjetaPago(), 0, 1);
            derecha.Controls.Add(ConstruirTarjetaTotal(), 0, 2);
            derecha.Controls.Add(ConstruirBotones(), 0, 3);

            raiz.Controls.Add(derecha, 1, 0);
            return raiz;
        }

        // Campo con borde fino redondeado que se pone azul (y más grueso) cuando tiene el foco del teclado.
        private static Panel CrearCampo(int alto, Func<bool> tieneFoco)
        {
            var campo = new Panel { Dock = DockStyle.Top, Height = alto, BackColor = Color.White };
            campo.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                bool conFoco = tieneFoco();
                using (GraphicsPath camino = Tema.RectanguloRedondeado(new RectangleF(1, 1, campo.Width - 3, campo.Height - 3), 8))
                using (var pluma = new Pen(conFoco ? Tema.Primario : Tema.Borde, conFoco ? 2f : 1f))
                {
                    e.Graphics.DrawPath(pluma, camino);
                }
            };
            campo.Resize += (s, e) => campo.Invalidate();
            return campo;
        }

        private TarjetaPanel ConstruirTarjetaCodigo()
        {
            var tarjeta = new TarjetaPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 12),
                Titulo = "Código del producto"
            };

            _campoCodigo = CrearCampo(46, () => codigo_Producto.Focused);

            codigo_Producto.BorderStyle = BorderStyle.None;
            codigo_Producto.Font = Tema.Fuente(19f);
            codigo_Producto.ForeColor = Tema.Texto;
            codigo_Producto.BackColor = Color.White;
            codigo_Producto.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            codigo_Producto.Location = new Point(14, 6);
            codigo_Producto.Width = 250;
            codigo_Producto.Enter += (s, e) => _campoCodigo.Invalidate();
            codigo_Producto.Leave += (s, e) => _campoCodigo.Invalidate();

            // Aviso de cantidad ("X 5") cuando se usa el asterisco
            lblAvisoCantidad.AutoSize = true;
            lblAvisoCantidad.Font = Tema.Fuente(13f, true);
            lblAvisoCantidad.ForeColor = Tema.Aviso;
            lblAvisoCantidad.BackColor = Tema.AvisoFondo;
            lblAvisoCantidad.Padding = new Padding(8, 2, 8, 2);
            lblAvisoCantidad.Visible = false;

            _campoCodigo.Controls.Add(codigo_Producto);
            _campoCodigo.Controls.Add(lblAvisoCantidad);

            // El aviso queda pegado a la derecha y el campo de texto ocupa el resto.
            Action acomodar = () =>
            {
                lblAvisoCantidad.Location = new Point(_campoCodigo.Width - lblAvisoCantidad.Width - 12,
                                                      (_campoCodigo.Height - lblAvisoCantidad.Height) / 2);
                codigo_Producto.Width = Math.Max(60, _campoCodigo.Width - 28 - (lblAvisoCantidad.Visible ? lblAvisoCantidad.Width + 8 : 0));
            };
            _campoCodigo.Resize += (s, e) => acomodar();
            lblAvisoCantidad.SizeChanged += (s, e) => acomodar();
            lblAvisoCantidad.VisibleChanged += (s, e) => acomodar();

            tarjeta.Controls.Add(_campoCodigo);
            return tarjeta;
        }

        private TarjetaPanel ConstruirTarjetaPago()
        {
            var tarjeta = new TarjetaPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 12),
                Titulo = "Forma de pago"
            };

            _campoPago = CrearCampo(42, () => cbx_metododepago.Focused);

            cbx_metododepago.DropDownStyle = ComboBoxStyle.DropDownList;
            cbx_metododepago.FlatStyle = FlatStyle.Flat;
            cbx_metododepago.Font = Tema.Fuente(13f);
            cbx_metododepago.BackColor = Color.White;
            cbx_metododepago.ForeColor = Tema.Texto;
            cbx_metododepago.Dock = DockStyle.None;
            cbx_metododepago.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            cbx_metododepago.Location = new Point(10, 6);
            cbx_metododepago.Width = 300;
            cbx_metododepago.Enter += (s, e) => _campoPago.Invalidate();
            cbx_metododepago.Leave += (s, e) => _campoPago.Invalidate();
            _campoPago.Controls.Add(cbx_metododepago);
            _campoPago.Resize += (s, e) => cbx_metododepago.Width = Math.Max(60, _campoPago.Width - 20);

            LblRecargo.AutoSize = false;
            LblRecargo.Dock = DockStyle.Top;
            LblRecargo.Height = 24;
            LblRecargo.Font = Tema.Fuente(10f);
            LblRecargo.ForeColor = Tema.TextoSuave;
            LblRecargo.BackColor = Color.Transparent;
            LblRecargo.TextAlign = ContentAlignment.MiddleLeft;
            LblRecargo.Image = null;

            // En un panel con Dock=Top el último agregado queda arriba: el campo arriba, el recargo debajo.
            tarjeta.Controls.Add(LblRecargo);
            tarjeta.Controls.Add(_campoPago);
            return tarjeta;
        }

        private TarjetaPanel ConstruirTarjetaTotal()
        {
            var tarjeta = new TarjetaPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 12),
                Titulo = "Total a cobrar",
                ColorFondo = Tema.MenuFondo,
                ColorBorde = Tema.MenuFondo,
                ColorTitulo = Tema.MenuTextoSuave,
                MinimumSize = new Size(0, 104)
            };

            lbl_precio.AutoSize = false;
            lbl_precio.Dock = DockStyle.Fill;
            lbl_precio.Font = Tema.Fuente(34f, true);
            lbl_precio.ForeColor = Color.White;
            lbl_precio.BackColor = Color.Transparent;
            lbl_precio.TextAlign = ContentAlignment.MiddleRight;
            lbl_precio.Text = 0m.ToString("C");

            tarjeta.Controls.Add(lbl_precio);
            return tarjeta;
        }

        private Control ConstruirBotones()
        {
            var pila = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                BackColor = Tema.Fondo,
                Margin = Padding.Empty
            };
            pila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            EstilizarBoton(BtnFiados, "Fiados", "Ctrl+F", VarianteBoton.Secundario, 40, 8);
            EstilizarBoton(btnEliminarArticulo, "Quitar artículo", "Supr", VarianteBoton.Peligro, 40, 8);
            EstilizarBoton(Btn_Total, "COBRAR", "Enter", VarianteBoton.Exito, 52, 0);
            Btn_Total.Font = Tema.Fuente(14f, true);

            pila.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pila.Controls.Add(BtnFiados, 0, 0);
            pila.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pila.Controls.Add(btnEliminarArticulo, 0, 1);
            pila.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pila.Controls.Add(Btn_Total, 0, 2);
            return pila;
        }

        private static void EstilizarBoton(BotonPlano plano, string texto, string atajo, VarianteBoton variante, int alto, int margenInferior)
        {
            plano.Text = texto;
            plano.Atajo = atajo;
            plano.Variante = variante;
            plano.Image = null;
            plano.Dock = DockStyle.Fill;
            plano.Height = alto;
            plano.Margin = new Padding(0, 0, 0, margenInferior);
            plano.Font = Tema.Fuente(11f, true);
        }

        // ================= La lista =================

        // El DataSource se reasigna en cada artículo agregado o quitado: este ajuste se reaplica cada vez.
        private void AjustarColumnasDeLaVenta()
        {
            if (DgvRegistradora.Columns.Contains("Nombre"))
            {
                DgvRegistradora.Columns["Nombre"].HeaderText = "Artículo";
                DgvRegistradora.Columns["Nombre"].FillWeight = 46;
                DgvRegistradora.Columns["Nombre"].DefaultCellStyle.Font = Tema.Fuente(12f, true);
            }

            if (DgvRegistradora.Columns.Contains("Descripcion"))
            {
                DgvRegistradora.Columns["Descripcion"].HeaderText = "Descripción";
                DgvRegistradora.Columns["Descripcion"].FillWeight = 38;
                DgvRegistradora.Columns["Descripcion"].DefaultCellStyle.ForeColor = Tema.TextoSuave;
            }

            if (DgvRegistradora.Columns.Contains("Precio"))
            {
                DataGridViewColumn precio = DgvRegistradora.Columns["Precio"];
                precio.HeaderText = "Precio";
                precio.FillWeight = 16;
                precio.DefaultCellStyle.Format = "N2";   // antes, al quitar un artículo el precio volvía a mostrarse con 4 decimales
                precio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                precio.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                precio.DefaultCellStyle.Font = Tema.Fuente(12f, true);
            }

            if (_lblCantidadArticulos != null)
                _lblCantidadArticulos.Text = listaArticulos.Count.ToString();
        }
    }
}
