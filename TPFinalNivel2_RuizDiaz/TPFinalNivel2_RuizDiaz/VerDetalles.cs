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
    public partial class VerDetalles : Form
    {

        private Articulos articulo = new Articulos();
        private Marca marca = new Marca();
        private categoria categoria = new categoria();

        public VerDetalles(Articulos articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void VerDetalles_Load(object sender, EventArgs e)
        {
            lblNombre.Text = articulo.Nombre;
            lblMarca.Text = articulo.Marca.Descripcion;
            lblDescripcion.Text = articulo.Descripcion;
            lblCodigo.Text = articulo.codigo;
            lblCategoria.Text = articulo.Categoria.Descripcion;
            lblPrecio.Text = articulo.Precio.ToString("C");

        }
    }
}
