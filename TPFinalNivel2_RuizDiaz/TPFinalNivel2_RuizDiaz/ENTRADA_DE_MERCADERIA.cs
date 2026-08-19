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
using System.Globalization; // Necesario para CultureInfo


namespace TPFinalNivel2_RuizDiaz
{
    public partial class ENTRADA_DE_MERCADERIA : Form
    {
        string nombreProveedor;
        int idProveedor;
        private List<MetodoDePagoNegocio> lista = new List<MetodoDePagoNegocio>();
        

       
        void cargarcombo()
        {
            MetodoDePagoNegocio negocio = new MetodoDePagoNegocio();
            var lista = negocio.Listametodo();
           
        }

       

       
        decimal TotalCompra;

        // Esta es tu lista dinámica en memoria
        private List<DetalleCompra> listaDetalle = new List<DetalleCompra>();

        public ENTRADA_DE_MERCADERIA(string nombre, int id)
        {
            InitializeComponent();
            nombreProveedor = nombre;
            idProveedor = id;
            TotalCompra = 0;
        }

        int cantidadagregar;

        private void CodigoProdcutotxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Bloqueamos el "beep" de Windows para cualquier caso de Enter
                e.Handled = true;
                e.SuppressKeyPress = true;

                try
                {
                    ArticuloNegocio negocio = new ArticuloNegocio();
                    Articulos articulo = negocio.Obtener_Por_Codigo(CodigoProdcutotxt.Text, false).FirstOrDefault();

                    if (articulo != null)
                    {
                        // Usamos el operador ternario para poner un texto por defecto si la propiedad es nula
                        string nombre = !string.IsNullOrEmpty(articulo.Nombre) ? articulo.Nombre : "Sin Nombre";
                        string descripcion = !string.IsNullOrEmpty(articulo.Descripcion) ? articulo.Descripcion : "Sin Descripción";
                        string stock = articulo.Stock > 0 ? articulo.Stock.ToString() : "Sin stock";

                        // Para el Precio Costo (asumiendo que es decimal)
                        // Corregido: antes decía articulo.Stock al final
                        string preciocosto = articulo.PrecioCosto > 0 ? articulo.PrecioCosto.ToString("N2") : "Sin precio costo";
                        string precioventa = articulo.Precio > 0 ? articulo.Precio.ToString("N2") : "Sin precio venta";



                        Lbl_Producto.Text = $"{nombre}  {descripcion} - stock: {stock} \n" + // Agregamos \n al final del primer renglón
                    $"Precio Costo: {preciocosto} \n " +
                    $"Precio Venta: {precioventa}";
                        
                        Cantidad_Stock_Ingreso.Focus();
                    }
                    else // Es lo mismo que if (articulo == null)
                    {
                        if (string.IsNullOrWhiteSpace(CodigoProdcutotxt.Text))
                        {
                            MessageBox.Show("ESCRIBA UN CÓDIGO");
                            return;
                        }

                        DialogResult resultado = MessageBox.Show("Artículo no encontrado, ¿le gustaría agregarlo?",
                            "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (resultado == DialogResult.Yes)
                        {
                            AltaArticulo alta = new AltaArticulo(CodigoProdcutotxt.Text);

                            // Solo entramos al IF si el usuario hizo clic en "Aceptar" y se guardó
                            if (alta.ShowDialog() == DialogResult.OK)
                            {
                                MessageBox.Show("El Articulo ha sido ingresado en éxito");
                                LimpiarFormulario();
                            }
                        }
                    }
                }





                catch (Exception ex)
                {
                    MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void RestaurarColores()
        {
            Cantidad_Stock_Ingreso.BackColor = Color.White;
            PrecioCosto_txt.BackColor = Color.White;
            txt_Precio_Venta.BackColor = Color.White;
        }
        private void ActualizarGrilla()
        {

            dgvListaCompra.DataSource = null;
            dgvListaCompra.DataSource = listaDetalle;

            // 1. El Grid debe permitir edición a nivel general
            dgvListaCompra.ReadOnly = false;

            // 2. Bloqueamos todas las columnas por defecto (opcional, pero seguro)
            foreach (DataGridViewColumn col in dgvListaCompra.Columns)
            {
                col.ReadOnly = true;
            }

            // 3. Habilitamos SOLO las que queremos editar
            // Usamos el nombre exacto de la propiedad de tu clase DetalleCompra
            if (dgvListaCompra.Columns.Contains("PrecioCosto"))
            {
                dgvListaCompra.Columns["PrecioCosto"].ReadOnly = false;
            }

            if (dgvListaCompra.Columns.Contains("PrecioVenta")) // Cambié "Precio" por "PrecioVenta"
            {
                dgvListaCompra.Columns["PrecioVenta"].ReadOnly = false;
            }
            dgvListaCompra.EditMode = DataGridViewEditMode.EditOnEnter;

            // Estilo visual para que el usuario sepa cuál puede editar
            dgvListaCompra.Columns["PrecioCosto"].DefaultCellStyle.BackColor = Color.LightYellow;
            dgvListaCompra.Columns["PrecioVenta"].DefaultCellStyle.BackColor = Color.LightBlue;

            ocultarColumnas();
        }

        private void Btn_Ingresar_Stock_Click(object sender, EventArgs e)
        {
            // 1. Validar
            if (!Campos_Validos()) return;

            // 2. Ejecutar Lógica
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();

                // Ya no necesitamos TryParse acá porque ValidarFormulario garantizó que son correctos
                int cant = int.Parse(Cantidad_Stock_Ingreso.Text);
                decimal costo = decimal.Parse(PrecioCosto_txt.Text);
                decimal venta = decimal.Parse(txt_Precio_Venta.Text);

                negocio.SumarStock(cant, CodigoProdcutotxt.Text, costo, venta);
                DetalleCompra item = new DetalleCompra();
                
                item.IdProducto = CodigoProdcutotxt.Text;
                item.Cantidad = int.Parse(Cantidad_Stock_Ingreso.Text);

                // 3. CARGAR LOS DATOS DESDE LA DB (Para que se vean en la grilla)
                item.PrecioCosto = Math.Round(negocio.ObtenerPrecioCostoPorId(CodigoProdcutotxt.Text), 2);
                item.Monto = Math.Round(item.Cantidad * item.PrecioCosto, 2);
                item.NombreProducto = negocio.ObtenerNombrePorId(CodigoProdcutotxt.Text); // DEBES CREAR ESTE MÉTODO

                // 4. Agregar y refrescar
                listaDetalle.Add(item);
                ActualizarGrilla();
                CalcularTotalCompra();
                MessageBox.Show("¡precio actualizados y añadido a la lista de compras!", "Éxito");
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LimpiarFormulario()
        {
            CodigoProdcutotxt.Clear();
            Cantidad_Stock_Ingreso.Clear();
            PrecioCosto_txt.Clear();
            txt_Precio_Venta.Clear();
            Lbl_Producto.Text = "-"; // O el nombre de tu label de descripción
            checkBox1.Checked = false;
            // Ponemos el foco de nuevo en el código para el próximo escaneo
            CodigoProdcutotxt.Focus();
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("¿Está seguro que desea cancelar el ingreso y limpiar los campos?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {

                LimpiarFormulario();
            }
        }





        private void button1_Click(object sender, EventArgs e)
        {
            if (!Campos_Valido_Cantidad()) return;

            try
            {
               

                
                    if (!string.IsNullOrWhiteSpace(CodigoProdcutotxt.Text) && !string.IsNullOrWhiteSpace(Cantidad_Stock_Ingreso.Text))
                    {

                        ArticuloNegocio negocio = new ArticuloNegocio();

                        if (int.TryParse(Cantidad_Stock_Ingreso.Text, out int cant))
                        {

                            cantidadagregar = cant;
                            DetalleCompra item = new DetalleCompra();
                            item.IdProducto = CodigoProdcutotxt.Text; 
                            item.Cantidad = int.Parse(Cantidad_Stock_Ingreso.Text);
                            item.PrecioVenta = Math.Round(negocio.ObtenerPrecioVentaPorCodigo(CodigoProdcutotxt.Text), 2);
                            item.PrecioCosto = Math.Round(negocio.ObtenerPrecioCostoPorId(CodigoProdcutotxt.Text), 2);
                            item.Monto = Math.Round(item.Cantidad * item.PrecioCosto, 2);
                            item.NombreProducto = negocio.ObtenerNombrePorId(CodigoProdcutotxt.Text);                           
                            listaDetalle.Add(item);
                            ActualizarGrilla();
                            CalcularTotalCompra();
                            LimpiarFormulario();
                        }
                    }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }

        private void ocultarColumnas()
        {

            dgvListaCompra.Columns["IdProducto"].Visible = false;

        }


        private void Cantidad_Stock_Ingreso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                button1_Click(sender, e);
            }
        }

        private void PrecioCosto_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_Precio_Venta.Focus();
            }
        }

