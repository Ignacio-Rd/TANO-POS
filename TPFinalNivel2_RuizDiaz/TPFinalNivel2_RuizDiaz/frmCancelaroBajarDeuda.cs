using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocios;
using Negocio_;

namespace TPFinalNivel2_RuizDiaz
{
    public partial class frmCancelaroBajarDeuda : Form
    {
        string NombreDeudor;
        decimal montoDeudor;
        int idDeudor;

        public frmCancelaroBajarDeuda(string Nombre, decimal monto, int id)
        {
            InitializeComponent();
            NombreDeudor = Nombre;
            montoDeudor = monto;
            idDeudor = id;

            lblDeuda.Text = montoDeudor.ToString();
            lblNombreDeudor.Text = NombreDeudor;
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                MetodoDePagoNegocio negocio = new MetodoDePagoNegocio();

                int idmetodo = negocio.IdDelMetodo(cboMetodoPago.Text);

                decimal montoIngresado = decimal.Parse(txtMontoPago.Text);

                // Confirmación
                if (MessageBox.Show($"¿Confirmar cobro de {montoIngresado:C}?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ClienteNegocio negocio1 = new ClienteNegocio();

                    // Registramos en la tabla nueva
                    negocio1.RegistrarPago(this.idDeudor, montoIngresado, idmetodo);

                    decimal saldoNuevo = montoDeudor - montoIngresado;
                    MessageBox.Show($"Cobro exitoso.\nNuevo saldo de cliente: {saldoNuevo:C}");

                   /* negocio.SumarAlFondo(idmetodo, montoIngresado); */

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void frmCancelaroBajarDeuda_Load(object sender, EventArgs e)
        {
            MetodoDePagoNegocio negocio = new MetodoDePagoNegocio();
            var lista = negocio.Listametodo();


            if (montoDeudor > 0)
            {
                lblDeuda.ForeColor = Color.Red;
            }

            else if(montoDeudor == 0)
            {
                lblDeuda.ForeColor = Color.Green;
            }

            if (lista != null && lista.Count > 0)
            {
                cboMetodoPago.DataSource = lista;
                cboMetodoPago.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No hay métodos de pago cargados.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void cboMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtMontoPago_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void lblNombreDeudor_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblDeuda_Click(object sender, EventArgs e)
        {

        }
    }
    }

