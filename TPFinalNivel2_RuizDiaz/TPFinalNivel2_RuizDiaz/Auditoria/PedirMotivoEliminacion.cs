using System;
using System.Drawing;
using System.Windows.Forms;
using TPFinalNivel2_RuizDiaz.Diseno;

namespace TPFinalNivel2_RuizDiaz.Auditoria
{
    // Pide el motivo antes de eliminar una venta. Se maneja solo con teclado: escribir, Enter = eliminar, Esc = cancelar.
    internal class PedirMotivoEliminacion : Form
    {
        private const int LargoMinimo = 3;

        private readonly TextBox _motivo = new TextBox();
        private readonly Label _error = new Label();

        public string Motivo
        {
            get { return _motivo.Text.Trim(); }
        }

        public PedirMotivoEliminacion(string resumenDeLaVenta)
        {
            Text = "Eliminar venta";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(520, 284);
            BackColor = Tema.Fondo;
            Font = Tema.Fuente(10f);
            KeyPreview = true;

            var titulo = new Label
            {
                Text = "Eliminar venta",
                Font = Tema.Fuente(15f, true),
                ForeColor = Tema.Texto,
                AutoSize = false,
                Location = new Point(24, 18),
                Size = new Size(472, 30)
            };

            var resumen = new Label
            {
                Text = resumenDeLaVenta,
                ForeColor = Tema.TextoSuave,
                AutoSize = false,
                Location = new Point(24, 52),
                Size = new Size(472, 22)
            };

            var aviso = new Label
            {
                Text = "Se van a devolver los productos al stock y va a quedar registrado quién la eliminó, cuándo y por qué.",
                ForeColor = Tema.Aviso,
                BackColor = Tema.AvisoFondo,
                AutoSize = false,
                Location = new Point(24, 82),
                Size = new Size(472, 54),
                Padding = new Padding(8, 6, 8, 6)
            };

            var etiqueta = new Label
            {
                Text = "Motivo (obligatorio)",
                Font = Tema.Fuente(9.5f, true),
                ForeColor = Tema.Texto,
                AutoSize = false,
                Location = new Point(24, 148),
                Size = new Size(472, 20)
            };

            _motivo.Location = new Point(24, 172);
            _motivo.Size = new Size(472, 28);
            _motivo.MaxLength = 300;
            _motivo.Font = Tema.Fuente(11f);

            _error.ForeColor = Tema.Peligro;
            _error.AutoSize = false;
            _error.Location = new Point(24, 204);
            _error.Size = new Size(472, 20);

            var eliminar = new BotonPlano
            {
                Text = "Eliminar venta",
                Variante = VarianteBoton.Peligro,
                Atajo = "Enter",
                Location = new Point(246, 230),
                Size = new Size(250, 40)
            };
            eliminar.Click += (s, e) => Confirmar();

            var cancelar = new BotonPlano
            {
                Text = "Cancelar",
                Variante = VarianteBoton.Secundario,
                Atajo = "Esc",
                Location = new Point(24, 230),
                Size = new Size(200, 40)
            };
            cancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { titulo, resumen, aviso, etiqueta, _motivo, _error, eliminar, cancelar });
            CancelButton = cancelar;

            _motivo.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    Confirmar();
                }
            };

            Shown += (s, e) => _motivo.Focus();
        }

        private void Confirmar()
        {
            if (Motivo.Length < LargoMinimo)
            {
                _error.Text = "Escribí el motivo (por ejemplo: \"error de carga\", \"cliente devolvió\").";
                _motivo.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
