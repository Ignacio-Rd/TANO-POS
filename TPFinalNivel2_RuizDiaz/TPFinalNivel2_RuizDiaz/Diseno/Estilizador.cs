using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TPFinalNivel2_RuizDiaz.Diseno
{
    // Lleva cualquier formulario existente al estilo moderno SIN mover ni renombrar ningún control:
    // cambia colores, tipografía, botones y grillas. Como no toca posiciones, eventos ni datos, es seguro
    // aplicarlo a pantallas con lógica delicada. Se usa con una línea en el constructor:
    //     InitializeComponent();
    //     Diseno.Estilizador.Aplicar(this);
    //
    // Los colores que el código asigna después en tiempo de ejecución (rojo/verde de deudas, campos con error, etc.)
    // siguen funcionando, porque esto se aplica una sola vez al crear la pantalla.
    internal static class Estilizador
    {
        private static readonly ConditionalWeakTable<Button, object> _variantes = new ConditionalWeakTable<Button, object>();

        public static void Aplicar(Form formulario)
        {
            formulario.SuspendLayout();

            // Sin autoescalado por fuente: cambiar la tipografía no debe agrandar la ventana ni mover controles.
            formulario.AutoScaleMode = AutoScaleMode.None;
            formulario.BackColor = Tema.Fondo;
            formulario.Font = Tema.Fuente(9f);

            foreach (Control hijo in formulario.Controls)
                Recorrer(hijo);

            formulario.ResumeLayout(true);
        }

        private static void Recorrer(Control control)
        {
            var boton = control as Button;
            var etiqueta = control as Label;

            if (boton != null)
            {
                EstilizarBoton(boton);
            }
            else if (control is DataGridView)
            {
                Tema.EstilizarGrilla((DataGridView)control, 34, true);
            }
            else if (etiqueta != null)
            {
                int anchoOriginal = etiqueta.Width;
                AdaptarFuente(etiqueta);
                etiqueta.ForeColor = ColorDeTexto(etiqueta.ForeColor);
                if (etiqueta.BackColor != Color.Transparent)
                    etiqueta.BackColor = Color.Transparent;
                AjustarAlAnchoOriginal(etiqueta, anchoOriginal);
            }
            else if (control is GroupBox)
            {
                control.BackColor = Tema.Fondo;
                control.ForeColor = Tema.Texto;
                control.Font = Tema.Fuente(9.5f, true);
            }
            else if (control is DateTimePicker)
            {
                AdaptarFuente(control);
                AjustarAnchoDeFecha((DateTimePicker)control);
            }
            else if (control is TextBox || control is ComboBox || control is NumericUpDown ||
                     control is MaskedTextBox || control is ListBox)
            {
                AdaptarFuente(control);
            }
            else if (control is RadioButton || control is CheckBox)
            {
                AdaptarFuente(control);
                control.ForeColor = ColorDeTexto(control.ForeColor);
                control.BackColor = Color.Transparent;
            }
            else if (control is PrintPreviewControl)
            {
                control.BackColor = Tema.Hex("#CBD5E1");   // fondo de la vista previa de las hojas
            }
            else if (control is Panel || control is TabPage)
            {
                if (control.BackColor != Color.Transparent)
                    control.BackColor = Tema.Fondo;
            }

            foreach (Control hijo in control.Controls)
                Recorrer(hijo);
        }

        // ---------- Tipografía y color ----------

        // Pasa las fuentes viejas (Bebas Neue, Microsoft Sans Serif, Arial Narrow...) a Segoe UI.
        // Solo toca las que no son Segoe UI, así que aplicarlo dos veces no cambia nada.
        private static void AdaptarFuente(Control control)
        {
            Font actual = control.Font;
            string familia = actual.FontFamily.Name;

            if (familia.StartsWith("Segoe UI", StringComparison.OrdinalIgnoreCase) ||
                familia.Equals("Consolas", StringComparison.OrdinalIgnoreCase))
                return;

            float puntos = actual.SizeInPoints;
            bool negrita = (actual.Style & FontStyle.Bold) == FontStyle.Bold;

            if (familia.Equals("Bebas Neue", StringComparison.OrdinalIgnoreCase) ||
                familia.Equals("Bahnschrift Condensed", StringComparison.OrdinalIgnoreCase))
            {
                puntos *= 0.62f;         // son tipografías angostas: a igual tamaño ocupan menos
                negrita = true;
            }
            else if (familia.Equals("Arial Narrow", StringComparison.OrdinalIgnoreCase))
            {
                puntos *= 0.85f;
            }
            else if (puntos < 11f)
            {
                puntos += 0.5f;          // Segoe UI se ve algo más chica que Microsoft Sans Serif
            }

            control.Font = Tema.Fuente((float)Math.Round(puntos * 2) / 2f, negrita);
        }

        // Si la tipografía nueva hace que un texto ocupe más que antes, se achica de a medio punto hasta que entre
        // en el espacio original: así nada se superpone con los controles vecinos ni se corta.
        private static void AjustarAlAnchoOriginal(Label etiqueta, int anchoOriginal)
        {
            if (string.IsNullOrEmpty(etiqueta.Text) || etiqueta.Text.Contains("\n") || anchoOriginal < 12)
                return;

            float puntos = etiqueta.Font.SizeInPoints;
            bool negrita = (etiqueta.Font.Style & FontStyle.Bold) == FontStyle.Bold;
            bool semibold = etiqueta.Font.FontFamily.Name.IndexOf("Semibold", StringComparison.OrdinalIgnoreCase) >= 0;

            while (puntos > 8f && AnchoDeTexto(etiqueta.Text, etiqueta.Font) > anchoOriginal)
            {
                puntos -= 0.5f;
                etiqueta.Font = Tema.Fuente(puntos, semibold || negrita);
            }
        }

        private static int AnchoDeTexto(string texto, Font fuente)
        {
            return TextRenderer.MeasureText(texto, fuente, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width;
        }

        private static int AltoDeTexto(string texto, Font fuente)
        {
            return TextRenderer.MeasureText(texto, fuente, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Height;
        }

        // Las fechas en formato largo ("domingo, 1 de noviembre de 2026") necesitan ancho de sobra: se ensancha si hace falta.
        private static void AjustarAnchoDeFecha(DateTimePicker selector)
        {
            string texto;
            switch (selector.Format)
            {
                case DateTimePickerFormat.Long: texto = selector.Value.ToLongDateString(); break;
                case DateTimePickerFormat.Short: texto = selector.Value.ToShortDateString(); break;
                case DateTimePickerFormat.Time: texto = selector.Value.ToLongTimeString(); break;
                default: texto = selector.CustomFormat == null ? "" : selector.Value.ToString(selector.CustomFormat); break;
            }

            if (texto.Length == 0)
                return;

            int necesario = AnchoDeTexto(texto, selector.Font) + 36;
            if (necesario > selector.Width)
                selector.Width = necesario;
        }

        // Colores neutros (negro, gris, blanco, colores del sistema) pasan al color de texto del tema.
        // Los colores con significado se llevan a la paleta: rojo = peligro/deuda, verde = a favor, ámbar = aviso.
        private static Color ColorDeTexto(Color color)
        {
            if (color.IsEmpty || color == Color.Transparent || color.IsSystemColor)
                return Tema.Texto;

            bool neutro = Math.Abs(color.R - color.G) < 24 && Math.Abs(color.G - color.B) < 24;
            if (neutro)
                return Tema.Texto;

            float tono = color.GetHue();
            if (tono >= 340f || tono < 20f) return Tema.Peligro;
            if (tono < 70f) return Tema.Aviso;
            if (tono < 170f) return Tema.Exito;
            return Tema.Primario;
        }

        // ---------- Botones ----------

        private static VarianteBoton ElegirVariante(string texto)
        {
            string t = (texto ?? "").Trim().ToUpperInvariant();

            string[] peligro = { "ELIMIN", "BORRAR", "QUITAR", "ANULAR" };
            string[] exito = { "FINALIZAR", "COBRAR", "CONFIRMAR", "GUARDAR", "AGREGAR", "ACEPTAR", "REGISTRAR", "PAGAR", "APLICAR", "MODIFICAR" };
            string[] primario = { "FILTRAR", "BUSCAR", "INFORME", "VER", "IMPRIMIR", "GENERAR", "CARGAR", "TICKET", "CONSULTAR", "EXPORTAR", "ABRIR", "AUMENTAR" };

            foreach (string p in peligro) if (t.Contains(p)) return VarianteBoton.Peligro;
            foreach (string p in exito) if (t.Contains(p)) return VarianteBoton.Exito;
            foreach (string p in primario) if (t.Contains(p)) return VarianteBoton.Primario;
            return VarianteBoton.Secundario;
        }

        private static void ColoresDeVariante(VarianteBoton v, out Color fondo, out Color hover, out Color texto)
        {
            switch (v)
            {
                case VarianteBoton.Exito: fondo = Tema.Exito; hover = Tema.ExitoHover; texto = Color.White; break;
                case VarianteBoton.Peligro: fondo = Tema.Peligro; hover = Tema.PeligroHover; texto = Color.White; break;
                case VarianteBoton.Primario: fondo = Tema.Primario; hover = Tema.PrimarioHover; texto = Color.White; break;
                default: fondo = Color.White; hover = Tema.EncabezadoGrilla; texto = Tema.Texto; break;
            }
        }

        private static void EstilizarBoton(Button boton)
        {
            // Los BotonPlano ya se dibujan solos; los botones que solo tienen ícono (sin texto) se dejan como están.
            if (boton is BotonPlano || string.IsNullOrWhiteSpace(boton.Text))
                return;

            VarianteBoton variante = ElegirVariante(boton.Text);
            Color fondo, hover, texto;
            ColoresDeVariante(variante, out fondo, out hover, out texto);

            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.FlatAppearance.MouseOverBackColor = hover;
            boton.FlatAppearance.MouseDownBackColor = hover;
            boton.UseVisualStyleBackColor = false;
            boton.BackColor = fondo;
            boton.ForeColor = texto;
            boton.Cursor = Cursors.Hand;
            boton.Image = null;                    // los íconos viejos no combinan con el estilo nuevo
            boton.TextAlign = ContentAlignment.MiddleCenter;

            // Tipografía del botón: la más grande (hasta 12 pt) cuyo texto entre cómodo en el botón.
            float puntos = Math.Max(9f, Math.Min(boton.Font.SizeInPoints, 12f));
            Font fuente = Tema.Fuente(puntos, true);
            while (puntos > 7.5f &&
                   (AnchoDeTexto(boton.Text, fuente) > boton.Width - 14 || AltoDeTexto(boton.Text, fuente) > boton.Height - 6))
            {
                puntos -= 0.5f;
                fuente = Tema.Fuente(puntos, true);
            }
            boton.Font = fuente;

            object marca;
            if (!_variantes.TryGetValue(boton, out marca))
            {
                _variantes.Add(boton, variante);
                boton.Paint += PintarBordeRedondeado;
            }
        }

        // Esquinas redondeadas y anillo de foco, dibujados sobre el botón plano (el botón sigue siendo un Button común).
        private static void PintarBordeRedondeado(object sender, PaintEventArgs e)
        {
            var boton = (Button)sender;
            object marca;
            _variantes.TryGetValue(boton, out marca);
            VarianteBoton variante = marca is VarianteBoton ? (VarianteBoton)marca : VarianteBoton.Secundario;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new RectangleF(0, 0, boton.Width, boton.Height);
            var interior = new RectangleF(0.5f, 0.5f, boton.Width - 1.5f, boton.Height - 1.5f);

            using (var esquinas = new GraphicsPath(FillMode.Alternate))
            using (GraphicsPath redondeado = Tema.RectanguloRedondeado(interior, 7))
            using (var fondoPadre = new SolidBrush(Tema.FondoEfectivo(boton.Parent)))
            {
                esquinas.AddRectangle(rect);
                esquinas.AddPath(redondeado, false);
                g.FillPath(fondoPadre, esquinas);

                if (variante == VarianteBoton.Secundario)
                {
                    using (var pluma = new Pen(Tema.Borde, 1f))
                        g.DrawPath(pluma, redondeado);
                }

                if (boton.Focused && boton.Enabled)
                {
                    using (GraphicsPath anillo = Tema.RectanguloRedondeado(new RectangleF(1f, 1f, boton.Width - 3f, boton.Height - 3f), 7))
                    using (var pluma = new Pen(Tema.Hex("#93C5FD"), 2.5f))
                        g.DrawPath(pluma, anillo);
                }
            }
        }
    }
}
