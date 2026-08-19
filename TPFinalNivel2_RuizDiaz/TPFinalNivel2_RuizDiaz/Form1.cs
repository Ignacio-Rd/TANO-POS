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
    public partial class Form1 : Form
    {
        private List<Dominio.Articulos> listaArticulos = new List<Dominio.Articulos>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            
          
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Dominio.Articulos seleccionado = (Dominio.Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.URL);
            }
        }

        private void cargar()
        {

            ArticuloNegocio negocio = new ArticuloNegocio();
            MarcasNegocio marcasNegocio = new MarcasNegocio();
            CategoriaNegocio categoria = new CategoriaNegocio();
            listaArticulos = negocio.listar();
            dgvArticulos.DataSource = listaArticulos;
            ocultarColumnas();
            dgvArticulos.ClearSelection();
            cbxBuscarMarca.DataSource = marcasNegocio.listarmarca();
            cbxBuscarRubro.DataSource = categoria.listarCategoria();
            //pbxArticulos.Load(listaArticulos[0].URL);


        }

        private void ocultarColumnas()
        {

            dgvArticulos.Columns["URL"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["metodoDePago"].Visible = false;
        }

        private void cargarImagen(string imagen)
        {
            try
            {

                pbxArticulos.Load(imagen);

            }
            catch (Exception ex)
            {

                pbxArticulos.Load("https://yaktribe.games/community/media/placeholder-jpg.84782/full");
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            AltaArticulo alta = new AltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvArticulos.CurrentRow == null || dgvArticulos.Rows.Count == 0 || dgvArticulos.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("No se encontró ningún artículo para modificar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                Dominio.Articulos seleccionado;

                seleccionado = (Dominio.Articulos)dgvArticulos.CurrentRow.DataBoundItem;

                AltaArticulo modificar = new AltaArticulo(seleccionado);

                modificar.ShowDialog();
                cargar();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ocurrió un error al intentar modificar el artículo.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {

            ArticuloNegocio negocio = new ArticuloNegocio();
            Dominio.Articulos seleccionado;

            try
            {
                if (dgvArticulos.CurrentRow == null || dgvArticulos.Rows.Count == 0 || dgvArticulos.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("No se encontró ningún artículo para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                DialogResult respuesta = MessageBox.Show("De verdad queres eliminar ese articulo?", "eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);


                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Dominio.Articulos)dgvArticulos.CurrentRow.DataBoundItem;

                    negocio.Eliminar(seleccionado.Id);

                    cargar();
                }



            }
            catch (Exception ex)
            {

                MessageBox.Show("Este articulo no se puede eliminar ya que hay ventas hechas de este articulo, si puede modificarlo o eliminar todas las ventas hechas de este articulo, puede afectar de mala manera su base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Dominio.Articulos> listafiltrada;

            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)
            {
                listafiltrada = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listafiltrada = listaArticulos;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listafiltrada;
            ocultarColumnas();
        }

    

        private void btnVerdetalles_Click(object sender, EventArgs e)
        {
            try
            {
                Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                VerDetalles ver = new VerDetalles(seleccionado);
                ver.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show("no seleccionó ningun producto");

            }

            

        }





        private void BtnPrecioRubro_Click(object sender, EventArgs e)
        {
            // Pasamos el Enum en modo Categoria
            // Usando el nombre del Formulario + el Enum
            Cambiar_Precio_Por_Marca formulario = new Cambiar_Precio_Por_Marca(Cambiar_Precio_Por_Marca.ModoAumento.Categoria);
            
            formulario.ShowDialog();
            cargar();
        }

        private void BtnAumentarMarca_Click(object sender, EventArgs e)
        {
            // Pasamos el Enum en modo Marca
            // Usando el nombre del Formulario + el Enum
            Cambiar_Precio_Por_Marca formulario = new Cambiar_Precio_Por_Marca(Cambiar_Precio_Por_Marca.ModoAumento.Marca);
            formulario.ShowDialog();
            cargar();
        }

        private bool ArticuloYaAgregado(string codigo)
        {


            foreach (DataGridViewRow fila in dgvArticulos.Rows )
            {
                if (fila.Cells["Codigo"].Value != null &&
                    fila.Cells["Codigo"].Value.ToString().Equals(codigo, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void dgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnBuscarMarca_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulos> list = new List<Articulos>();
            string marca = cbxBuscarMarca.Text;

            dgvArticulos.DataSource = negocio.buscarmarca(marca);
       }

        private void BtnBuscarRubro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulos> list = new List<Articulos>();
            string rubro = cbxBuscarRubro.Text;

            dgvArticulos.DataSource = negocio.buscarrubro(rubro);


        }

        private void BtnAumentarProducto_Click(object sender, EventArgs e)
        {
            Cambiar_Precio_Por_Marca formulario = new Cambiar_Precio_Por_Marca(Cambiar_Precio_Por_Marca.ModoAumento.Producto);
            formulario.ShowDialog();
            cargar();
        }
    }


}

