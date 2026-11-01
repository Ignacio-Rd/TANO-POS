using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Negocio_;
using TPFinalNivel2_RuizDiaz.Diseno;

namespace TPFinalNivel2_RuizDiaz.Auditoria
{
    // Registro de ventas eliminadas: quién, cuándo y por qué. Solo lectura.
    internal class VentasEliminadasForm : Form
    {
        private readonly DataGridView _grilla = new DataGridView();

        public VentasEliminadasForm()
        {
            Text = "Ventas eliminadas";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1100, 560);
            MinimumSize = new Size(800, 400);
            BackColor = Tema.Fondo;
            Font = Tema.Fuente(10f);
            KeyPreview = true;

            var titulo = new Label
            {
                Text = "Ventas eliminadas",
                Font = Tema.Fuente(15f, true),
                ForeColor = Tema.Texto,
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(16, 10, 0, 0)
            };

            _grilla.Dock = DockStyle.Fill;
            _grilla.ReadOnly = true;
            _grilla.AllowUserToAddRows = false;
            _grilla.AllowUserToDeleteRows = false;
            _grilla.AutoGenerateColumns = true;
            _grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grilla.MultiSelect = false;
            _grilla.RowHeadersVisible = false;
            Tema.EstilizarGrilla(_grilla, 34, true);

            var pie = new Label
            {
                Text = "Solo lectura. Esc para cerrar.",
                ForeColor = Tema.TextoSuave,
                AutoSize = false,
                Dock = DockStyle.Bottom,
                Height = 28,
                Padding = new Padding(16, 4, 0, 0)
            };

            Controls.Add(_grilla);
            Controls.Add(titulo);
            Controls.Add(pie);

            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };
            Load += (s, e) => Cargar();
        }

        private void Cargar()
        {
            try
            {
                DataTable tabla = new VentaNegocio().ListarVentasEliminadas();
                _grilla.DataSource = tabla;
                ArmarColumnas();
            }
            catch (Exception ex)
            {
                RegistroErrores.Registrar(ex, "Ver ventas eliminadas");
                MessageBox.Show(ex.Message, "Ventas eliminadas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }

        private void ArmarColumnas()
        {
            Titulo("FechaEliminacion", "Eliminada", "dd/MM/yyyy HH:mm", 130);
            Titulo("Usuario", "Quién", null, 150);
            Titulo("Equipo", "Equipo", null, 110);
            Titulo("IdVenta", "Venta Nº", null, 70);
            Titulo("FechaVenta", "Fecha de la venta", "dd/MM/yyyy HH:mm", 130);
            Titulo("Total", "Total", "C2", 90);
            Titulo("MetodoPago", "Método", null, 100);
            Titulo("Motivo", "Por qué", null, 220);
            Titulo("UnidadesRepuestas", "Unid. repuestas", null, 90);
            Titulo("Productos", "Productos", null, 300);
        }

        private void Titulo(string columna, string texto, string formato, int ancho)
        {
            DataGridViewColumn c = _grilla.Columns[columna];
            if (c == null)
                return;

            c.HeaderText = texto;
            c.Width = ancho;
            if (formato != null)
                c.DefaultCellStyle.Format = formato;
        }
    }
}
