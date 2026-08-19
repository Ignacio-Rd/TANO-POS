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

namespace TPFinalNivel2_RuizDiaz
{
    public partial class PAGAR_A_PROVEEDOR : Form
    {
        public PAGAR_A_PROVEEDOR()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Validación correcta: que no haya nada seleccionado en el combo
            if (cbxProveedores.SelectedIndex == -1 || cbxProveedores.SelectedValue == null)
            {
                MessageBox.Show("INGRESE EL NOMBRE DE UN PROVEEDOR");
                return;
            }

            int idProv = Convert.ToInt32(cbxProveedores.SelectedValue);
            ENTRADA_DE_MERCADERIA formularioExistente = Application.OpenForms.OfType<ENTRADA_DE_MERCADERIA>().FirstOrDefault();

            if (formularioExistente != null)
            {
                formularioExistente.BringToFront();
                formularioExistente.WindowState = FormWindowState.Normal;
                formularioExistente.Focus();
            }
            else
            {
                ENTRADA_DE_MERCADERIA formulario = new ENTRADA_DE_MERCADERIA(cbxProveedores.Text, idProv);
                formulario.Show();
            }
        }

        private void btnAgregarProveedor_Click(object sender, EventArgs e)
        {
            CargarProveedor formulario = new CargarProveedor();

            formulario.ShowDialog();

            cargar();

                    }

        private void cargar()
        {
            ProveedorNegocio negocio = new ProveedorNegocio();
            var lista = negocio.listar();

            cbxProveedores.DataSource = lista;

            // Lo que el usuario ve:
            cbxProveedores.DisplayMember = "Nombre";

            // Lo que el sistema usa por detrás (el ID):
            cbxProveedores.ValueMember = "Id";
        }

        private void PAGAR_A_PROVEEDOR_Load(object sender, EventArgs e)
        {
            cargar();
            

        }

        private void cbxProveedores_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                button2_Click(sender, e);
            }
        }
    }
}
