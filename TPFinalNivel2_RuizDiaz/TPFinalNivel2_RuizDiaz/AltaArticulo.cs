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
using Dominio;
using System.IO;
using System.Configuration;
using Negocio_;

namespace TPFinalNivel2_RuizDiaz
{
    public partial class AltaArticulo : Form
    {
        private Dominio.Articulos articulo = null;
        private OpenFileDialog archivo = null;

        public AltaArticulo()
        {
            InitializeComponent();
            this.KeyPreview = true;
            txtCodigo.Multiline = false;
            txtCodigo.AcceptsReturn = false;

            // Asignar eventos
            txtCodigo.KeyDown += txtCodigo_KeyDown_1;
             txtNombre.KeyDown += AltaArticulo_KeyDown;
            txtDescripcion.KeyDown += AltaArticulo_KeyDown;
            cbxMarcas.KeyDown += AltaArticulo_KeyDown;
            cbxCategoria.KeyDown += AltaArticulo_KeyDown;
            PrecioCostoTxt.KeyDown += AltaArticulo_KeyDown;
            txtPrecio.KeyDown += AltaArticulo_KeyDown;
            txtStock.KeyDown += AltaArticulo_KeyDown;
        }

        public AltaArticulo(string codigo) : this()
        {
            txtCodigo.Text = codigo;

        }



            public AltaArticulo(Dominio.Articulos articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (!CamposValidos())
                {
                    MessageBox.Show("Complete todos los campos requeridos.");
                    return;
                }

                if (cbxMarcas.SelectedIndex == -1)
                {
                    MessageBox.Show("Debes seleccionar una marca válida de la lista.");
                    return;
                }

                if (cbxCategoria.SelectedIndex == -1)
                {
                    MessageBox.Show("Debes seleccionar una categoría válida de la lista.");
                    return;
                }

                if (articulo == null) articulo = new Dominio.Articulos();

                // RELLENAR TEXTOS Y NÚMEROS
                articulo.codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.PrecioCosto = decimal.Parse(PrecioCostoTxt.Text);
                articulo.Stock = int.Parse(txtStock.Text);

                // --- EL PUNTO CRÍTICO: ASIGNAR LOS OBJETOS DE LOS COMBOS ---
                // Debemos castear el SelectedItem al objeto correspondiente
                articulo.Marca = (Marca)cbxMarcas.SelectedItem;
                articulo.Categoria = (categoria)cbxCategoria.SelectedItem;

                if (articulo.Id != 0)
                {
                    if (MessageBox.Show("¿Desea Modificar?", "Atención", MessageBoxButtons.YesNo) == DialogResult.No) return;
                    negocio.Modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    if (MessageBox.Show("¿Desea Agregar?", "Atención", MessageBoxButtons.YesNo) == DialogResult.No) return;
                    negocio.Agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }

                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }



        private void AltaArticulo_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categorianegocio = new CategoriaNegocio();
            MarcasNegocio marcasnegocio = new MarcasNegocio();
            if (cbxMarcas.Items.Count > 0)
            {
                cbxMarcas.SelectedIndex = 0; // Selecciona el primer ítem por defecto
            }
            if (cbxCategoria.Items.Count > 0)
            {
                cbxCategoria.SelectedIndex = 0; // Selecciona el primer ítem por defecto
            }

            try
            {
                // 1. Configuramos el "mapeo" primero (Indispensable)
                cbxCategoria.ValueMember = "Id";
                cbxCategoria.DisplayMember = "Descripcion";
                cbxMarcas.ValueMember = "Id";
                cbxMarcas.DisplayMember = "Descripcion";

                // 2. Cargamos los datos
                cbxCategoria.DataSource = categorianegocio.listarCategoria();
                cbxMarcas.DataSource = marcasnegocio.listarmarca();

                if (articulo != null)
                {
                    // Carga de campos de texto
                    txtCodigo.Text = articulo.codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtPrecio.Text = articulo.Precio.ToString();
                    PrecioCostoTxt.Text = articulo.PrecioCosto.ToString();
                    txtStock.Text = articulo.Stock.ToString();

                    // 3. LA SOLUCIÓN PRO: Forzamos el hilo de ejecución
                    // Esto permite que el DataSource se "asiente" antes de elegir el valor
                    Application.DoEvents();

                    if (articulo.Categoria != null)
                        cbxCategoria.SelectedValue = articulo.Categoria.Id;

                    if (articulo.Marca != null)
                        cbxMarcas.SelectedValue = articulo.Marca.Id;

                    // 4. Doble verificación (si SelectedValue falló por el timing de WinForms)
                    if (cbxMarcas.SelectedIndex == -1 && articulo.Marca != null)
                        cbxMarcas.Text = articulo.Marca.Descripcion;

                    if (cbxCategoria.SelectedIndex == -1 && articulo.Categoria != null)
                        cbxCategoria.Text = articulo.Categoria.Descripcion;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar: " + ex.Message);
            }
        }




        private void ResetearColores()
        {
            txtCodigo.BackColor = SystemColors.Window;
            txtNombre.BackColor = SystemColors.Window;
            txtDescripcion.BackColor = SystemColors.Window;
            txtPrecio.BackColor = SystemColors.Window;
            PrecioCostoTxt.BackColor = SystemColors.Window;
        }

