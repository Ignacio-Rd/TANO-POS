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
    public partial class FIADO : Form
    { 
        private decimal montoActualCompra;
        private ClienteNegocio clienteNegocio = new ClienteNegocio();
        private List<Articulos> _listaArticulos;
        private string _nombreMetodo;
        private decimal _subtotalSinRecargo;
        decimal totalacum;
        int metodopago;
        TICKET _ticketParaImprimir;
        int ID;

        public FIADO(decimal montoTotal, decimal subtotal, List<Articulos> lista, int idMetodo, string nombreMetodo, int idventa)
        {
            InitializeComponent();
            Diseno.Estilizador.Aplicar(this);
            this.montoActualCompra = montoTotal;
            // Cargamos el monto de la compra actual en el TextBox para que ya aparezca
            txtSumaDeuda.Text = montoActualCompra.ToString("0.00");
            _listaArticulos = lista;
            _nombreMetodo = nombreMetodo;
            _subtotalSinRecargo = subtotal;
            totalacum = montoTotal;
            metodopago = idMetodo;
            ID = idventa;
        }

        private void FIADO_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true; // ← AGREGÁ ESTO
            CargarComboDeudores();
            txtSumaDeuda.Text = montoActualCompra.ToString("0.00");
            lblEstadoDeuda.Text = "----";
            lblDeuda.Text = "----";

        }

        private void CargarComboDeudores()
        {
            try
            {
                // 1. Configuramos el comportamiento primero
                cboNombreDeudor.ValueMember = "Id";
                cboNombreDeudor.DisplayMember = "Nombre";

                // 2. Asignamos los datos después
                cboNombreDeudor.DataSource = clienteNegocio.listarActivos();

                // 3. Limpiamos la selección inicial
                cboNombreDeudor.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cboNombreDeudor_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si el SelectedIndex es -1, el combo está vacío o limpiándose, no hacemos nada
            if (cboNombreDeudor.SelectedIndex == -1) return;

            try
            {
                Cliente seleccionado = (Cliente)cboNombreDeudor.SelectedItem;

                

                ClienteNegocio negocio = new ClienteNegocio();
                decimal deudaTotal = negocio.ObtenerDeudaActual(seleccionado.Id);
                lblDeuda.Text = deudaTotal.ToString("N2"); // "N2" para número con 2 decimales
                lblDeuda.Refresh(); // <--- AGREGÁ ESTA LÍNEA

                // Lógica de colores para el estado
                if (deudaTotal > 0)
                {
                    lblEstadoDeuda.Text = "DEUDA PENDIENTE";
                    lblEstadoDeuda.ForeColor = Color.Red;
                }
                else
                {
                    lblEstadoDeuda.Text = "PAGADA / SIN DEUDA";
                    lblEstadoDeuda.ForeColor = Color.Green;
                }

                BtnFiar.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }




        private bool _guardando;

        private void BtnFiar_Click(object sender, EventArgs e)
        {
            if (_guardando)
                return;   // un doble clic / doble Enter no debe fiar dos veces

            if (cboNombreDeudor.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un deudor.");
                return;
            }
            _guardando = true;
            try
            {
                VentaNegocio negocio = new VentaNegocio();
                Cliente seleccionado = (Cliente)cboNombreDeudor.SelectedItem;

                if (!decimal.TryParse(txtSumaDeuda.Text, out decimal montoAFiar))
                {
                    MessageBox.Show("Por favor, ingrese un monto válido.");
                    return;
                }

                // Venta, líneas, descuento de stock y detalle del deudor en UNA sola transacción:
                // si algo falla no queda ni la deuda ni el stock a medias.
                negocio.RegistrarVentaCompleta(
                    _listaArticulos,
                    IdsMetodoPago.Fiado,
                    _nombreMetodo,
                    _subtotalSinRecargo,
                    montoAFiar,
                    seleccionado.Id);

                MessageBox.Show($"¡Venta fiada registrada con éxito!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                RegistroErrores.Registrar(ex, "Error al registrar el fiado");
                MessageBox.Show("No se pudo registrar el fiado. No se guardó nada (ni la deuda ni el stock), podés reintentar." +
                                Environment.NewLine + Environment.NewLine + "Detalle: " + ex.Message);
            }
            finally
            {
                _guardando = false;
            }
        }
        private void btnNuevoDeudor_Click(object sender, EventArgs e)
        {
            // Abrimos tu nuevo formulario como una ventana de diálogo
            using (frmNuevoClientecs ventana = new frmNuevoClientecs())
            {
                ventana.ShowDialog();

                // Si el usuario hizo clic en AGREGAR
                if (ventana.DialogResult == DialogResult.OK)
                {
                    try
                    {
                        // Guardamos en la Base de Datos usando el método que ya hicimos
                        clienteNegocio.AgregarNuevo(ventana.NombreDeudor);

                        MessageBox.Show("¡Nuevo deudor guardado con éxito!");

                        // Recargamos el ComboBox para que aparezca el nuevo nombre
                        CargarComboDeudores();

                        // Lo seleccionamos automáticamente por comodidad
                        cboNombreDeudor.Text = ventana.NombreDeudor;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al guardar: " + ex.Message);
                    }
                }
            }
        }

        private void FIADO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (this.ContainsFocus) // ← Solo cierra si TOTAL_A_COBRAR tiene el foco activo
                {
                    this.Close();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        


        }
    }


