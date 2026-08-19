using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio_;
using Negocios;

namespace TPFinalNivel2_RuizDiaz
{
    public partial class VerInformeEnPantalla : Form
    {
        private int _totalPaginas = 0;
        // Propiedades públicas para recibir los datos
        public InformeCaja Datos { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }

        // Variables de estado para paginación (igual que FechaInformeDeVentas)
        private int _paginaActual;
        private int _seccionActual;
        private int _metodoActual;

        public VerInformeEnPantalla()
        {
            InitializeComponent();
        }


        private void Pd_BeginPrint(object sender, PrintEventArgs e)
        {
            // Esto resetea todo justo antes de que el visor empiece a dibujar
            _paginaActual = 1;
            _seccionActual = 0;
            _metodoActual = 0;
        }


        private void VerInformeEnPantalla_Shown(object sender, EventArgs e)
        {
            if (Datos == null)
            {
                MessageBox.Show("No hay datos para mostrar.", "Sin datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _paginaActual = 1;
            _seccionActual = 0;
            _metodoActual = 0;

            PrintDocument pd = new PrintDocument();
            pd.PrintController = new PreviewPrintController(); // 👈 CLAVE
            pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
            pd.BeginPrint += Pd_BeginPrint;
            pd.PrintPage += Pd_PrintPage;

            ppcInforme.Document = pd;
            ppcInforme.AutoZoom = true;

            // Genera TODAS las páginas
            pd.Print();

            // Total de páginas generadas
            _totalPaginas = ((PreviewPrintController)pd.PrintController)
                                .GetPreviewPageInfo().Length;

            //lblPagina.Text = $"Página 1 de {_totalPaginas}";
            ppcInforme.StartPage = 0;
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                DibujarTicketInforme(e, Datos, Desde, Hasta);
            }
            catch (Exception ex)
            {
                e.Graphics.DrawString("Error: " + ex.Message,
                    new Font("Arial", 10), Brushes.Red, 10, 10);
                e.HasMorePages = false;
            }
        }

        private void DibujarPiePagina(Graphics g, PrintPageEventArgs e, Font fSmall)
        {
            g.DrawString("Informe emitido por el Sistema de Gestión de Ventas v1.0",
                fSmall, Brushes.Gray, 60, e.PageBounds.Height - 60);
            g.DrawString($"Página {_paginaActual}", fSmall, Brushes.Gray,
                e.PageBounds.Width - 120, e.PageBounds.Height - 60);
        }


        private void DibujarTicketInforme(PrintPageEventArgs e, InformeCaja info, DateTime desde, DateTime hasta)
        {
            // Guard: si ya terminamos, no dibujar nada
            if (_seccionActual == 5)
            {
                e.HasMorePages = false;
                return;
            }

            Graphics g = e.Graphics;

            Font fTit = new Font("Arial", 18, FontStyle.Bold);
            Font fSub = new Font("Arial", 14, FontStyle.Bold);
            Font fReg = new Font("Arial", 12);
            Font fSmall = new Font("Arial", 10, FontStyle.Italic);

            int x = 60;
            int anchoUtil = e.PageBounds.Width - 120;
            float margenInf = e.PageBounds.Height - 80;
            float y = 50;

            StringFormat centro = new StringFormat { Alignment = StringAlignment.Center };

            // ── Corta la página si no hay espacio ──────────────────────────────────
            // Solo devuelve true/false. El pie y HasMorePages se manejan AFUERA.
            bool Cortar(float espacioRequerido)
            {
                if (y + espacioRequerido > margenInf)
                {
                    DibujarPiePagina(g, e, fSmall);
                    e.HasMorePages = true;
                    _paginaActual++;
                    return true;
                }
                return false;
            }

            // ── ENCABEZADO (solo página 1) ─────────────────────────────────────────
            if (_paginaActual == 1)
            {
                g.DrawString("REPORTE DE CIERRE DE CAJA", fTit, Brushes.Black,
                    new RectangleF(0, y, e.PageBounds.Width, 40), centro); y += 40;
                g.DrawString($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}", fSmall, Brushes.DarkGray,
                    new RectangleF(0, y, e.PageBounds.Width, 20), centro); y += 40;
                g.DrawRectangle(Pens.Black, x, (int)y, anchoUtil, 60);
                g.DrawString("PERIODO CONSULTADO", fSub, Brushes.Black, x + 10, y + 5);
                g.DrawString($"Desde: {desde:dd/MM/yyyy HH:mm}   -   Hasta: {hasta:dd/MM/yyyy HH:mm}",
                    fReg, Brushes.Black, x + 10, y + 30);
                y += 90;
            }

            // ══════════════════════════════════════════════
            // SECCIÓN I: EFECTIVO
            // ══════════════════════════════════════════════
            if (_seccionActual == 0)
            {
                if (Cortar(160)) return;

                g.DrawString("I. DISPONIBILIDAD EN EFECTIVO", fSub, Brushes.DodgerBlue, x, y); y += 30;
                g.DrawLine(Pens.Gray, x, (int)y, x + anchoUtil, (int)y); y += 10;

                decimal entradaBruta = info.EfectivoVentas + info.PagosAProveedores + info.GastosVariosEfectivo;
                g.DrawString("Entradas (Ventas + Cobros):", fReg, Brushes.Black, x + 20, y);
                g.DrawString($"{entradaBruta:C2}", fReg, Brushes.Black, x + anchoUtil - 100, y); y += 25;

                g.DrawString("Salidas (Pagos Proveedores):", fReg, Brushes.Red, x + 20, y);
                g.DrawString($"- {info.PagosAProveedores:C2}", fReg, Brushes.Red, x + anchoUtil - 100, y); y += 25;

                g.DrawString("Salidas (Gastos Varios):", fReg, Brushes.Red, x + 20, y);
                g.DrawString($"- {info.GastosVariosEfectivo:C2}", fReg, Brushes.Red, x + anchoUtil - 100, y); y += 30;

                g.FillRectangle(Brushes.LightGray, x, (int)y, anchoUtil, 30);
                g.DrawString("TOTAL EFECTIVO NETO EN CAJA:", fSub, Brushes.Black, x + 5, y + 5);
                g.DrawString($"{info.EfectivoVentas:C2}", fSub, Brushes.Black, x + anchoUtil - 110, y + 5);
                y += 55;

                _seccionActual = 1;
            }

            // ══════════════════════════════════════════════
            // SECCIÓN II: MÉTODOS DIGITALES
            // ══════════════════════════════════════════════
            if (_seccionActual == 1)
            {
                if (_metodoActual == 0)
                {
                    if (Cortar(60)) return;
                    g.DrawString("II. OTROS MEDIOS DE PAGO (Bancarizado/Digital)", fSub, Brushes.DarkGreen, x, y); y += 30;
                    g.DrawLine(Pens.Gray, x, (int)y, x + anchoUtil, (int)y); y += 10;
                }

                while (_metodoActual < info.DesgloseMetodos.Count)
                {
                    var m = info.DesgloseMetodos[_metodoActual];

                    float alturaBloque = 25 + 22 + 30 + 10;
                    if (m.PagosProveedores > 0) alturaBloque += 22;
                    if (m.GastosVarios > 0) alturaBloque += 22;

                    if (Cortar(alturaBloque)) return;

                    g.DrawString($"► {m.Nombre}", fSub, Brushes.DarkGreen, x + 10, y); y += 25;

                    decimal entradaBrutaMetodo = m.Total + m.PagosProveedores + m.GastosVarios;
                    g.DrawString("    Entradas (Ventas + Cobros):", fReg, Brushes.Black, x + 20, y);
                    g.DrawString($"{entradaBrutaMetodo:C2}", fReg, Brushes.Black, x + anchoUtil - 100, y); y += 22;

                    if (m.PagosProveedores > 0)
                    {
                        g.DrawString("    Salidas (Pagos Proveedores):", fReg, Brushes.Red, x + 20, y);
                        g.DrawString($"- {m.PagosProveedores:C2}", fReg, Brushes.Red, x + anchoUtil - 100, y); y += 22;
                    }

                    if (m.GastosVarios > 0)
                    {
                        g.DrawString("    Salidas (Gastos Varios):", fReg, Brushes.Red, x + 20, y);
                        g.DrawString($"- {m.GastosVarios:C2}", fReg, Brushes.Red, x + anchoUtil - 100, y); y += 22;
                    }

                    Brush pincelNeto = m.Total < 0 ? Brushes.Red : Brushes.DarkGreen;
                    g.DrawString($"    Neto {m.Nombre}:", fReg, pincelNeto, x + 20, y);
                    g.DrawString($"{m.Total:C2}", fReg, pincelNeto, x + anchoUtil - 100, y); y += 30;
                    g.DrawLine(Pens.LightGray, x + 20, (int)y, x + anchoUtil - 20, (int)y); y += 10;

                    _metodoActual++;
                }

                y += 10;
                _seccionActual = 2;
            }

            // ══════════════════════════════════════════════
            // SECCIÓN III: FIADOS
            // ══════════════════════════════════════════════
            if (_seccionActual == 2)
            {
                if (Cortar(150)) return;

                g.DrawString("III. RESUMEN DE FIADOS", fSub, Brushes.DarkRed, x, y); y += 30;
                g.DrawLine(Pens.Gray, x, (int)y, x + anchoUtil, (int)y); y += 10;

                g.DrawString("Cobranza a Deudores (Entradas):", fReg, Brushes.Black, x + 20, y);
                g.DrawString($"{info.CobroDeudores:C2}", fReg, Brushes.Black, x + anchoUtil - 100, y); y += 25;

                g.DrawString("Ventas Fiadas:", fReg, Brushes.Black, x + 20, y);
                g.DrawString($"{info.VentasFiadasNuevas:C2}", fReg, Brushes.Black, x + anchoUtil - 100, y); y += 30;

                g.DrawRectangle(Pens.Red, x, (int)y, anchoUtil, 40);
                g.DrawString("DEUDA TOTAL PENDIENTE DE CLIENTES:", fSub, Brushes.Red, x + 5, y + 10);
                g.DrawString($"{info.SaldoHistorico:C2}", fSub, Brushes.Red, x + anchoUtil - 110, y + 10);
                y += 70;

                _seccionActual = 3;
            }

            // ══════════════════════════════════════════════
            // SECCIÓN IV: FONDOS
            // ══════════════════════════════════════════════
            if (_seccionActual == 3)
            {
                float alturaFondos = 80 + (info.DesgloseMetodos.Count + 1) * 25f + 70;
                if (Cortar(alturaFondos)) return;

                g.DrawString("IV. ESTADO DE FONDOS POR MÉTODO DE PAGO", fSub, Brushes.DarkSlateBlue, x, y); y += 30;
                g.DrawLine(Pens.Gray, x, (int)y, x + anchoUtil, (int)y); y += 10;

                // ── Encabezados de columnas ──────────────────────────
                g.DrawString("Método", fReg, Brushes.DarkSlateBlue, x + 20, y);
                g.DrawString("Recaudado", fReg, Brushes.DarkSlateBlue, x + 200, y);
                g.DrawString("Fondo en Caja", fReg, Brushes.DarkSlateBlue, x + 370, y);
                y += 22;
                g.DrawLine(Pens.LightGray, x, (int)y, x + anchoUtil, (int)y); y += 8;

                // ── Fila Efectivo ────────────────────────────────────
                decimal totalRecaudado = info.EfectivoVentas;
                decimal totalFondoCaja = info.FondoInicialEfectivo;

                g.DrawString("Efectivo:", fReg, Brushes.Black, x + 20, y);
                g.DrawString($"{info.EfectivoVentas:C2}", fReg, Brushes.Black, x + 200, y);
                g.DrawString($"{info.FondoInicialEfectivo:C2}", fReg, Brushes.Black, x + 370, y);
                y += 25;

                // ── Filas de métodos digitales ───────────────────────
                foreach (var m in info.DesgloseMetodos)
                {
                    totalRecaudado += m.Total;
                    totalFondoCaja += m.FondoInicial;

                    Brush pincelRec = m.Total < 0 ? Brushes.Red : Brushes.Black;
                    Brush pincelFondo = m.FondoInicial < 0 ? Brushes.Red : Brushes.Black;

                    g.DrawString($"{m.Nombre}:", fReg, Brushes.Black, x + 20, y);
                    g.DrawString($"{m.Total:C2}", fReg, pincelRec, x + 200, y);
                    g.DrawString($"{m.FondoInicial:C2}", fReg, pincelFondo, x + 370, y);
                    y += 25;
                }

                y += 10;

                // ── Barra de totales (dos filas, altura 60) ──────────
                g.FillRectangle(Brushes.DarkSlateBlue, x, (int)y, anchoUtil, 60);
                g.DrawString("Recaudado:", fReg, Brushes.White, x + 200, y + 5);
                g.DrawString("Fondo en Caja:", fReg, Brushes.White, x + 370, y + 5);
                g.DrawString("TOTALES:", fSub, Brushes.White, x + 10, y + 28);
                g.DrawString($"{totalRecaudado:C2}", fSub, Brushes.White, x + 200, y + 28);
                g.DrawString($"{totalFondoCaja:C2}", fSub, Brushes.White, x + 370, y + 28);

                y += 75;
                _seccionActual = 4;
            }

            // ══════════════════════════════════════════════
            // TOTAL GENERAL + PIE FINAL
            // ══════════════════════════════════════════════
            if (_seccionActual == 4)
            {
                if (Cortar(120)) return;
                // En lugar de usar info.TotalGeneralRecaudado,
                // usá el mismo totalFondos que ya calculaste arriba
                g.FillRectangle(Brushes.Black, x, (int)y, anchoUtil, 40);
                g.DrawString("TOTAL GENERAL RECAUDADO:", fSub, Brushes.White, x + 10, y + 10);
                g.DrawString($"{info.TotalGeneralRecaudado:C2}", fSub, Brushes.White, x + anchoUtil - 120, y + 10);
                y += 70;



                g.DrawLine(Pens.Black, x + 50, (int)y, x + 250, (int)y);
                g.DrawString("Firma Responsable", fReg, Brushes.Black, x + 90, y + 5);

                _seccionActual = 5;
            }

            // ── Pie de página y control de páginas ────────────────────────────────
            DibujarPiePagina(g, e, fSmall);
            e.HasMorePages = (_seccionActual < 5);
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (ppcInforme.StartPage < _totalPaginas - 1)
            {
                ppcInforme.StartPage++;
               // lblPagina.Text = $"Página {ppcInforme.StartPage + 1} de {_totalPaginas}";
            }


        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (ppcInforme.StartPage > 0)
            {
                ppcInforme.StartPage--;
               // lblPagina.Text = $"Página {ppcInforme.StartPage + 1} de {_totalPaginas}";
            }
        }
    }

}
