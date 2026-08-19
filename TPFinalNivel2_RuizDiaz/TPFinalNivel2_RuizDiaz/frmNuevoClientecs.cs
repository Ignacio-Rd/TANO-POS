using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPFinalNivel2_RuizDiaz
{
    public partial class frmNuevoClientecs : Form
    {

        public string NombreDeudor { get; set; }
        public frmNuevoClientecs()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAgregarDeudor_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNuevoNombre.Text))
            {
                NombreDeudor = txtNuevoNombre.Text;
                this.DialogResult = DialogResult.OK; // Indica que se completó con éxito
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un nombre.");
            }
        }

        private void frmNuevoClientecs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAgregarDeudor_Click(sender, e);
                e.Handled = true;
            }

            else if (e.KeyCode == Keys.Escape)
            {
                btnCancelar_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
