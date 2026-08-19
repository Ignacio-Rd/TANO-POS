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
    public partial class GastosVarios : Form
    {
        public GastosVarios()
        {
            InitializeComponent();
            cargarcbx();      // ← ¿está esto?
            CargarHistorial();
        }

        public void cargarcbx()
        {
            PagosVariosNegocio negocio = new PagosVariosNegocio();
            MetodoDePagoNegocio negociometodo = new MetodoDePagoNegocio();

            List<CategoriaPagosVarios> categoria = negocio.lista();
            List<Metodo_de_pago> metodo = negociometodo.Listametodo();

            cbxCategoria.DataSource = categoria;
            cbxCategoria.DisplayMember = "Nombre";
            cbxCategoria.ValueMember = "IdCategoria";

            cbxMetodoPago.DataSource = metodo;
            cbxMetodoPago.DisplayMember = "MetodoPago";  // ajustá al nombre de tu propiedad
            cbxMetodoPago.ValueMember = "id";    // ajustá al nombre de tu propiedad

            if (categoria.Count > 0) cbxCategoria.SelectedIndex = 0;
            if (metodo.Count > 0) cbxMetodoPago.SelectedIndex = 0;
        }

        private void CargarHistorial()
        {
            try
            {
                PagosVariosNegocio negocio = new PagosVariosNegocio();

                List<PagosVarios> lista = negocio.listaPagos();
                dgvHistorial.DataSource = lista;

                if (dgvHistorial.Columns["IdPago"] != null) dgvHistorial.Columns["IdPago"].Visible = false;
                if (dgvHistorial.Columns["IdCategoria"] != null) dgvHistorial.Columns["IdCategoria"].Visible = false;
                if (dgvHistorial.Columns["IdMetodoPago"] != null) dgvHistorial.Columns["IdMetodoPago"].Visible = false;

                if (dgvHistorial.Columns["Monto"] != null)
                    dgvHistorial.Columns["Monto"].DefaultCellStyle.Format = "C";
                if (dgvHistorial.Columns["Fecha"] != null)
                    dgvHistorial.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar historial: " + ex.Message);
            }
        }

        private void BtnPagar_Click(object sender, EventArgs e)
        {
            MetodoDePagoNegocio metodo = new MetodoDePagoNegocio();
            PagosVariosNegocio negocio = new PagosVariosNegocio();
            // Validar monto
            decimal monto;
            if (!decimal.TryParse(txtMonto.Text, out monto) || monto <= 0)
            {
                MessageBox.Show("Ingrese un monto válido.", "Atención",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar selección
            if (cbxCategoria.SelectedValue == null || cbxMetodoPago.SelectedValue == null)
            {
                MessageBox.Show("Seleccione categoría y método de pago.", "Atención",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult confirm = MessageBox.Show(
           $"¿Confirmar pago de {monto:C}\nCategoría: {cbxCategoria.Text}\nMétodo: {cbxMetodoPago.Text}?",
           "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {

                PagosVarios pago = new PagosVarios()
                {
                    IdCategoria = Convert.ToInt32(cbxCategoria.SelectedValue),
                    IdMetodoPago = Convert.ToInt32(cbxMetodoPago.SelectedValue),
                    Monto = monto,
                    Fecha = DateTime.Now,
                    Observacion = TxtObservaciones.Text.Trim()  // ← agregás esto
                };

                negocio.RegistrarPago(pago);

                MessageBox.Show("✅ Pago registrado correctamente.", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

               // metodo.restarAlFondo(pago.IdMetodoPago, monto);
                LimpiarCampos();
                CargarHistorial(); // Refrescamos la grilla
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtMonto.Text = "";
            TxtObservaciones.Text = "";
            cbxCategoria.SelectedIndex = 0;
            cbxMetodoPago.SelectedIndex = 0;
        }
    }

}
  