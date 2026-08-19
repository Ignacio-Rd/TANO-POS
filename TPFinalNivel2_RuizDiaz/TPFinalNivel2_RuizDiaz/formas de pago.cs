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
using Negocio_;
using Negocios;

namespace TPFinalNivel2_RuizDiaz
{
    public partial class formas_de_pago : Form
    {
        private List<Metodo_de_pago> listametodo = new List<Metodo_de_pago>();

        private Metodo_de_pago metodo = null;

        public formas_de_pago()
        {
            InitializeComponent();
        }

    

        private void cargar()
        {
            MetodoDePagoNegocio negocio = new MetodoDePagoNegocio();
            listametodo = negocio.Listametodo();
            dgv_metodopago.DataSource = listametodo;
            dgv_metodopago.Columns["id"].Visible = false;
        }

        private void btnAgregarMetodopago_Click(object sender, EventArgs e)
        {
            MetodoDePagoNegocio negocio = new MetodoDePagoNegocio();
            BtnAgregar formulario = new BtnAgregar();

            try {

                formulario.ShowDialog();
                MetodoDePagoNegocio negocio1 = new MetodoDePagoNegocio();
                listametodo = negocio.Listametodo();
                dgv_metodopago.DataSource = listametodo;
                dgv_metodopago.Columns["id"].Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
               
            }
        }

        private void btn_eliminarmetodopago_Click(object sender, EventArgs e)
        {
            MetodoDePagoNegocio negocio = new MetodoDePagoNegocio();
            Metodo_de_pago seleccionado;

            try
            {

                DialogResult respuesta = MessageBox.Show("De verdad queres eliminar esta forma de pago?", "eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Metodo_de_pago)dgv_metodopago.CurrentRow.DataBoundItem;

                    negocio.EliminarMetodoPago(seleccionado.id);
                    cargar();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void formas_de_pago_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try {
                if (dgv_metodopago.CurrentRow != null)
            {
                Metodo_de_pago seleccionado = (Metodo_de_pago)dgv_metodopago.CurrentRow.DataBoundItem;

                BtnAgregar formulario = new BtnAgregar(
                    seleccionado.id,
                    seleccionado.MetodoPago,
                    seleccionado.porcentaje
                );

                formulario.ShowDialog();
                    cargar();
            }
            else
            {
                MessageBox.Show("Seleccioná un método de pago primero.");
            }


            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}
