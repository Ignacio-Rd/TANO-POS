using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TPFinalNivel2_RuizDiaz.Diseno
{
    internal enum VarianteBoton
    {
        Primario,    // azul corporativo: acción principal
        Exito,       // verde sobrio: cobrar
        Peligro,     // rojo: quitar / cancelar
        Secundario   // blanco con borde fino: acciones de apoyo
    }

    // Botón "flat" de esquinas redondeadas, con estados (hover, presionado, deshabilitado)
    // y un anillo de foco bien visible para quien navega con el teclado.
    // Sigue siendo un Button: PerformClick(), Click, TabStop, etc. funcionan igual.
    internal class BotonPlano : Button
    {
        private bool _hover;
        private bool _presionado;
        private VarianteBoton _variante = VarianteBoton.Primario;
        private string _atajo;

        public BotonPlano()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            Cursor = Cursors.Hand;
            Font = Tema.Fuente(11f, true);
            Height = 44;
            Radio = 8;
        }

        [DefaultValue(8f)]
        public float Radio { get; set; }

        [DefaultValue(VarianteBoton.Primario)]
        public VarianteBoton Variante
        {
            get { return _variante; }
            set { _variante = value; Invalidate(); }
        }

        // Etiqueta de la tecla rápida que se dibuja a la derecha ("Enter", "Supr", "Ctrl+F").
        public string Atajo
        {
            get { return _atajo; }
            set { _atajo = value; Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _presionado = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs mevent) { _presionado = true; Invalidate(); base.OnMouseDown(mevent); }
        protected override void OnMouseUp(MouseEventArgs mevent) { _presionado = false; Invalidate(); base.OnMouseUp(mevent); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
        protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }

        private void Colores(out Color fondo, out Color texto, out Color borde, out Color chip)
        {
            bool vivo = Enabled;
            bool resalta = _hover || _presionado;

            switch (_variante)
            {
                case VarianteBoton.Exito:
                    fondo = resalta ? Tema.ExitoHover : Tema.Exito;
                    texto = Color.White; borde = fondo; chip = Color.FromArgb(48, 255, 255, 255);
                    break;
                case VarianteBoton.Peligro:
                    fondo = resalta ? Tema.PeligroHover : Tema.Peligro;
                    texto = Color.White; borde = fondo; chip = Color.FromArgb(48, 255, 255, 255);
                    break;
                case VarianteBoton.Secundario:
                    fondo = resalta ? Tema.EncabezadoGrilla : Color.White;
                    texto = Tema.Texto; borde = Tema.Borde; chip = Tema.EncabezadoGrilla;
                    break;
                default:
                    fondo = resalta ? Tema.PrimarioHover : Tema.Primario;
                    texto = Color.White; borde = fondo; chip = Color.FromArgb(48, 255, 255, 255);
                    break;
            }

            if (!vivo)
            {
                fondo = Tema.Borde; texto = Tema.MenuTextoSuave; borde = Tema.Borde; chip = Color.FromArgb(40, 255, 255, 255);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Tema.FondoEfectivo(Parent));
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color fondo, texto, borde, chip;
            Colores(out fondo, out texto, out borde, out chip);

            var rect = new RectangleF(1.5f, 1.5f, Width - 3.5f, Height - 3.5f);
            using (GraphicsPath camino = Tema.RectanguloRedondeado(rect, Radio))
            using (var relleno = new SolidBrush(fondo))
            using (var pluma = new Pen(borde, 1f))
            {
                g.FillPath(relleno, camino);
                g.DrawPath(pluma, camino);
            }

            // Anillo de foco: visible solo cuando el botón tiene el foco del teclado.
            if (Focused && Enabled)
            {
                var anillo = new RectangleF(0.75f, 0.75f, Width - 2f, Height - 2f);
                using (GraphicsPath camino = Tema.RectanguloRedondeado(anillo, Radio + 1))
                using (var pluma = new Pen(Tema.Hex("#93C5FD"), 2.5f))
                {
                    g.DrawPath(pluma, camino);
                }
            }

            int margenDerecho = 12;
            Rectangle zonaTexto = new Rectangle(12, 0, Width - 24, Height);

            if (!string.IsNullOrEmpty(_atajo))
            {
                Font fuenteChip = Tema.Fuente(8.5f, true);
                Size medida = TextRenderer.MeasureText(g, _atajo, fuenteChip, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
                int ancho = medida.Width + 14;
                int alto = medida.Height + 6;
                var rectChip = new Rectangle(Width - margenDerecho - ancho, (Height - alto) / 2, ancho, alto);

                using (GraphicsPath camino = Tema.RectanguloRedondeado(rectChip, 5))
                using (var pincel = new SolidBrush(chip))
                {
                    g.FillPath(pincel, camino);
                }

                TextRenderer.DrawText(g, _atajo, fuenteChip, rectChip, texto,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

                zonaTexto = new Rectangle(12, 0, rectChip.Left - 20, Height);
            }

            TextRenderer.DrawText(g, Text, Font, zonaTexto, texto,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }
    }
}
