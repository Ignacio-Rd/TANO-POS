using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocios;
using Negocio_;
using System.Windows.Input;

namespace TPFinalNivel2_RuizDiaz
{
    public partial class REGISTRADORA_DE_COMPRAS : Form
    {
        private List<Articulos> listaArticulos = new List<Articulos>();

        private int cantidadA_Agregar = 1;

        //private bool Total = false;

        //private bool Sumar = false;



        private decimal total_acumulado = 0;





        //string textoLabel = lblNumero.Text;  // Ejemplo: "123"
        //int numero = int.Parse(textoLabel);



        public REGISTRADORA_DE_COMPRAS()
        {
            InitializeComponent();
            lbl_precio.Text = "0";
        }





        private void Btn_Total_Click(object sender, EventArgs e)
        {
            // 1. Validaciones iniciales y captura de datos
            string metodo;
            int metodoDePago;

            MetodoDePagoNegocio negocio = new MetodoDePagoNegocio();
            VentaNegocio venta = new VentaNegocio();
            Metodo_de_pago metodopago = new Metodo_de_pago();

            metodo = cbx_metododepago.SelectedItem != null ? cbx_metododepago.SelectedItem.ToString() : "";

            if (string.IsNullOrEmpty(metodo))
            {
                MessageBox.Show("Seleccione un método de pago", "Atención");
                return;
            }

            // 2. Cálculos de importes
            // Guardamos el subtotal original (suma de productos sin recargos)
            decimal subtotalReal = total_acumulado;

            // Obtenemos el ID del método
            metodoDePago = negocio.IdDelMetodo(metodo);

            // Calculamos el total final con el recargo/descuento del método elegido
            // Usamos una variable local 'totalFinal' para no sobreescribir 'total_acumulado' aún
            decimal totalFinal = negocio.Elegir_metodopago(total_acumulado, metodo);

            try
            {
                // 3. Apertura del formulario de cobro
                // Pasamos: Total con recargo, Subtotal puro, Lista, ID método y Nombre método
                TOTAL_A_COBRAR totalForm = new TOTAL_A_COBRAR(totalFinal, subtotalReal, listaArticulos, metodoDePago, metodo);

                // Ejecutamos el formulario y esperamos la respuesta
                DialogResult respuesta = totalForm.ShowDialog();

                // 4. Lógica de limpieza condicionada
                if (respuesta == DialogResult.OK)
                {
                    // SI SE COMPLETÓ LA VENTA: Limpiamos todo
                    total_acumulado = 0;
                    lbl_precio.Text = "0";
                    listaArticulos.Clear();
                    DgvRegistradora.DataSource = null;
                    cbx_metododepago.SelectedIndex = 0;
                    codigo_Producto.Focus();

                   // MessageBox.Show("Venta finalizada con éxito.", "Listo");
                }
                else
                {
                    // SI CERRÓ O CANCELÓ: No limpiamos la lista para que pueda corregir algo
                   // MessageBox.Show("Venta no confirmada. Puede modificar los datos o el método de pago.", "Aviso");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al procesar la venta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private bool Procesando_Codigo = false;

        private void codigo_Producto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string entrada = codigo_Producto.Text.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    // Enter en textbox vacío → finalizar compra
                    Btn_Total.PerformClick();
                    e.Handled = true;
                    return; // Salimos para evitar procesar el resto
                }

                // --- LÓGICA DE DETECCIÓN DE MULTIPLICADOR (*) ---
                /* if (entrada.EndsWith("*"))
                 {
                     // Intentamos extraer el número antes del asterisco
                     string parteNumerica = entrada.Substring(0, entrada.Length - 1);

                     if (int.TryParse(parteNumerica, out int nuevaCantidad) && nuevaCantidad > 0)
                     {
                         cantidadA_Agregar = nuevaCantidad;
                         lblAvisoCantidad.Text = $"X {cantidadA_Agregar}";
                         lblAvisoCantidad.Visible = true;

                         codigo_Producto.Clear();
                         e.Handled = true;
                         return; // Importante: Detenemos aquí, no buscamos producto
                     }
                 }*/

                // --- PROCESAMIENTO NORMAL DE PRODUCTO ---
                try
                {
                    ArticuloNegocio negocio = new ArticuloNegocio();
                    Articulos articulo = negocio.Obtener_Por_Codigo(entrada, true).FirstOrDefault();

                    if (articulo == null)
                    {
                        DialogResult resultado = MessageBox.Show("Artículo no encontrado, ¿le gustaría agregarlo?",
                            "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (resultado == DialogResult.Yes)
                        {
                            AltaArticulo alta = new AltaArticulo();
                            alta.ShowDialog();
                            // cargar(); // Si es necesario refrescar datos
                            codigo_Producto.Clear();
                            codigo_Producto.Focus();
                        }
                    }
                    else
                    {
                        // Agregamos el artículo tantas veces como diga el multiplicador
                        for (int i = 0; i < cantidadA_Agregar; i++)
                        {
                            // Nota: Si usas la misma instancia, asegúrate de que no cause problemas de referencia
                            // Si Articulos es una clase simple, podrías necesitar clonarla o crear una nueva instancia
                            listaArticulos.Add(articulo);
                            total_acumulado += articulo.Precio;
                        }

                        // Refrescamos UI
                        DgvRegistradora.DataSource = null;
                        DgvRegistradora.DataSource = listaArticulos;
                        ocultarColumnas();

                        if (DgvRegistradora.Columns.Contains("Precio"))
                            DgvRegistradora.Columns["Precio"].DefaultCellStyle.Format = "N2";

                        DgvRegistradora.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        lbl_precio.Text = total_acumulado.ToString("C");

                        // --- RESET DE ESTADO ---
                        cantidadA_Agregar = 1;
                        lblAvisoCantidad.Visible = false;

                        codigo_Producto.Clear();
                        codigo_Producto.Focus();
                        if (DgvRegistradora.Rows.Count > 0)
                        {
                            int ultimaFila = DgvRegistradora.Rows.Count - 1;
                            DgvRegistradora.Rows[ultimaFila].Selected = true;

                            DataGridViewCell primeraCeldaVisible = DgvRegistradora.Rows[ultimaFila]
                           .Cells.Cast<DataGridViewCell>()
                           .FirstOrDefault(c => c.Visible);

                            if (primeraCeldaVisible != null)
                                DgvRegistradora.CurrentCell = primeraCeldaVisible;
                        }
                    }



                    e.Handled = true;
                }


                catch (Exception ex)
                {
                    MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                

            }

            else if (e.KeyCode == Keys.F && e.Control) // ← AGREGÁ ESTO
            {
                BtnFiados_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            // Agregá esto en el KeyDown de codigo_Producto, junto a los otros else if:

            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                if (DgvRegistradora.Rows.Count == 0) return;

                int filaActual = DgvRegistradora.CurrentRow?.Index ?? -1;

                if (e.KeyCode == Keys.Down)
                {
                    int siguienteFila = Math.Min(filaActual + 1, DgvRegistradora.Rows.Count - 1);
                    DgvRegistradora.Rows[siguienteFila].Selected = true;

                    DataGridViewCell celdaVisible = DgvRegistradora.Rows[siguienteFila]
                        .Cells.Cast<DataGridViewCell>()
                        .FirstOrDefault(c => c.Visible);

                    if (celdaVisible != null)
                        DgvRegistradora.CurrentCell = celdaVisible;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    int filaAnterior = Math.Max(filaActual - 1, 0);
                    DgvRegistradora.Rows[filaAnterior].Selected = true;

                    DataGridViewCell celdaVisible = DgvRegistradora.Rows[filaAnterior]
                        .Cells.Cast<DataGridViewCell>()
                        .FirstOrDefault(c => c.Visible);

                    if (celdaVisible != null)
                        DgvRegistradora.CurrentCell = celdaVisible;
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Delete)
            {

                btnEliminarArticulo_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;

            }

            else if (e.KeyCode == Keys.Escape) // ← AGREGÁ ESTO
            {
                this.Close();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            else if (e.KeyCode == Keys.Right)
            {
                if (cbx_metododepago.SelectedIndex < cbx_metododepago.Items.Count - 1)
                    cbx_metododepago.SelectedIndex++;

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (cbx_metododepago.SelectedIndex > 0)
                    cbx_metododepago.SelectedIndex--;

                e.Handled = true;
                e.SuppressKeyPress = true;
            }

        }



        private void REGISTRADORA_DE_COMPRAS_Load(object sender, EventArgs e)
        {
            MetodoDePagoNegocio negocio = new MetodoDePagoNegocio();
            var lista = negocio.Listametodo();
            codigo_Producto.Select();
           


            if (lista != null && lista.Count > 0)
            {
                cbx_metododepago.DataSource = lista;
                cbx_metododepago.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No hay métodos de pago cargados.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ocultarColumnas()
        {
            

            // Verifica si hay columnas en el DataGridView antes de intentar acceder a ellas
            if (DgvRegistradora.Columns.Count > 0)
            {
                // Solo intenta ocultar si la columna existe para evitar errores
                if (DgvRegistradora.Columns.Contains("URL"))
                {
                    DgvRegistradora.Columns["URL"].Visible = false;
                }
                if (DgvRegistradora.Columns.Contains("Id"))
                {
                    DgvRegistradora.Columns["Id"].Visible = false;
                }

                if (DgvRegistradora.Columns.Contains("metodoDePago"))
                {
                    DgvRegistradora.Columns["metodoDePago"].Visible = false;
                }
                if (DgvRegistradora.Columns.Contains("Categoria"))
                {
                    DgvRegistradora.Columns["Categoria"].Visible = false;
                }
                if (DgvRegistradora.Columns.Contains("Stock"))
                {
                    DgvRegistradora.Columns["Stock"].Visible = false;
                }
                if (DgvRegistradora.Columns.Contains("codigo"))
                {
                    DgvRegistradora.Columns["codigo"].Visible = false;
                }
                if (DgvRegistradora.Columns.Contains("PrecioCosto"))
                {
                    DgvRegistradora.Columns["PrecioCosto"].Visible = false;
                }


                DgvRegistradora.Columns["Marca"].Visible = false;


            }
        }

        private void btnEliminarArticulo_Click(object sender, EventArgs e)
        {
            if (DgvRegistradora.Rows.Count == 0 || DgvRegistradora.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione un artículo para eliminar.");
                codigo_Producto.Focus();
                return;
            }

            try
            {
                // Usamos CurrentRow directamente ya que es lo que controlamos con las flechas
                Articulos seleccionado = (Articulos)DgvRegistradora.CurrentRow.DataBoundItem;

                if (seleccionado == null) return;

               
                    total_acumulado -= seleccionado.Precio;
                    listaArticulos.Remove(seleccionado);
                

                if (total_acumulado < 0) total_acumulado = 0;
                lbl_precio.Text = total_acumulado.ToString("C");
                cargar();
                if (DgvRegistradora.Rows.Count > 0)
                {
                    int ultimaFila = DgvRegistradora.Rows.Count - 1;
                    DgvRegistradora.Rows[ultimaFila].Selected = true;

                    DataGridViewCell primeraCeldaVisible = DgvRegistradora.Rows[ultimaFila]
                   .Cells.Cast<DataGridViewCell>()
                   .FirstOrDefault(c => c.Visible);

                    if (primeraCeldaVisible != null)
                        DgvRegistradora.CurrentCell = primeraCeldaVisible;
                }
                codigo_Producto.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }




        private void cargar()
        {

            DgvRegistradora.DataSource = null;
            DgvRegistradora.DataSource = listaArticulos;

            ocultarColumnas();
            if (DgvRegistradora.Rows.Count > 0)
            {
                DgvRegistradora.ClearSelection();
                DgvRegistradora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }



        }

        private void cbx_metododepago_SelectedIndexChanged(object sender, EventArgs e)
        {
            MetodoDePagoNegocio negocio = new MetodoDePagoNegocio();
            decimal porcentaje = negocio.ObtenerPorcentaje(cbx_metododepago.Text);

            // Resultado: "Tarjeta - Recargo: 10%"
            LblRecargo.Text = $"{cbx_metododepago.Text} - Recargo: {porcentaje}%";
            codigo_Producto.Focus();

        }

        private void BtnFiados_Click(object sender, EventArgs e)
        {
            ListaDeudores lista = new ListaDeudores();
            lista.ShowDialog();
            codigo_Producto.Select();
        }

        private void codigo_Producto_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Detectamos si el usuario presionó la tecla asterisco '*'
            if (e.KeyChar == '*')
            {
                string entrada = codigo_Producto.Text.Trim();

                // Verificamos que haya un número antes del asterisco
                if (!string.IsNullOrEmpty(entrada) && int.TryParse(entrada, out int nuevaCantidad) && nuevaCantidad > 0)
                {
                    cantidadA_Agregar = nuevaCantidad;
                    lblAvisoCantidad.Text = $"X {cantidadA_Agregar}";
                    lblAvisoCantidad.Visible = true;

                    codigo_Producto.Clear();
                    e.Handled = true; // Esto evita que el '*' se escriba en el cuadro de texto
                }
            }
        }

        private void DgvRegistradora_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
