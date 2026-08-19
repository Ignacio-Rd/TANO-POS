using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows.Forms;
using System.Drawing.Printing;
using Negocio_;


namespace TPFinalNivel2_RuizDiaz
{
    public partial class Configurar_Impresora : Form
    {
        public Configurar_Impresora()
        {
            InitializeComponent();
        }

        private void Configurar_Impresora_Load(object sender, EventArgs e)
        {
            // WMI para detectar mejor impresoras de red e inalámbricas
            var searcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            foreach (System.Management.ManagementObject printer in searcher.Get())
            {
                string nombre = printer["Name"]?.ToString();
                if (!string.IsNullOrEmpty(nombre))
                    cboImpresoras.Items.Add(nombre);
            }

            // Seleccionar correctamente el item guardado en DB
            ImpresionNeogcio negocio = new ImpresionNeogcio();
            string impresoraGuardada = negocio.ObtenerImpresora();

            int index = cboImpresoras.Items.IndexOf(impresoraGuardada);
            if (index >= 0)
                cboImpresoras.SelectedIndex = index;
            else
                cboImpresoras.SelectedIndex = 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string seleccionada = cboImpresoras.Text;

            if (!string.IsNullOrWhiteSpace(seleccionada))
            {
                ImpresionNeogcio negocio = new ImpresionNeogcio();
                try
                {
                    negocio.GuardarImpresora(seleccionada);
                    MessageBox.Show("¡Configuración guardada! Impresora: " + seleccionada);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una impresora de la lista.");
            }
        }
    }
}
 
