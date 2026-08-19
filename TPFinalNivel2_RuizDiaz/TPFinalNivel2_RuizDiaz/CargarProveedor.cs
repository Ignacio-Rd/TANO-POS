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
    public partial class CargarProveedor : Form
    {
        public CargarProveedor()
        {
            InitializeComponent();
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            ProveedorNegocio proveedor = new ProveedorNegocio();

            try
            {
                if (!string.IsNullOrWhiteSpace(txtNombreProveedor.Text))
                {
                    // 2. Creas el objeto 'Dominio' y le cargas los datos de los inputs
                    Proveedor nuevo = new Proveedor();
                    nuevo.Nombre = txtNombreProveedor.Text;
                    nuevo.Cuit = null;      // Asegúrate de tener estos campos en tu diseño
                    nuevo.Telefono = null;   // Si no los tienes, se enviarán como null o vacío

                    // 3. PASAS EL OBJETOS COMPLETO (Esto corrige tu error actual)
                    proveedor.agregar(nuevo);

                    MessageBox.Show("¡Proveedor agregado con éxito!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
