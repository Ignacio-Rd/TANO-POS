using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocios;
using Negocio_;
using Dominio;
using System.Drawing.Printing;

namespace TPFinalNivel2_RuizDiaz
{
    public partial class FechaInformeDeVentas : Form
    {
        private InformeCaja _informe;
        private DateTime _desde;
        private DateTime _hasta;
        private int _paginaActual;
        private float _yGlobal;
        private int _seccionActual; // 0=header+I, 1=II, 2=III, 3=IV, 4=totales
        private int _metodoActual;  // índice del método donde quedó en sección II

        public FechaInformeDeVentas()
        {
            InitializeComponent();
        }

        private void FechaInformeDeVentas_Load(object sender, EventArgs e)
        {
            rbtnFechaEspecifica.Checked = true;
            GbFechaEspecifica.Enabled = true;
            DtmInformeDesde.Value = DateTime.Today;
            DtmInformeHasta.Value = DateTime.Today.AddHours(23).AddMinutes(59);
        }

        private void rbtnFechaEspecifica_CheckedChanged(object sender, EventArgs e)
        {
            GpbRangoFecha.Enabled = false;
            GbFechaEspecifica.Enabled = true;
        }

        private void rbtnRangoFecha_CheckedChanged(object sender, EventArgs e)
        {
            GbFechaEspecifica.Enabled = false;
            GpbRangoFecha.Enabled = true;
        }

        private void BtnPedirInforme_Click(object sender, EventArgs e)
        {


            // Definimos las variables fuera de los IF para poder usarlas luego al generar el PDF
            DateTime fechaInicio;
            DateTime fechaFin;



            if (rbtnFechaEspecifica.Checked)
            {
                // 1. Caso: Un solo día con rango horario específico
                if (DtmInformeDesde.Value.TimeOfDay > DtmInformeHasta.Value.TimeOfDay)
                {
                    MessageBox.Show("La hora de inicio no puede ser mayor a la hora de fin.", "Rango Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                fechaInicio = new DateTime(DtmInforme.Value.Year, DtmInforme.Value.Month, DtmInforme.Value.Day,
                                           DtmInformeDesde.Value.Hour, DtmInformeDesde.Value.Minute, 0);

                fechaFin = new DateTime(DtmInforme.Value.Year, DtmInforme.Value.Month, DtmInforme.Value.Day,
                                         DtmInformeHasta.Value.Hour, DtmInformeHasta.Value.Minute, 59);
            }
            else
            {
                // 2. Caso: Rango de días completo
                if (DtmInformeFecha1.Value.Date > DtmFechaInforme2.Value.Date)
                {
                    MessageBox.Show("La fecha de inicio no puede ser mayor a la fecha fin.", "Rango Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Importante: Inicio a las 00:00:00 y Fin a las 23:59:59 para no perder datos
                fechaInicio = DtmInformeFecha1.Value.Date;
                fechaFin = DtmFechaInforme2.Value.Date.AddDays(1).AddSeconds(-1);
            }

            Venta_Por_Producto_Negocio producto = new Venta_Por_Producto_Negocio();

            InformeCaja datosInforme = producto.ObtenerResumenInforme(fechaInicio, fechaFin);

            _informe = datosInforme;
            _desde = fechaInicio;
            _hasta = fechaFin;
            _paginaActual = 1;
            _seccionActual = 0;  // ← nuevo
            _metodoActual = 0;  // ← nuevo
            _yGlobal = 0;
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += DocumentoImpresion_PrintPage;

            PrintPreviewDialog ppd = new PrintPreviewDialog { Document = pd };
            ppd.ShowDialog();


        }

        private void DocumentoImpresion_PrintPage(object sender, PrintPageEventArgs e)
        {
            DibujarTicketInforme(e, _informe, _desde, _hasta);
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

                // Fila 1: encabezados blancos
                g.DrawString("Recaudado:", fReg, Brushes.White, x + 200, y + 5);
                g.DrawString("Fondo en Caja:", fReg, Brushes.White, x + 370, y + 5);

                // Fila 2: valores
                g.DrawString("TOTALES:", fSub, Brushes.White, x + 10, y + 28);
                g.DrawString($"{totalRecaudado:C2}", fSub, Brushes.White, x + 200, y + 28);
                g.DrawString($"{totalFondoCaja:C2}", fSub, Brushes.White, x + 370, y + 28);

                y += 75;
                _seccionActual = 4;
            }
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

        private void BtnVerEnPantalla_Click(object sender, EventArgs e)
        {
            DateTime fechaInicio;
    DateTime fechaFin;

    // 1. Lógica de selección de fechas (copiada de tu lógica de validación)
    if (rbtnFechaEspecifica.Checked)
    {
        if (DtmInformeDesde.Value.TimeOfDay > DtmInformeHasta.Value.TimeOfDay)
        {
            MessageBox.Show("La hora de inicio no puede ser mayor a la hora de fin.", "Rango Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        fechaInicio = new DateTime(DtmInforme.Value.Year, DtmInforme.Value.Month, DtmInforme.Value.Day,
                                   DtmInformeDesde.Value.Hour, DtmInformeDesde.Value.Minute, 0);

        fechaFin = new DateTime(DtmInforme.Value.Year, DtmInforme.Value.Month, DtmInforme.Value.Day,
                                 DtmInformeHasta.Value.Hour, DtmInformeHasta.Value.Minute, 59);
    }
    else
    {
        if (DtmInformeFecha1.Value.Date > DtmFechaInforme2.Value.Date)
        {
            MessageBox.Show("La fecha de inicio no puede ser mayor a la fecha fin.", "Rango Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        fechaInicio = DtmInformeFecha1.Value.Date;
        fechaFin = DtmFechaInforme2.Value.Date.AddDays(1).AddSeconds(-1);
    }

    // 2. Consultar la base de datos a través de la capa de Negocio
    Venta_Por_Producto_Negocio negocio = new Venta_Por_Producto_Negocio();
    InformeCaja datosInforme = negocio.ObtenerResumenInforme(fechaInicio, fechaFin);

    // 3. Validar si trajo datos (opcional pero recomendado)
    if (datosInforme == null)
    {
        MessageBox.Show("No se encontraron movimientos en el rango seleccionado.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }

    // 4. Guardar en las variables de clase (para que queden sincronizadas)
    _informe = datosInforme;
    _desde = fechaInicio;
    _hasta = fechaFin;

    // 5. Abrir el visor personalizado
    VerInformeEnPantalla visor = new VerInformeEnPantalla();
    visor.Datos = _informe;
    visor.Desde = _desde;
    visor.Hasta = _hasta;

    visor.ShowDialog();
        }
    }
}