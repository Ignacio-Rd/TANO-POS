using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TPFinalNivel2_RuizDiaz.Diseno
{
    // Panel con apariencia de tarjeta: esquinas redondeadas, borde fino y título opcional.
    // Los controles que se agregan adentro quedan separados del borde por el Padding (16 px).
    internal class TarjetaPanel : Panel
    {
        private string _titulo;
        private Color _colorFondo = Tema.Tarjeta;
        private Color _colorBorde = Tema.Borde;
        private Color _colorTitulo = Tema.TextoSuave;

        public TarjetaPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            BackColor = Tema.Tarjeta;
            Padding = new Padding(16);
            Radio = 10;
        }

        [DefaultValue(10f)]
        public float Radio { get; set; }

        // Texto chico en mayúsculas arriba a la izquierda ("ARTÍCULOS DE LA VENTA"). Reserva el espacio necesario.
        public string Titulo
        {
            get { return _titulo; }
            set
            {
                _titulo = value;
                Padding = new Padding(16, string.IsNullOrEmpty(value) ? 16 : 46, 16, 16);
                Invalidate();
            }
        }

        public Color ColorFondo
        {
            get { return _colorFondo; }
            set { _colorFondo = value; BackColor = value; Invalidate(); }
        }

        public Color ColorBorde
        {
            get { return _colorBorde; }
            set { _colorBorde = value; Invalidate(); }
        }

        public Color ColorTitulo
        {
            get { return _colorTitulo; }
            set { _colorTitulo = value; Invalidate(); }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Lo que queda fuera de las esquinas redondeadas se pinta con el color del contenedor.
            Color externo = Tema.FondoEfectivo(Parent);
            e.Graphics.Clear(externo);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            using (GraphicsPath camino = Tema.RectanguloRedondeado(rect, Radio))
            using (var relleno = new SolidBrush(_colorFondo))
            using (var borde = new Pen(_colorBorde, 1f))
            {
                e.Graphics.FillPath(relleno, camino);
                e.Graphics.DrawPath(borde, camino);
            }

            if (!string.IsNullOrEmpty(_titulo))
            {
                TextRenderer.DrawText(e.Graphics, _titulo.ToUpperInvariant(), Tema.Fuente(9f, true),
                    new Point(16, 14), _colorTitulo, TextFormatFlags.NoPadding | TextFormatFlags.Left);
            }
        }
    }
}
