using System;
using System.Drawing;
using System.Windows.Forms;
using TPFinalNivel2_RuizDiaz.Diseno;

namespace TPFinalNivel2_RuizDiaz
{
    // Aspecto de la pantalla de cobro. Los controles y sus eventos son los de siempre (button1 = FINALIZAR,
    // BtnImprimir = TICKET, BtnFiar = FIAR, LblTotal, lblMetodo): solo cambian de lugar y de estilo.
    // Teclado: Enter finaliza la venta y Esc vuelve (manejador TOTAL_A_COBRAR_KeyDown, sin cambios).
    public partial class TOTAL_A_COBRAR
    {
        private void AplicarDisenioCobro()
        {
            SuspendLayout();

            Text = "Cobrar venta";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(780, 500);
            BackColor = Tema.Fondo;

            // Se sacan los controles viejos (sin destruirlos) para ubicarlos de nuevo.
            Controls.Clear();

            var cuerpo = new Panel { Dock = DockStyle.Fill, BackColor = Tema.Fondo, Padding = new Padding(32, 24, 32, 16) };

            // Título
            var titulo = new Label
            {
                Text = "Cobrar venta",
                Font = Tema.Fuente(18f, true),
                ForeColor = Tema.Texto,
                BackColor = Tema.Fondo,
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 36
            };
            var subtitulo = new Label
            {
                Text = "Revisá el importe y elegí cómo cerrar la venta",
                Font = Tema.Fuente(10f),
                ForeColor = Tema.TextoSuave,
                BackColor = Tema.Fondo,
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 26
            };

            // Total (tarjeta oscura)
            var tarjetaTotal = new TarjetaPanel
            {
                Dock = DockStyle.Top,
                Height = 150,
                Titulo = "Total a cobrar",
                ColorFondo = Tema.MenuFondo,
                ColorBorde = Tema.MenuFondo,
                ColorTitulo = Tema.MenuTextoSuave,
                Margin = Padding.Empty
            };
            LblTotal.AutoSize = false;
            LblTotal.Dock = DockStyle.Fill;
            LblTotal.Font = Tema.Fuente(40f, true);
            LblTotal.ForeColor = Color.White;
            LblTotal.BackColor = Color.Transparent;
            LblTotal.TextAlign = ContentAlignment.MiddleRight;
            tarjetaTotal.Controls.Add(LblTotal);

            // Forma de pago
            var tarjetaMetodo = new TarjetaPanel
            {
                Dock = DockStyle.Top,
                Height = 92,
                Titulo = "Forma de pago",
                Margin = Padding.Empty
            };
            lblMetodo.AutoSize = false;
            lblMetodo.Dock = DockStyle.Fill;
            lblMetodo.Font = Tema.Fuente(16f, true);
            lblMetodo.ForeColor = Tema.Texto;
            lblMetodo.BackColor = Color.Transparent;
            lblMetodo.TextAlign = ContentAlignment.MiddleLeft;
            tarjetaMetodo.Controls.Add(lblMetodo);

            // Botones
            var botones = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 64,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Tema.Fondo,
                Margin = Padding.Empty
            };
            botones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26));
            botones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            botones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            botones.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            ConfigurarBoton(BtnFiar, "Fiar", null, VarianteBoton.Secundario, new Padding(0, 6, 8, 6));
            ConfigurarBoton(BtnImprimir, "Ticket", "Imprimir", VarianteBoton.Primario, new Padding(4, 6, 8, 6));
            ConfigurarBoton(button1, "Finalizar", "Enter", VarianteBoton.Exito, new Padding(4, 6, 0, 6));
            botones.Controls.Add(BtnFiar, 0, 0);
            botones.Controls.Add(BtnImprimir, 1, 0);
            botones.Controls.Add(button1, 2, 0);

            // Ayuda de teclado
            var ayuda = new Label
            {
                Text = "Enter  finalizar la venta        Esc  volver y corregir",
                Font = Tema.Fuente(9.5f),
                ForeColor = Tema.TextoSuave,
                BackColor = Tema.Fondo,
                AutoSize = false,
                Dock = DockStyle.Bottom,
                Height = 28,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // En un contenedor con Dock=Top el último agregado queda arriba: se agregan de abajo hacia arriba.
            cuerpo.Controls.Add(ayuda);
            cuerpo.Controls.Add(Separador(18));
            cuerpo.Controls.Add(botones);
            cuerpo.Controls.Add(Separador(16));
            cuerpo.Controls.Add(tarjetaMetodo);
            cuerpo.Controls.Add(Separador(16));
            cuerpo.Controls.Add(tarjetaTotal);
            cuerpo.Controls.Add(Separador(14));
            cuerpo.Controls.Add(subtitulo);
            cuerpo.Controls.Add(titulo);

            Controls.Add(cuerpo);
            ResumeLayout(true);
        }

        private static Panel Separador(int alto)
        {
            return new Panel { Dock = DockStyle.Top, Height = alto, BackColor = Tema.Fondo };
        }

        private static void ConfigurarBoton(Button boton, string texto, string atajo, VarianteBoton variante, Padding margen)
        {
            var plano = (BotonPlano)boton;
            plano.Text = texto;
            plano.Atajo = atajo;
            plano.Variante = variante;
            plano.Image = null;
            plano.Dock = DockStyle.Fill;
            plano.Margin = margen;
            plano.Font = Tema.Fuente(13f, true);
        }
    }
}
