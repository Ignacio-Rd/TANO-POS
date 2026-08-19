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
    public partial class FormularioPrincipal : Form
    {

     

        public FormularioPrincipal()
        {
            InitializeComponent();
           
        }

        private void btnAbrirFormulario_Click(object sender, EventArgs e)
        {
            Form1 formulario = new Form1();
            formulario.ShowDialog();
           
        }



      

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Registradora_Click(object sender, EventArgs e)
        {
            REGISTRADORA_DE_COMPRAS formulario = new REGISTRADORA_DE_COMPRAS();
            formulario.ShowDialog();
        }

        private void Metodo_de_pago_Click(object sender, EventArgs e)
        {
            formas_de_pago formulario = new formas_de_pago();
            formulario.ShowDialog();
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Venta_Por_Venta formulario = new Venta_Por_Venta();
            formulario.ShowDialog();
           
        }

        private void FormularioPrincipal_Load(object sender, EventArgs e)
        {
            
        }

       
       
        private void BtnCajaRegistradora_Click(object sender, EventArgs e)
        {
            REGISTRADORA_DE_COMPRAS formulario = new REGISTRADORA_DE_COMPRAS();
            formulario.ShowDialog();
        }

        private void Btn_Impresora_Click(object sender, EventArgs e)
        {
            Configurar_Impresora formulario = new Configurar_Impresora();
            formulario.ShowDialog();
        }

        private void BtnIngresoStock_Click(object sender, EventArgs e)
        {
            // Busca si ya hay una instancia abierta de PAGAR_A_PROVEEDOR
            PAGAR_A_PROVEEDOR pagarExistente = Application.OpenForms.OfType<PAGAR_A_PROVEEDOR>().FirstOrDefault();

            if (pagarExistente != null)
            {
                // Ya está abierto: lo trae al frente
                pagarExistente.BringToFront();
                pagarExistente.WindowState = FormWindowState.Normal; // por si está minimizado
                pagarExistente.Focus();
            }
            else
            {
                // No está abierto: crea uno nuevo
                PAGAR_A_PROVEEDOR pagar = new PAGAR_A_PROVEEDOR();
                pagar.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ConsultarCompraProveedores proveedores = new ConsultarCompraProveedores();
            proveedores.ShowDialog();
        }

        private void BtnPagosVarios_Click(object sender, EventArgs e)
        {
            GastosVarios formulario = new GastosVarios();

            formulario.ShowDialog();
        }
    }
}
