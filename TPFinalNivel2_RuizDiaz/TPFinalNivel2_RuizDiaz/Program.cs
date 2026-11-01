using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPFinalNivel2_RuizDiaz.Infraestructura;

namespace TPFinalNivel2_RuizDiaz
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Una sola copia del programa por computadora (dos cajas sobre la misma base se pisarían).
            using (InstanciaUnica instancia = InstanciaUnica.Tomar(InstanciaUnica.NombreAplicacion))
            {
                if (instancia == null)
                {
                    if (!InstanciaUnica.TraerAlFrenteLaOtra())
                        MessageBox.Show("TANO POS ya está abierto en esta computadora.", "TANO POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Antes de crear cualquier ventana: así ningún error queda sin atender.
                ManejadorErrores.Instalar();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormularioPrincipal());
            }
        }
    }
}