        private bool CamposValidos()
        {
            bool camposValidos = true;

            // Validación de campos de texto
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                txtCodigo.BackColor = Color.Firebrick;
                camposValidos = false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                txtNombre.BackColor = Color.Firebrick;
                camposValidos = false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                txtDescripcion.BackColor = Color.Firebrick;
                camposValidos = false;
            }

            // Validación numérica con TryParse
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0 )
            {
                txtPrecio.BackColor = Color.Firebrick;
                camposValidos = false;
            }

            // Validación de comboboxes
            if (cbxMarcas.SelectedItem == null)
            {
                cbxMarcas.BackColor = Color.Firebrick;
                camposValidos = false;
            }


            if (cbxCategoria.SelectedItem == null)
            {
                cbxCategoria.BackColor = Color.Firebrick;
                camposValidos = false;
            }

            if (string.IsNullOrWhiteSpace(txtStock.Text) || !txtStock.Text.All(char.IsDigit))
            {
                txtStock.BackColor = Color.Firebrick;
                camposValidos = false;
            }

            if (!decimal.TryParse(PrecioCostoTxt.Text, out decimal PrecioCosto) || PrecioCosto <= 0)
            {
                PrecioCostoTxt.BackColor = Color.Firebrick;
                camposValidos = false;
            }

            

            return camposValidos;
        }


         private void AltaArticulo_KeyDown(object sender, KeyEventArgs e)
        {
          if (e.KeyCode == Keys.Enter)
        {
          this.SelectNextControl((Control)sender, true, true, true, true);
        e.Handled = true;
        e.SuppressKeyPress = true;
        }
        }




        private void BtnMarcas_Click(object sender, EventArgs e)
        {
            MarcasNegocio marcasnegocio = new MarcasNegocio();
            // Guardamos lo que estaba seleccionado
            var idActual = cbxMarcas.SelectedValue;
            cbxMarcas.DataSource = marcasnegocio.listarmarca();

            AltaMarcas formulario = new AltaMarcas();
            formulario.ShowDialog();

            // Recargamos
            cbxMarcas.DataSource = marcasnegocio.listarmarca();
            // Restauramos la selección
             cbxMarcas.SelectedValue = idActual;
        }

        private void BtnCategorias_Click(object sender, EventArgs e)
        {
            CategoriaNegocio categorianegocio = new CategoriaNegocio();

            // 1. Guardamos el ID de la categoría que está seleccionada actualmente
            // Lo guardamos en una variable para que no se pierda al recargar la lista
            var idActual = cbxCategoria.SelectedValue;

            altaCategorias formulario = new altaCategorias();
            formulario.ShowDialog();

            // 2. Refrescamos la lista desde la base de datos 
            // (Por si el usuario agregó una categoría nueva en la otra ventana)
            cbxCategoria.DataSource = categorianegocio.listarCategoria();

            // 3. Restauramos la selección original
            // Gracias al método Equals que agregamos a la clase, esto funcionará perfecto
            if (idActual != null)
            {
                cbxCategoria.SelectedValue = idActual;
            }
        }

        private void txtCodigo_KeyDown_1(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Enter)
            {
                ArticuloNegocio negocio = new ArticuloNegocio(); // se inicia una instancia ArticuloNegocio
                bool existe = negocio.Obtener_Por_Codigo_Altas(txtCodigo.Text.Trim()); // se hace una variable booleana como bandera para saber si existe el código

                if (existe)
                {
                    txtCodigo.BackColor = Color.Red; // si existe el backcolor de txtcodigo va a ser rojo

                    DialogResult result = MessageBox.Show( //va a mandar una leyenda que me va a avisar si esta cargado el codigo y me va a dar la opcion de aceptar
                        "El código ya está cargado",
                        "Aviso",
                        MessageBoxButtons.OK,   // solo muestra botón Aceptar
                        MessageBoxIcon.Warning);
                        txtCodigo.Focus();



                    if (result == DialogResult.OK) // si me da okey tendría que pasar lo siguiente = 
                    {
                        txtCodigo.Text = "";                // limpia el textbox
                        txtCodigo.BackColor = Color.White; // opcional: vuelve a color normal
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            txtCodigo.Focus();
                            txtCodigo.Select();
                        });               // vuelve el foco
                    }

                   e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }

                else
                {
                    
                    this.SelectNextControl((Control)sender, true, true, true, true);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;

                }
            }
        }

        private void cbxMarcas_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbxMarcas.Text))
            {
                if (cbxMarcas.Items.Count > 0)
                {
                    cbxMarcas.SelectedIndex = 0;
                    // Al asignar un índice, ya no es necesario cancelar el evento (e.Cancel = false)
                }
            }
        }

        private void cbxCategoria_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbxCategoria.Text))
            {
                if (cbxCategoria.Items.Count > 0)
                {
                    cbxCategoria.SelectedIndex = 0;
                    // Al asignar un índice, ya no es necesario cancelar el evento (e.Cancel = false)
                }
            }
        }
    }

}
    

