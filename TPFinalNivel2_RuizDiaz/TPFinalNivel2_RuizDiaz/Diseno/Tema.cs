using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms;

namespace TPFinalNivel2_RuizDiaz.Diseno
{
    // Paleta, tipografía y utilidades de dibujo compartidas por toda la interfaz.
    // Estética sobria tipo ERP / Fluent / Stripe: menú azul marino, fondo gris claro, acento azul corporativo.
    internal static class Tema
    {
        // ---------- Paleta ----------
        public static readonly Color MenuFondo = Hex("#0F172A");        // slate-900
        public static readonly Color MenuFondoActivo = Hex("#1E293B");  // slate-800
        public static readonly Color MenuFondoHover = Hex("#162033");
        public static readonly Color MenuTexto = Hex("#CBD5E1");        // slate-300
        public static readonly Color MenuTextoSuave = Hex("#64748B");   // slate-500
        public static readonly Color MenuAcento = Hex("#60A5FA");       // blue-400

        public static readonly Color Fondo = Hex("#F8FAFC");            // slate-50
        public static readonly Color Tarjeta = Color.White;
        public static readonly Color Borde = Hex("#E2E8F0");            // slate-200
        public static readonly Color Texto = Hex("#0F172A");
        public static readonly Color TextoSuave = Hex("#64748B");

        public static readonly Color Primario = Hex("#2563EB");         // azul corporativo
        public static readonly Color PrimarioHover = Hex("#1D4ED8");
        public static readonly Color Exito = Hex("#16A34A");            // verde sobrio (cobrar)
        public static readonly Color ExitoHover = Hex("#15803D");
        public static readonly Color Peligro = Hex("#DC2626");          // rojo (cancelar / quitar)
        public static readonly Color PeligroHover = Hex("#B91C1C");
        public static readonly Color Aviso = Hex("#B45309");
        public static readonly Color AvisoFondo = Hex("#FEF3C7");

        public static readonly Color EncabezadoGrilla = Hex("#F1F5F9");
        public static readonly Color FilaAlterna = Hex("#F8FAFC");
        public static readonly Color Seleccion = Hex("#DBEAFE");        // blue-100
        public static readonly Color SeleccionTexto = Hex("#1E3A8A");   // blue-900

        public static Color Hex(string html)
        {
            return ColorTranslator.FromHtml(html);
        }

        // ---------- Tipografía ----------
        public const string FamiliaBase = "Segoe UI";
        public const string FamiliaSemibold = "Segoe UI Semibold";
        public const string FamiliaIconos = "Segoe MDL2 Assets";

        private static readonly Dictionary<string, Font> _fuentes = new Dictionary<string, Font>();

        public static Font Fuente(float puntos, bool semibold = false, FontStyle estilo = FontStyle.Regular)
        {
            string familia = semibold ? FamiliaSemibold : FamiliaBase;
            string clave = familia + "|" + puntos + "|" + (int)estilo;

            lock (_fuentes)
            {
                Font fuente;
                if (!_fuentes.TryGetValue(clave, out fuente))
                {
                    fuente = new Font(familia, puntos, estilo, GraphicsUnit.Point);
                    _fuentes[clave] = fuente;
                }
                return fuente;
            }
        }

        public static Font FuenteIconos(float puntos)
        {
            string clave = "icono|" + puntos;
            lock (_fuentes)
            {
                Font fuente;
                if (!_fuentes.TryGetValue(clave, out fuente))
                {
                    fuente = new Font(FamiliaIconos, puntos, FontStyle.Regular, GraphicsUnit.Point);
                    _fuentes[clave] = fuente;
                }
                return fuente;
            }
        }

        private static bool? _iconosDisponibles;

        // Segoe MDL2 Assets viene con Windows 10 y 11. Si no está (Windows 7/8), el menú muestra un punto en vez de un ícono roto.
        public static bool IconosDisponibles
        {
            get
            {
                if (!_iconosDisponibles.HasValue)
                {
                    try
                    {
                        using (var instaladas = new InstalledFontCollection())
                        {
                            bool hay = false;
                            foreach (FontFamily familia in instaladas.Families)
                            {
                                if (string.Equals(familia.Name, FamiliaIconos, StringComparison.OrdinalIgnoreCase))
                                {
                                    hay = true;
                                    break;
                                }
                            }
                            _iconosDisponibles = hay;
                        }
                    }
                    catch (Exception)
                    {
                        _iconosDisponibles = false;
                    }
                }
                return _iconosDisponibles.Value;
            }
        }

