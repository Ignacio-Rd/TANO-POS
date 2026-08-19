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
    public partial class PagarAProveedor : Form
    {
        

        string nombreProveedor;

        decimal deudaAProveedor;


        public PagarAProveedor(string nombre, decimal Deuda)
        {
            nombreProveedor = nombre;

            deudaAProveedor = Deuda;

            InitializeComponent();
        }

        private void cargar()
        {
            lblDeuda.Text = "$ " + deudaAProveedor.ToString("N2") + " A:" + nombreProveedor;

            MetodoDePagoNegocio negocio = new MetodoDePagoNegocio();

            List<Metodo_de_pago> lista = negocio.Listametodo();

            cboMetodo.DataSource = lista;

            if (lista.Count > 0)
            {
                cboMetodo.SelectedIndex = 0;
            }

        }

        private void PagarAProveedor_Load(object sender, EventArgs e)
        {
            cargar();
            
        }

        private bool Validar_PlataProveedor()
        {
            bool esValido = true;
            errorProvider1.Clear();



            // 2. Validar el PAGO AL PROVEEDOR
            if (!decimal.TryParse(TxtPagoAProveedor.Text, out decimal pago) || pago < 0)
            {
                errorProvider1.SetError(TxtPagoAProveedor, "Ingrese un monto de pago válido mayor a cero.");
                esValido = false;
            }



            // Nota: Si solo estás finalizando la compra, no necesitas validar precios de venta aquí
            // a menos que el checkbox de "Actualizar Precio" esté marcado.

            return esValido;
        }

        private void btnCobrar_Click(object sender, EventArgs e)
        {
            

            if (!Validar_PlataProveedor())
            {
                return;
            }

            string metodo;

            metodo = cboMetodo.SelectedItem != null ? cboMetodo.SelectedItem.ToString() : "";

            if (string.IsNullOrEmpty(metodo))
            {
                MessageBox.Show("Seleccione un metodo", "Atención");
                return;
            }

            PagoAProveedoresNegocio negocio = new PagoAProveedoresNegocio();

            ClienteNegocio clienteNegocio = new ClienteNegocio();

            MetodoDePagoNegocio metododepago = new MetodoDePagoNegocio();

            try
            {
                if (MessageBox.Show($"¿Confirmar cobro de {TxtPagoAProveedor.Text:C}?", "Confirmar",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int idProveedor = negocio.BuscarIdPorNombre(nombreProveedor);

                    decimal DineroAProveedor = decimal.Parse(TxtPagoAProveedor.Text);

                    int metododepagoid = metododepago.IdDelMetodo(metodo);

                    negocio.PagarAProveedor(idProveedor,  DineroAProveedor, metododepagoid);

                    decimal DeudaNueva = deudaAProveedor - DineroAProveedor;

                    MessageBox.Show($"Pago exitoso.\nLa deuda con {nombreProveedor} es de: {DeudaNueva:C}");

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }


                }
            catch (Exception)
            {

                throw;
            }
                        
        }

       
    }
}
