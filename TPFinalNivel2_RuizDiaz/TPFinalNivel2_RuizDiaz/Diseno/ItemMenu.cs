using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TPFinalNivel2_RuizDiaz.Diseno
{
    // Ítem del menú lateral: ícono + texto + tecla rápida.
    // Teclado: ↑/↓ mueven el foco entre ítems, Enter o Espacio lo abren. El foco se ve con un anillo.
    internal class ItemMenu : Control
    {
        private bool _hover;
        private bool _seleccionado;

        public ItemMenu()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.Selectable | ControlStyles.StandardClick, true);

            TabStop = true;
            Cursor = Cursors.Hand;
            Height = 36;
            Margin = Padding.Empty;
            Font = Tema.Fuente(10.5f);
        }

        // Carácter de Segoe MDL2 Assets (por ejemplo "" = carrito).
        public string Icono { get; set; }

        // Etiqueta de la tecla rápida a la derecha ("F1", "Ctrl+P").
        public string Atajo { get; set; }

        // Botón cuadrado con solo el ícono (para el pie del menú). El texto sirve como ayuda al pasar el mouse.
        public bool SoloIcono { get; set; }

        [DefaultValue(false)]
        public bool Seleccionado
        {
            get { return _seleccionado; }
            set { _seleccionado = value; Invalidate(); }
        }

        public event EventHandler Activado;

        public void Activar()
        {
            var manejador = Activado;
            if (manejador != null)
                manejador(this, EventArgs.Empty);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down)
                return true;
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                Activar();
            }
            else if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) && Parent != null)
            {
                Parent.SelectNextControl(this, e.KeyCode == Keys.Down, true, false, true);
                e.Handled = true;
            }

            base.OnKeyDown(e);
        }

        protected override void OnClick(EventArgs e)
        {
            Focus();
            base.OnClick(e);
            Activar();
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

        private void PintarSoloIcono(Graphics g, bool resaltado, bool conFoco)
        {
            var area = new RectangleF(1, 1, Width - 3, Height - 3);
            if (resaltado)
            {
                using (GraphicsPath camino = Tema.RectanguloRedondeado(area, 8))
                using (var pincel = new SolidBrush(Tema.MenuFondoActivo))
                {
                    g.FillPath(pincel, camino);
                }
            }

            if (conFoco)
            {
                using (GraphicsPath camino = Tema.RectanguloRedondeado(area, 8))
                using (var pluma = new Pen(Tema.MenuAcento, 1.5f))
                {
                    g.DrawPath(pluma, camino);
                }
            }

            Color color = resaltado ? Color.White : Tema.MenuTexto;
            if (!string.IsNullOrEmpty(Icono) && Tema.IconosDisponibles)
            {
                TextRenderer.DrawText(g, Icono, Tema.FuenteIconos(13f), new Rectangle(0, 0, Width, Height), color,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
            else
            {
                using (var pincel = new SolidBrush(color))
                    g.FillEllipse(pincel, Width / 2 - 3, Height / 2 - 3, 6, 6);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            bool conFoco = Focused;
            bool resaltado = _hover || conFoco || _seleccionado;

            g.Clear(Tema.MenuFondo);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (SoloIcono)
            {
                PintarSoloIcono(g, resaltado, conFoco);
                return;
            }

            var area = new RectangleF(10, 2, Width - 20, Height - 4);
            if (resaltado)
            {
                Color fondo = (_seleccionado || conFoco) ? Tema.MenuFondoActivo : Tema.MenuFondoHover;
                using (GraphicsPath camino = Tema.RectanguloRedondeado(area, 8))
                using (var pincel = new SolidBrush(fondo))
                {
                    g.FillPath(pincel, camino);
                }
            }

            if (conFoco)
            {
                using (GraphicsPath camino = Tema.RectanguloRedondeado(new RectangleF(10.75f, 2.75f, Width - 21.5f, Height - 5.5f), 8))
                using (var pluma = new Pen(Tema.MenuAcento, 1.5f))
                {
                    g.DrawPath(pluma, camino);
                }
            }

            Color colorTexto = resaltado ? Color.White : Tema.MenuTexto;
            Color colorIcono = (_seleccionado || conFoco) ? Tema.MenuAcento : (resaltado ? Color.White : Tema.MenuTextoSuave);

            // Ícono (o un punto si la fuente de íconos no está instalada)
            var rectIcono = new Rectangle(20, 0, 28, Height);
            if (!string.IsNullOrEmpty(Icono) && Tema.IconosDisponibles)
            {
                TextRenderer.DrawText(g, Icono, Tema.FuenteIconos(13f), rectIcono, colorIcono,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
            else
            {
                using (var pincel = new SolidBrush(colorIcono))
                {
                    g.FillEllipse(pincel, rectIcono.X + 10, Height / 2 - 3, 6, 6);
                }
            }

            // Tecla rápida
            int derecha = Width - 22;
            if (!string.IsNullOrEmpty(Atajo))
            {
                Font fuenteAtajo = Tema.Fuente(8.5f, true);
                Size medida = TextRenderer.MeasureText(g, Atajo, fuenteAtajo, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
                var rectAtajo = new Rectangle(Width - 22 - medida.Width - 12, (Height - medida.Height - 6) / 2, medida.Width + 12, medida.Height + 6);

                using (GraphicsPath camino = Tema.RectanguloRedondeado(rectAtajo, 5))
                using (var pluma = new Pen(Tema.Hex("#334155"), 1f))
                {
                    g.DrawPath(pluma, camino);
                }

                TextRenderer.DrawText(g, Atajo, fuenteAtajo, rectAtajo, Tema.MenuTextoSuave,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

                derecha = rectAtajo.Left - 8;
            }

            // Texto
            var rectTexto = new Rectangle(56, 0, Math.Max(10, derecha - 56), Height);
            TextRenderer.DrawText(g, Text, _seleccionado ? Tema.Fuente(10.5f, true) : Font, rectTexto, colorTexto,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }
    }
}