        private bool Campos_Validos()
        {
            bool esValido = true;
            errorProvider1.Clear();

            // 1. Seguridad en Cantidad (Enteros)
            if (!int.TryParse(Cantidad_Stock_Ingreso.Text, out int cant) || cant <= 0)
            {
                errorProvider1.SetError(Cantidad_Stock_Ingreso, "La cantidad debe ser un número entero mayor a cero.");
                esValido = false;
            }

            // 2. Seguridad en Precios (Usando el método del punto 1)
            if (!ValidarDecimal(PrecioCosto_txt.Text, out decimal costo) || costo <= 0)
            {
                errorProvider1.SetError(PrecioCosto_txt, "Precio de costo inválido (use punto o coma para decimales).");
                esValido = false;
            }

            if (!ValidarDecimal(txt_Precio_Venta.Text, out decimal venta) || venta <= 0)
            {
                errorProvider1.SetError(txt_Precio_Venta, "Precio de venta inválido.");
                esValido = false;
            }

            return esValido;
        }

        private bool Campos_Valido_Cantidad()
        {
            bool esValido = true;

            // 1. Limpiar errores anteriores
            errorProvider1.Clear();

            // 2. Validar Cantidad
            if (!int.TryParse(Cantidad_Stock_Ingreso.Text, out int cant) || cant <= 0)
            {
                errorProvider1.SetError(Cantidad_Stock_Ingreso, "Ingrese una cantidad válida mayor a cero.");
                esValido = false;
            }

            return esValido;



        }

