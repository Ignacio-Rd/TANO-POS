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
    public partial class AltaMarcas : Form
    {
        private List<Marca> listamarcas = new List<Marca>();

        private Marca marca = null;

        public AltaMarcas()
        {
            InitializeComponent();
        }

        private void AltaMarcas_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void cargar()
        {
            MarcasNegocio negocio = new MarcasNegocio();
            listamarcas = negocio.listarmarca();
            dgvMarcas.DataSource = listamarcas;
            dgvMarcas.Columns["id"].Visible = false;

        }

        private void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            MarcasNegocio negocio = new MarcasNegocio();

            try
            {
                string nombreNuevaMarca = txtAgregarMarca.Text.Trim();

                // 1. Validación de campo vacío
                if (string.IsNullOrEmpty(nombreNuevaMarca))
                {
                    MessageBox.Show("Por favor, ingrese un nombre para la marca.");
                    return; // Salimos del método para que no intente agregar nada
                }

                // 2. Traer la lista actual para comparar
                List<Marca> listaExistente = negocio.listarmarca(); // Asumiendo que tienes un método listar()

                // 3. Buscar si ya existe (usamos Any de LINQ para mayor rapidez)
                bool yaExiste = listaExistente.Any(x => x.Descripcion.Equals(nombreNuevaMarca, StringComparison.OrdinalIgnoreCase));

                if (yaExiste)
                {
                    MessageBox.Show("Esta marca ya se encuentra en la lista.");
                }
                else
                {
                    // 4. Si no existe y no es nula, la agregamos
                    if (marca == null)
                    {
                        marca = new Marca();
                        marca.Descripcion = nombreNuevaMarca;
                        negocio.agregarMarca(marca);

                        MessageBox.Show("Agregado exitosamente");
                        cargar();
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.ToString());
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            MarcasNegocio negocio = new MarcasNegocio();
            Marca seleccionado;

            try
            {

                DialogResult respuesta = MessageBox.Show("De verdad queres eliminar ese marca?", "eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if(respuesta == DialogResult.Yes)
                {
                    seleccionado = (Marca)dgvMarcas.CurrentRow.DataBoundItem;

                    negocio.eliminarMarca(seleccionado.Id);
                    cargar();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Este articulo no se puede eliminar ya que hay ventas hechas de este articulo, si puede modificarlo o eliminar todas las ventas hechas de este articulo, puede afectar de mala manera su base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}
