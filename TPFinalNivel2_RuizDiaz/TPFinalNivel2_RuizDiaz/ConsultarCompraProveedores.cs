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
    public partial class ConsultarCompraProveedores : Form
    {
        string nombreProveedorEnviar;

        int idProveedor;

        decimal deudaEnviar;

        public ConsultarCompraProveedores()
        {
            InitializeComponent();
        }

        private void BtnFiltrarFecha_Click(object sender, EventArgs e)
        {
            DateTime fechaseleccionada = DtpFiltro.Value.Date;
            PagoAProveedoresNegocio negocio = new PagoAProveedoresNegocio();

            // 1. Obtenemos la lista una sola vez
            List<PagoAProveedores> listafecha = negocio.listaPagos(fechaseleccionada);

            // 2. La asignamos directamente o usamos el método de carga corregido
            CargarGrillaFiltrada(listafecha);

            dgvPagos.DataSource = null;
        }

        private void FiltroRango_Click(object sender, EventArgs e)
        {
            int anio = DtpFiltro.Value.Year;
            int mes = DtpFiltro.Value.Month;
            int dia = DtpFiltro.Value.Day;

            if (DtpHoraInicio.Value.TimeOfDay > DtpHoraFin.Value.TimeOfDay)
            {
                MessageBox.Show("La hora de inicio no puede ser mayor a la hora de fin.", "Rango Horario Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime HoraInicio = new DateTime(anio, mes, dia, DtpHoraInicio.Value.Hour, DtpHoraInicio.Value.Minute, 0);
            DateTime HoraFin = new DateTime(anio, mes, dia, DtpHoraFin.Value.Hour, DtpHoraFin.Value.Minute, 59);

            PagoAProveedoresNegocio negocio = new PagoAProveedoresNegocio();
            decimal deudaHistorica; // Variable para recibir el valor out

            try
            {
                // Llamamos al nuevo método con el parámetro out
                List<PagoAProveedores> listaRango = negocio.ConsultaRango(HoraInicio, HoraFin);

                DgvVentas.DataSource = null;
                DgvVentas.DataSource = listaRango;
                OcultarColumnas();
                dgvPagos.DataSource = null;
                if (listaRango.Count == 0)
                {
                    MessageBox.Show($"No se hallaron compras entre las {HoraInicio:HH:mm} y las {HoraFin:HH:mm}.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void CargarGrillaFiltrada(List<PagoAProveedores> lista)
        {
            // Ya no le pedimos nada al 'negocio' aquí adentro.
            // Solo usamos la lista que nos llegó por el paréntesis.
            DgvVentas.DataSource = lista;
            OcultarColumnas();

            if (lista.Count == 0)
            {
                MessageBox.Show("No hay compras en la fecha seleccionada.");
            }


        }

        public void ocultarColumnasPagos()
        {
            dgvPagos.Columns["Id"].Visible = false;
            dgvPagos.Columns["IdProveedor"].Visible = false;
            dgvPagos.Columns["IdCompra"].Visible = false;
            dgvPagos.Columns["IdMetodo"].Visible = false;
            dgvPagos.Columns["Compra"].Visible = false;
            if (dgvPagos.Columns["TotalCompra"] != null)
                dgvPagos.Columns["TotalCompra"].Visible = false;
        }


        public void OcultarColumnas()
        {
            DgvVentas.Columns["Id"].Visible = false;
            DgvVentas.Columns["IdProveedor"].Visible = false;
            DgvVentas.Columns["IdCompra"].Visible = false;
            DgvVentas.Columns["IdMetodo"].Visible = false;
            DgvVentas.Columns["Compra"].Visible = false;
            DgvVentas.Columns["Monto"].Visible = false;       // "Cuanto pagaste"
            DgvVentas.Columns["metodopago"].Visible = false;  // "Metodo de Pago"
        }

        private void DgvVentas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtenemos el objeto PagoAProveedores que representa esa fila
                // Asegurate de que 'PagoAProveedores' sea el nombre de tu clase de dominio
                PagoAProveedores seleccionado = (PagoAProveedores)DgvVentas.Rows[e.RowIndex].DataBoundItem;

                // Ahora sacamos el ID directamente del objeto, no de la celda de la pantalla
                int idCompra = seleccionado.NumeroCompra;
                decimal Total = seleccionado.TotalCompra;

                VerDetalleCompra formulario = new VerDetalleCompra(idCompra, Total);
                formulario.ShowDialog();
            }

        }

        private void ConsultarCompraProveedores_Load(object sender, EventArgs e)
        {
            cargacbo();
        }

        private void cargacbo()
        {
            ProveedorNegocio negocio = new ProveedorNegocio();
            var lista = negocio.listar();

            cboProveedor.ValueMember = "Id"; // El nombre de la propiedad ID
            cboProveedor.DisplayMember = "Nombre"; // El nombre de la propiedad a mostrar
            cboProveedor.DataSource = lista;

            // Selecciona el primer ítem si la lista no está vacía
            if (lista.Count > 0)
            {
                cboProveedor.SelectedIndex = 0;
            }
        }

        private void btnFiltrarPorNombre_Click(object sender, EventArgs e)
        {
            PagoAProveedoresNegocio negocio = new PagoAProveedoresNegocio();
            List<PagoAProveedores> listaFiltrada;

            if (string.IsNullOrEmpty(cboProveedor.Text))
            {
                MessageBox.Show("no se seeccionó ningun proveedor");

                return;
            }
        
            // Usamos .Text para obtener el nombre seleccionado en el combo
            string nombreProveedor = cboProveedor.Text;

            if (!rdbFiltrarNombreFechHora.Checked)
            {
                listaFiltrada = negocio.buscarSoloPorNombre(nombreProveedor);

                nombreProveedorEnviar = nombreProveedor;
               
            }
            else
            {
                DateTime fechaseleccionada = DtpFiltro.Value.Date;
                listaFiltrada = negocio.buscarPorNombreYFecha(fechaseleccionada, nombreProveedor);
                nombreProveedorEnviar = nombreProveedor;
            }

            // 2. Validar si la lista trajo resultados (se usa == y se cuenta el .Count)
             if (listaFiltrada == null || listaFiltrada.Count == 0)
    {
        MessageBox.Show("No se encontraron registros para la selección actual.");
        
        // Opcional: Limpiar la grilla si no hay resultados
        DgvVentas.DataSource = null;

                decimal deuda1 = negocio.obtenerDeudaProveedor(nombreProveedor);

                // Supongamos que tu label se llama lblDeuda
                lblDeuda.Text = "$ " + deuda1.ToString("N2") + " A:" + nombreProveedor;

                // Opcional: Cambiar el color si hay deuda o no
                lblDeuda.ForeColor = deuda1 > 0 ? Color.Red : Color.Green;

                return;
    }

            // 3. Cargar la grilla
            DgvVentas.DataSource = listaFiltrada;
            OcultarColumnas();

            // 2. NUEVO: Calcular y mostrar la deuda en el Label
            decimal deuda = negocio.obtenerDeudaProveedor(nombreProveedor);

            deudaEnviar = deuda;

            // Supongamos que tu label se llama lblDeuda
            lblDeuda.Text = "$ " + deuda.ToString("N2") + " A:" + nombreProveedor;

            // Opcional: Cambiar el color si hay deuda o no
            lblDeuda.ForeColor = deuda > 0 ? Color.Red : Color.Green;

            btnBajarDeuda.Visible = true;

            if(rdbFiltrarNombreFechHora.Checked == false)
            {
                List<PagoAProveedores> listadepago = new List<PagoAProveedores>();

                int idProveedor = negocio.BuscarIdPorNombre(cboProveedor.Text);

                listadepago = negocio.ListaPagosAProveedores(idProveedor);

                dgvPagos.DataSource = listadepago;

                ocultarColumnasPagos();
            }

            else {

                DateTime fechaseleccionada = DtpFiltro.Value.Date;

                List<PagoAProveedores> listadepago = new List<PagoAProveedores>();

                int idProveedor = negocio.BuscarIdPorNombre(cboProveedor.Text);

                listadepago = negocio.ListaPagosAProveedoresPorFecha(idProveedor, fechaseleccionada);

                dgvPagos.DataSource = listadepago;

                ocultarColumnasPagos();
            }          
        }

     

        private void btnBajarDeuda_Click(object sender, EventArgs e)
        {
            PagarAProveedor formulario = new PagarAProveedor(nombreProveedorEnviar, deudaEnviar);
            formulario.ShowDialog();
            Limpiar();

        }

        public void Limpiar()
        {
            lblDeuda.Text = "";
            DgvVentas.DataSource = null;
            btnBajarDeuda.Visible = false;
            dgvPagos.DataSource = null;

        }

        private void btnLimpiarGrilla_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void rdbFiltrarNombreFechHora_Click(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Tag != null && (bool)rb.Tag == true)
            {
                rb.Checked = false;
                rb.Tag = false;
            }
            else
            {
                rb.Tag = true;
            }
        }
    }
}