        private bool Validar_PlataProveedor()
        {
            bool esValido = true;
            errorProvider2.Clear();



            // 2. Validar el PAGO AL PROVEEDOR
            if (!string.IsNullOrWhiteSpace(TxtPagoAProveedor.Text)) {
                if (!decimal.TryParse(TxtPagoAProveedor.Text, out decimal pago))
                {
                    errorProvider2.SetError(TxtPagoAProveedor, "Ingrese un monto de pago válido .");
                    esValido = false;
                }
            }
            



            // Nota: Si solo estás finalizando la compra, no necesitas validar precios de venta aquí
            // a menos que el checkbox de "Actualizar Precio" esté marcado.

            return esValido;
        }

        private void txt_Precio_Venta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }


        private bool ValidarDecimal(string texto, out decimal resultado)
        {
            // Permite tanto punto como coma, convirtiéndolo a un formato estándar
            return decimal.TryParse(texto.Replace(',', '.'),
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out resultado);
        }

        private void AumentarPorPorcentajeOCifra_Click(object sender, EventArgs e)
        {
            // Validación 1: campos vacíos → cortamos con return
            if (string.IsNullOrWhiteSpace(CodigoProdcutotxt.Text) || string.IsNullOrWhiteSpace(Cantidad_Stock_Ingreso.Text))
            {
                MessageBox.Show("NO INGRESÓ EL CÓDIGO O LA CANTIDAD DEL PRODUCTO");
                return; // ← IMPORTANTE: corta acá
            }

            // Validación 2: cantidad no es número → cortamos con return
            if (!int.TryParse(Cantidad_Stock_Ingreso.Text, out int resultado))
            {
                MessageBox.Show("Por favor, ingresa un número válido.");
                return; // ← IMPORTANTE: corta acá
            }

            // Recién acá abrimos el formulario
            try
            {
                Cambiar_Precio_Por_Marca formulario = new Cambiar_Precio_Por_Marca(
                    Cambiar_Precio_Por_Marca.ModoAumento.Producto, CodigoProdcutotxt.Text, resultado);

                if (formulario.ShowDialog() == DialogResult.OK)
                {
                    ArticuloNegocio negocio = new ArticuloNegocio();
                    DetalleCompra item = new DetalleCompra();
                    item.IdProducto = CodigoProdcutotxt.Text;
                    item.Cantidad = resultado; // ya lo tenés parseado, no uses int.Parse de nuevo
                    item.PrecioVenta = Math.Round(negocio.ObtenerPrecioVentaPorCodigo(CodigoProdcutotxt.Text), 2); // ← agregás esto
                    item.PrecioCosto = Math.Round(negocio.ObtenerPrecioCostoPorId(CodigoProdcutotxt.Text), 2);
                    item.Monto = Math.Round(item.Cantidad * item.PrecioCosto, 2);
                    item.NombreProducto = negocio.ObtenerNombrePorId(CodigoProdcutotxt.Text);
                    listaDetalle.Add(item);
                    ActualizarGrilla();
                    CalcularTotalCompra();
                    LimpiarFormulario();
                }
                // Si el usuario cerró con X → DialogResult.Cancel → no entra al if ✓
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void CalcularTotalCompra()
        {
            decimal acumulado = 0;

            // Recorremos cada objeto de la lista dinámica
            foreach (var item in listaDetalle)
            {
                // Multiplicamos el costo por la cantidad de cada renglón
                acumulado += (item.PrecioCosto * item.Cantidad);
            }

            // 1. Guardamos el valor en una variable global si la usas para SQL
            this.TotalCompra = acumulado;

            // 2. Lo mostramos en el Label (con formato de moneda)
            lblDeuda.Text = acumulado.ToString("N2");
        }

        private void CboPrecioTeclado_CheckedChanged(object sender, EventArgs e)
        {
            bool estaMarcado = CboPrecioTeclado.Checked;

            // 1. Manejo de visibilidad (lo que ya tenías)
            label4.Visible = estaMarcado;
            label5.Visible = estaMarcado;
            PrecioCosto_txt.Visible = estaMarcado;
            txt_Precio_Venta.Visible = estaMarcado;
            Btn_Ingresar_Stock.Visible = estaMarcado;
            BtnSoloCantidad.Visible = estaMarcado;

            groupBox2.Visible = !estaMarcado;
            groupBox3.Visible = !estaMarcado;
            checkBox1.Visible = !estaMarcado;
            // 2. LA MAGIA: Si el usuario desmarca el cuadro, limpiamos los precios
            if (!estaMarcado)
            {
                PrecioCosto_txt.Clear();
                txt_Precio_Venta.Clear();

                // Opcional: Si querés que el foco vuelva a la cantidad para seguir cargando
                Cantidad_Stock_Ingreso.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("¿Está seguro que desea cancelar el ingreso y limpiar los campos?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {

                LimpiarFormulario();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("¿Está seguro que desea cancelar el ingreso y limpiar los campos?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {

                LimpiarFormulario();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool estaMarcado = checkBox1.Checked;


            groupBox3.Enabled = false;
            groupBox2.Enabled = true;

            if (!estaMarcado)
            {
                groupBox3.Enabled = true;
                groupBox2.Enabled = false;
            }
        }

        private void ENTRADA_DE_MERCADERIA_Load(object sender, EventArgs e)
        {
            
            lblProveedor.Text = nombreProveedor;
            cargarcombo();
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            // Validamos que haya algo para guardar
            if (listaDetalle.Count == 0)
            {
                MessageBox.Show("La lista está vacía. Cargue al menos un producto.");
                return;
            }

            if (!Validar_PlataProveedor())
            {
                return;
            }

           /* string metodo;

            metodo = cboMetodo.SelectedItem != null ? cboMetodo.SelectedItem.ToString() : "";

            if (string.IsNullOrEmpty(metodo))
            {
                MessageBox.Show("Seleccione un método de pago", "Atención");
                return;
            }*/



            ClienteNegocio negocio = new ClienteNegocio();
            DialogResult respuesta = MessageBox.Show("¿Desea finalizar el ingreso de mercadería? Esto actualizará el stock de todos los productos.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    decimal DineroAProveedor = string.IsNullOrWhiteSpace(TxtPagoAProveedor.Text)
                    ? 0
                    : decimal.Parse(TxtPagoAProveedor.Text);

                    MetodoDePagoNegocio fondo = new MetodoDePagoNegocio();

                   /* int Metodopago = fondo.IdDelMetodo(metodo); */
                    // idProveedor es el que recibiste por parámetro al abrir el formulario
                    negocio.GuardarCompraFinal(idProveedor, listaDetalle, DineroAProveedor, TotalCompra);

                    /*fondo.restarAlFondo(Metodopago, DineroAProveedor);*/

                    MessageBox.Show("¡Stock actualizado y compra registrada con éxito!", "Éxito");

                    // Limpiamos todo para una nueva carga o cerramos
                    listaDetalle.Clear();
                    ActualizarGrilla();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error crítico al finalizar: " + ex.Message);
                }
                
                
            }
        }

        private void TxtPagoAProveedor_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite números y teclas de control (como borrar)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            // Solo permite UNA coma decimal
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            
    if (dgvListaCompra.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione el producto que desea eliminar de la lista.");
                return;
            }

            // 2. Preguntar al usuario para evitar borrados accidentales
            DialogResult respuesta = MessageBox.Show("¿Está seguro de eliminar este producto de la carga actual?", "Eliminar Producto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                // 3. Obtenemos el objeto 'DetalleCompra' que está vinculado a esa fila
                // DataBoundItem es la forma más limpia de obtener el objeto real de la lista
                DetalleCompra seleccionado = (DetalleCompra)dgvListaCompra.CurrentRow.DataBoundItem;

                // 4. Lo eliminamos de la lista dinámica (la BindingList o List que estés usando)
                listaDetalle.Remove(seleccionado);

                // 5. Refrescamos la visualización
                ActualizarGrilla();

                // Opcional: Recalcular el total si tenés un label que sume todo
                CalcularTotalCompra();
            }
        }

        private void dgvListaCompra_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string columna = dgvListaCompra.Columns[e.ColumnIndex].Name;
            if (columna != "PrecioCosto" && columna != "PrecioVenta") return;
            try
            {
                decimal precioCosto = Convert.ToDecimal(dgvListaCompra.Rows[e.RowIndex].Cells["PrecioCosto"].Value);
                decimal cantidad = Convert.ToDecimal(dgvListaCompra.Rows[e.RowIndex].Cells["Cantidad"].Value);
                dgvListaCompra.Rows[e.RowIndex].Cells["Monto"].Value = precioCosto * cantidad;
                RecalcularTotal();

                string idArticulo = dgvListaCompra.Rows[e.RowIndex].Cells["IdProducto"].Value.ToString();
                decimal precioVenta = Convert.ToDecimal(dgvListaCompra.Rows[e.RowIndex].Cells["PrecioVenta"].Value);

                // 1. Actualizar en ARTICULOS
                ArticuloNegocio articulo = new ArticuloNegocio();
                articulo.ModificarPrecios(idArticulo, precioCosto, precioVenta);

                // 2. Actualizar en listaDetalle en memoria y recalcular TotalCompra
                DetalleCompra itemEnMemoria = listaDetalle.FirstOrDefault(x => x.IdProducto == idArticulo);
                if (itemEnMemoria != null)
                {
                    itemEnMemoria.PrecioCosto = precioCosto;
                    itemEnMemoria.PrecioVenta = precioVenta;
                    itemEnMemoria.Monto = precioCosto * cantidad;
                }

                // 3. Recalcular TotalCompra global desde la lista actualizada
                CalcularTotalCompra();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
           

        private void RecalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvListaCompra.Rows)
            {
                if (row.IsNewRow) continue;
                if (decimal.TryParse(row.Cells["Monto"].Value?.ToString(), out decimal monto))
                    total += monto;
            }
            lblDeuda.Text = total.ToString("N2"); // o el label que uses
        }

        private void dgvListaCompra_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvListaCompra.Columns[e.ColumnIndex].Name != "PrecioCosto") return;

            string valor = e.FormattedValue?.ToString();
            if (!decimal.TryParse(valor, out decimal resultado) || resultado <= 0)
            {
                e.Cancel = true;
                dgvListaCompra.Rows[e.RowIndex].ErrorText = "Ingresá un precio válido mayor a 0";
                MessageBox.Show("El precio debe ser un número mayor a 0.", "Valor inválido",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                dgvListaCompra.Rows[e.RowIndex].ErrorText = ""; // Limpiar error
            }
        }

     
    }
    }