        // ---------- Dibujo ----------
        // Primer color opaco hacia arriba en la jerarquía (un Panel transparente no tiene color propio).
        public static Color FondoEfectivo(Control control)
        {
            for (Control actual = control; actual != null; actual = actual.Parent)
            {
                if (actual.BackColor.A == 255)
                    return actual.BackColor;
            }
            return Fondo;
        }

        public static GraphicsPath RectanguloRedondeado(RectangleF rect, float radio)
        {
            var camino = new GraphicsPath();
            float d = Math.Min(radio * 2, Math.Min(rect.Width, rect.Height));

            if (d <= 0)
            {
                camino.AddRectangle(rect);
                return camino;
            }

            camino.AddArc(rect.X, rect.Y, d, d, 180, 90);
            camino.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            camino.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            camino.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            camino.CloseFigure();
            return camino;
        }

        public static Color Mezclar(Color a, Color b, float t)
        {
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }

        // ---------- DataGridView ----------
        // Grilla limpia: encabezado claro sin estilo clásico de Windows, filas alternadas, solo líneas horizontales finas.
        // Los estilos viven en el control, así que sobreviven a cada "DataSource = null / DataSource = lista".
        public static void EstilizarGrilla(DataGridView grilla, int altoFila = 44, bool soloAspecto = false)
        {
            grilla.BorderStyle = BorderStyle.None;
            grilla.BackgroundColor = Tarjeta;
            grilla.GridColor = Borde;
            grilla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grilla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grilla.RowHeadersVisible = false;
            grilla.EnableHeadersVisualStyles = false;

            // Con soloAspecto no se toca el comportamiento (edición, selección): sirve para grillas que ya tienen su lógica.
            if (!soloAspecto)
            {
                grilla.AllowUserToAddRows = false;
                grilla.AllowUserToDeleteRows = false;
                grilla.AllowUserToResizeRows = false;
                grilla.AllowUserToResizeColumns = false;
                grilla.ReadOnly = true;
                grilla.MultiSelect = false;
                grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grilla.ColumnHeadersHeight = soloAspecto ? 34 : 40;

            // Los estilos se modifican "en el lugar" (no se reemplazan) para conservar formatos como "N2" que ya traiga la grilla.
            DataGridViewCellStyle encabezado = grilla.ColumnHeadersDefaultCellStyle;
            encabezado.BackColor = EncabezadoGrilla;
            encabezado.ForeColor = TextoSuave;
            encabezado.Font = Fuente(soloAspecto ? 9f : 9.5f, true);
            encabezado.SelectionBackColor = EncabezadoGrilla;
            encabezado.SelectionForeColor = TextoSuave;
            encabezado.Alignment = DataGridViewContentAlignment.MiddleLeft;
            encabezado.Padding = new Padding(soloAspecto ? 8 : 12, 0, soloAspecto ? 8 : 12, 0);
            encabezado.WrapMode = DataGridViewTriState.False;

            DataGridViewCellStyle celdas = grilla.DefaultCellStyle;
            celdas.BackColor = Tarjeta;
            celdas.ForeColor = Texto;
            celdas.Font = Fuente(soloAspecto ? 10f : 12f);
            celdas.SelectionBackColor = Seleccion;
            celdas.SelectionForeColor = SeleccionTexto;
            celdas.Padding = new Padding(soloAspecto ? 8 : 12, 0, soloAspecto ? 8 : 12, 0);
            if (!soloAspecto)
            {
                celdas.Alignment = DataGridViewContentAlignment.MiddleLeft;
                celdas.WrapMode = DataGridViewTriState.False;
            }

            DataGridViewCellStyle alterna = grilla.AlternatingRowsDefaultCellStyle;
            alterna.BackColor = FilaAlterna;
            alterna.SelectionBackColor = Seleccion;
            alterna.SelectionForeColor = SeleccionTexto;

            grilla.RowTemplate.Height = altoFila;
            ActivarDobleBuffer(grilla);
        }
        // La grilla pinta mucho más suave con doble buffer (la propiedad es protegida, se activa por reflexión).
        public static void ActivarDobleBuffer(Control control)
        {
            try
            {
                PropertyInfo propiedad = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                if (propiedad != null)
                    propiedad.SetValue(control, true, null);
            }
            catch (Exception)
            {
                // Es solo una mejora visual: si no se puede, la grilla funciona igual.
            }
        }
    }
}
