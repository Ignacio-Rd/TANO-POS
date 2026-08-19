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

namespace TPFinalNivel2_RuizDiaz
{
    public partial class altaCategorias : Form
    {
        private categoria categoria = null;

        public altaCategorias()
        {
            InitializeComponent();
        }

        private List<Dominio.categoria> listaCategoria = new List<Dominio.categoria>();


        private void altaCategorias_Load(object sender, EventArgs e)
        {
            cargar();

        }

        private void cargar()
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            listaCategoria = negocio.listarCategoria();
            dgvCategoria.DataSource = listaCategoria;
            dgvCategoria.Columns["id"].Visible = false;
         }

        private void btnAgregarCategorias_Click(object sender, EventArgs e)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            try
            {
                // 1. Limpiamos espacios en blanco y guardamos el texto
                string nombreNuevaCat = txtAgregarCategoria.Text.Trim();

                // 2. Validación de campo vacío (antes de hacer cualquier otra cosa)
                if (string.IsNullOrEmpty(nombreNuevaCat))
                {
                    MessageBox.Show("El nombre de la categoría no puede estar vacío.");
                    return; // Salimos del método
                }

                // 3. Traemos la lista actual de la DB para comparar
                // Asumo que tu método en CategoriaNegocio se llama listar()
                List<categoria> listaExistente = negocio.listarCategoria();

                // 4. Verificamos si ya existe una descripción igual (ignorando mayúsculas/minúsculas)
                bool yaExiste = listaExistente.Any(x => x.Descripcion.Equals(nombreNuevaCat, StringComparison.OrdinalIgnoreCase));

                if (yaExiste)
                {
                    MessageBox.Show("Esta categoría ya existe en el sistema.");
                }
                else
                {
                    // 5. Si es nueva, la creamos
                    if (categoria == null)
                    {
                        categoria = new categoria();
                        categoria.Descripcion = nombreNuevaCat;
                        negocio.agregarCategoria(categoria);

                        MessageBox.Show("Categoría agregada exitosamente.");
                        cargar(); // Refresca la lista en el formulario principal
                        Close();  // Cierra la ventana actual
                    }
                }
            }
            catch (Exception ex)
            {
                // Es mejor mostrar el error real aquí para saber qué falló en la DB
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            categoria seleccionado;

            try
            {

                DialogResult respuesta = MessageBox.Show("De verdad queres eliminar ese marca?", "eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (categoria)dgvCategoria.CurrentRow.DataBoundItem;

                    negocio.eliminarCategoria(seleccionado.Id);
                    cargar();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
