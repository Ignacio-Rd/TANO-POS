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
    public partial class BtnAgregar : Form
    {
        int id_metodopago = 0;
        string metodopago = null;

        public BtnAgregar()
        {
            InitializeComponent();
        }



        public BtnAgregar(int id_metodo, string metodo_pago, decimal porcentaje)
        {
            InitializeComponent();
            id_metodopago = id_metodo;
            metodopago = metodo_pago;
            txtMetodoDePago.Text = metodo_pago;
            txtPorcentaje.Text = porcentaje.ToString();

            btnAgregarMetodo.Text = "Modificar";
            this.Text = "Modificar Metodo";
            
        }



        private void btnAgregarMetodo_Click(object sender, EventArgs e)
        {
            MetodoDePagoNegocio negocio = new MetodoDePagoNegocio();

            if ( string.IsNullOrEmpty(txtMetodoDePago.Text) ||
                string.IsNullOrEmpty(txtPorcentaje.Text))
            {
                MessageBox.Show("Complete todos los campos.", "Atención",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (metodopago != null && id_metodopago > 0)
                {
                    // ✅ Usás directamente id_metodopago, sin volver a buscarlo
                    negocio.modificarMetodoPago(
                        id_metodopago,
                        txtMetodoDePago.Text,
                        decimal.Parse(txtPorcentaje.Text) );
                    MessageBox.Show("✅ Método modificado correctamente.", "Éxito",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Metodo_de_pago metodo = new Metodo_de_pago();
                    metodo.MetodoPago = txtMetodoDePago.Text;
                    metodo.porcentaje = decimal.Parse(txtPorcentaje.Text);
                    negocio.agregarMetodopago(metodo);
                    MessageBox.Show("✅ Método agregado correctamente.", "Éxito",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.Close(); // ← cierra el formulario al terminar
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
