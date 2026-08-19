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




        private void BtnFiar_Click(object sender, EventArgs e)
        {
            if (cboNombreDeudor.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un deudor.");
                return;
            }
            try
            {
                ArticuloNegocio articuloNegocio = new ArticuloNegocio();
                VentaNegocio negocio = new VentaNegocio();
                Cliente seleccionado = (Cliente)cboNombreDeudor.SelectedItem;

                Ventas ventaFiada = new Ventas();
                ventaFiada.Fecha = DateTime.Now;

                if (!decimal.TryParse(txtSumaDeuda.Text, out decimal montoAFiar))
                {
                    MessageBox.Show("Por favor, ingrese un monto válido.");
                    return;
                }

                ventaFiada.Total = montoAFiar;
                ventaFiada.metododepago = new Metodo_de_pago();
                ventaFiada.metododepago.id = 11; // ID de FIADO

                // --- CAMBIO CLAVE: Capturamos el ID de la venta que se acaba de guardar ---
                int idRecienCreado = negocio.RegistrarVenta(ventaFiada, seleccionado.Id);

                Venta_Por_Producto_Negocio vppNegocio = new Venta_Por_Producto_Negocio();

                var productosAgrupados = _listaArticulos.GroupBy(x => x.codigo)
                                                      .Select(g => new { Codigo = g.Key, Total = g.Count() });

                foreach (var item in productosAgrupados)
                {
                    articuloNegocio.Descontar_Stock(item.Codigo, item.Total);
                }

                // Ahora usamos idRecienCreado en lugar de ID
                if (idRecienCreado > 0)
                {
                    vppNegocio.Agregar_Venta_Producto(
                        _listaArticulos,
                        DateTime.Now,
                        idRecienCreado, // <--- Aquí usamos el ID real de la base de datos
                        _subtotalSinRecargo,
                        _nombreMetodo,
                        totalacum
                    );
                }


                    // Guardar detalle en DETALLE_COMPRAS_FIADO
                    vppNegocio.GuardarDetalleFiado(_listaArticulos, idRecienCreado);

                    MessageBox.Show($"¡Venta fiada registrada con éxito!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el fiado: " + ex.Message);
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


