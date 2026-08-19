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
    public partial class VerDetalleCompra : Form
    {
        List<DetalleCompra> detalle;

        PagoAProveedoresNegocio negocio = new PagoAProveedoresNegocio();

        decimal TotalCompra;

        int IdCompra;

        public VerDetalleCompra(int idcompra, decimal monto)
        {
            InitializeComponent();

            IdCompra = idcompra;

            TotalCompra = monto;
        }

        private void OcultarColumna()
        {
            DgvDetalles.Columns["PrecioVenta"].Visible = false;
        }

        private void VerDetalleCompra_Load(object sender, EventArgs e)
        {
            detalle = negocio.listadetalle(IdCompra);


             if (detalle == null)
            {

                MessageBox.Show("Tipo de pago cancelación de Deuda. /n Usted pagó: " + TotalCompra);
            }

            DgvDetalles.DataSource = detalle;

            OcultarColumna();

            lblTotal.Text = TotalCompra.ToString();

           
        }
    }
}
