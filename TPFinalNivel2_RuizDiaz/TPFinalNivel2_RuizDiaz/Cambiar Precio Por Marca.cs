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
    public partial class Cambiar_Precio_Por_Marca : Form
    {
        public enum ModoAumento { Marca, Categoria, Producto }

        private ModoAumento modoActual;

        //variable global
        int cantidadagregar;

        // Constructor por defecto (Marca)
        public Cambiar_Precio_Por_Marca()
        {
            InitializeComponent();
            modoActual = ModoAumento.Marca;
            this.Text = "Actualizar por Marca";
        }

        public Cambiar_Precio_Por_Marca(ModoAumento modo, string codigo, int cantidad)
        {
            InitializeComponent();
            this.modoActual = modo;

            // 1. PRIMERO asignamos los valores a las variables globales
            this.cantidadagregar = cantidad;

            // 2. Configuramos la interfaz
            txtCodigoBarras.Visible = true;
            txtCodigoBarras.Text = codigo;
            this.Text = "Actualizar por Código de Barras";
            label1.Text = "Código Barras";
            BtnAceptar.Text = "APLICAR AUMENTO";

            // 3. Corregimos la visibilidad (Ponemos en TRUE para que se vea)
            lblStock.Visible = true;
            lblStockEntrante.Visible = true; // <--- Cambiado de false a true

            // 4. RECIÉN AHORA buscamos el producto (ahora cantidadagregar ya vale lo que recibiste)
            BuscarProductoPorCodigo();

            CbxMarca.Visible = false;
        }

        public Cambiar_Precio_Por_Marca(ModoAumento modo)
        {
            InitializeComponent();
            this.modoActual = modo;

            if (modoActual == ModoAumento.Categoria)
            {
                this.Text = "Actualizar por Categoría";
                label1.Text = "Rubro";
            }
            else if (modoActual == ModoAumento.Producto)
            {
                this.Text = "Actualizar por Código de Barras";
                label1.Text = "Código Barras";

                // TRUCO VISUAL: Ocultamos el ComboBox y mostramos un TextBox
                CbxMarca.Visible = false;

            }
        }





        private void Cambiar_Precio_Por_Marca_Load(object sender, EventArgs e)
        {
            try
            {
                // Limpiamos cualquier rastro previo para evitar errores de binding
                CbxMarca.DataSource = null;
                lblPorcenOCifra.Text = "%";
                lblIngrese.Text = "INGRESE PORCENTAJE A SUMAR AL VALOR DE COSTO Y VENTA:";


                if (modoActual == ModoAumento.Categoria)
                {
                    CategoriaNegocio catNego = new CategoriaNegocio();
                    CbxMarca.ValueMember = "Id";
                    CbxMarca.DisplayMember = "Descripcion";
                    CbxMarca.DataSource = catNego.listarCategoria();
                }
                else if (modoActual == ModoAumento.Marca)
                {
                    MarcasNegocio Marcanegocio = new MarcasNegocio();
                    CbxMarca.ValueMember = "Id";
                    CbxMarca.DisplayMember = "Descripcion";
                    CbxMarca.DataSource = Marcanegocio.listarmarca();
                }
                else if (modoActual == ModoAumento.Producto)
                {
                    // En modo producto no necesitamos el ComboBox cargado
                    label1.Visible = false;
                    CbxMarca.Visible = false; // Lo ocultamos
                    txtCodigoBarras.Visible = true;
                    label3.Visible = true;
                    



                    // Si no quieres crear el TextBox por código, puedes arrastrar uno 
                    // al diseño, llamarlo txtCodigoBarras y ponerlo como Visible = false 
                    // por defecto, y aquí ponerlo en true.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            if (!CampoValido()) return;

            try
            {
                var cbxselect = groupBox1.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                string tipo = cbxselect.Tag.ToString();
                decimal valor = decimal.Parse(TxtPorcentajeyocifra.Text);

                if (MessageBox.Show("¿Estás seguro?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.No) return;

                if (modoActual == ModoAumento.Categoria)
                {
                    var seleccionado = (categoria)CbxMarca.SelectedItem;
                    new CategoriaNegocio().AumentarPorRubro(seleccionado.Id, valor, tipo);
                }
                else if (modoActual == ModoAumento.Marca)
                {
                    var seleccionado = (Marca)CbxMarca.SelectedItem;
                    new MarcasNegocio().AumentarPorMarca(seleccionado.Id, valor, tipo);
                }
                /* else if (modoActual == ModoAumento.Producto)
                 {
                     // Buscamos el control TextBox que creamos dinámicamente
                     TextBox txtBarras = (TextBox)this.Controls["txtCodigoBarras"];
                     string codigo = txtBarras.Text;

                     if (string.IsNullOrEmpty(codigo))
                     {
                         MessageBox.Show("Ingrese un código de barras.");
                         return;
                     }

                     ArticuloNegocio negocio = new ArticuloNegocio();
                     negocio.AumentarPorCodigo(codigo, valor, tipo); // Deberás crear este método en tu Negocio
                    // negocio.SumarSoloCantidadStock(cantidadagregar, codigo);

                 }*/

                else if (modoActual == ModoAumento.Producto)
                {
                    // NO BUSQUES EL CONTROL POR NOMBRE. Usá la variable directamente.
                    // Si en el diseño se llama txtCodigoBarras, usalo así:
                    string codigo = txtCodigoBarras.Text;

                    if (string.IsNullOrEmpty(codigo))
                    {
                        MessageBox.Show("Ingrese un código de barras.");
                        return;
                    }

                    ArticuloNegocio negocio = new ArticuloNegocio();
                    // Aquí es donde ahora sí va a llegar el punto de control
                    negocio.AumentarPorCodigo(codigo, valor, tipo);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

                MessageBox.Show("CAMBIOS APLICADOS.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }




        private bool CampoValido()
        {
            bool EsValido = true;

            errorProvider1.Clear();

            if (!decimal.TryParse(TxtPorcentajeyocifra.Text, out decimal cant))
            {
                errorProvider1.SetError(TxtPorcentajeyocifra, "Porcentaje O cifra invalidos");
                EsValido = false;


            }

            return EsValido;
        }


        private void BuscarProductoPorCodigo()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulos articulo = negocio.Obtener_Por_Codigo(txtCodigoBarras.Text, false).FirstOrDefault();

            if (articulo != null)
            {
                // Usamos el operador ternario para poner un texto por defecto si la propiedad es nula
                string nombre = !string.IsNullOrEmpty(articulo.Nombre) ? articulo.Nombre : "Sin Nombre";
                string descripcion = !string.IsNullOrEmpty(articulo.Descripcion) ? articulo.Descripcion : "Sin Descripción";

                lblProducto.Text = $"{nombre} - {descripcion}";
                lblStock.Text = $"{nombre}- {descripcion} - {cantidadagregar} ";
            }
        }

        private void txtCodigoBarras_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Bloqueamos el "beep" de Windows para cualquier caso de Enter
                e.Handled = true;
                e.SuppressKeyPress = true;

                try
                {
                    ArticuloNegocio negocio = new ArticuloNegocio();
                    Articulos articulo = negocio.Obtener_Por_Codigo(txtCodigoBarras.Text, false).FirstOrDefault();

                    if (articulo != null)
                    {
                        // Usamos el operador ternario para poner un texto por defecto si la propiedad es nula
                        string nombre = !string.IsNullOrEmpty(articulo.Nombre) ? articulo.Nombre : "Sin Nombre";
                        string descripcion = !string.IsNullOrEmpty(articulo.Descripcion) ? articulo.Descripcion : "Sin Descripción";

                        lblProducto.Text = $"{nombre} - {descripcion}";

                    }
                    else // Es lo mismo que if (articulo == null)
                    {
                        if (string.IsNullOrWhiteSpace(txtCodigoBarras.Text))
                        {
                            MessageBox.Show("ESCRIBA UN CÓDIGO");
                            return;
                        }

                        DialogResult resultado = MessageBox.Show("Artículo no encontrado, ¿le gustaría agregarlo?",
                            "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (resultado == DialogResult.Yes)
                        {
                            AltaArticulo alta = new AltaArticulo(txtCodigoBarras.Text);

                            // Solo entramos al IF si el usuario hizo clic en "Aceptar" y se guardó
                            if (alta.ShowDialog() == DialogResult.OK)
                            {

                                MessageBox.Show("El Articulo ha sido ingresado en éxito");

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            }

        private void RbtnPorcentajeCostoVenta_CheckedChanged(object sender, EventArgs e)
        {
            lblPorcenOCifra.Text = "%";
            lblIngrese.Text = "INGRESE PORCENTAJE A SUMAR AL VALOR DE COSTO Y VENTA:";
        }

        private void RbtnMontoFijo_CheckedChanged(object sender, EventArgs e)
        {
            lblPorcenOCifra.Text = "$";
            lblIngrese.Text = "INGRESE DINERO A SUMAR AL VALOR DE COSTO Y VENTA:";
        }

        private void RbtnPorcentajeSoloVenta_CheckedChanged(object sender, EventArgs e)
        {
            lblPorcenOCifra.Text = "%";
            lblIngrese.Text = "INGRESE PORCENTAJE A SUMAR SOLO AL VALOR VENTA:";
        }

        private void RbtnSumarSoloAVenta_CheckedChanged(object sender, EventArgs e)
        {
            lblPorcenOCifra.Text = "$";
            lblIngrese.Text = "INGRESE DINERO A SUMAR SOLO AL VALOR VENTA:";


        }

        private void RbtnPorcentajeSoloACosto_CheckedChanged(object sender, EventArgs e)
        {
            lblPorcenOCifra.Text = "%";
            lblIngrese.Text = "INGRESE PORCENTAJE A SUMAR SOLO AL VALOR COSTO:";



        }

        private void RbtnSumaSoloACosto_CheckedChanged(object sender, EventArgs e)
        {
            lblPorcenOCifra.Text = "$";
            lblIngrese.Text = "INGRESE DINERO A SUMAR AL VALOR COSTO:";

        }
    }

    }
               
            
        

