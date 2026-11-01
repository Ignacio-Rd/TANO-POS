using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TPFinalNivel2_RuizDiaz
{
    // Pantalla principal. Acá solo vive la navegación entre módulos; el aspecto está en FormularioPrincipal.Interfaz.cs.
    // Cada módulo se abre igual que antes (ventana modal), con el mismo formulario de siempre.
    public partial class FormularioPrincipal : Form
    {
        private readonly Dictionary<Keys, Action> _atajos = new Dictionary<Keys, Action>();

        public FormularioPrincipal()
        {
            InitializeComponent();
            ConstruirInterfaz();
        }

        // Teclas rápidas globales de la pantalla principal (F1..F9, Ctrl+P). Se resuelven antes que cualquier control.
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Action accion;
            if (_atajos.TryGetValue(keyData, out accion))
            {
                accion();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ---------- Módulos ----------

        private void AbrirModulo(Func<Form> crear)
        {
            using (Form formulario = crear())
            {
                formulario.ShowDialog(this);
            }

            // Al volver (por ejemplo de una venta) el tablero se actualiza solo.
            var tarea = ActualizarIndicadoresAsync();
        }

        private void AbrirVenta() { AbrirModulo(() => new REGISTRADORA_DE_COMPRAS()); }
        private void AbrirCierreDeCaja() { AbrirModulo(() => new FechaInformeDeVentas()); }
        private void AbrirGastosVarios() { AbrirModulo(() => new GastosVarios()); }
        private void AbrirMetodosDePago() { AbrirModulo(() => new formas_de_pago()); }
        private void AbrirDeudores() { AbrirModulo(() => new ListaDeudores()); }
        private void AbrirArticulos() { AbrirModulo(() => new Form1()); }
        private void AbrirProveedores() { AbrirModulo(() => new ConsultarCompraProveedores()); }
        private void AbrirHistorialDeVentas() { AbrirModulo(() => new Venta_Por_Venta()); }
        private void AbrirVentasPorProducto() { AbrirModulo(() => new CUANTO_SE_VENDIÓ_POR_PRODUCTO()); }
        private void AbrirTotalPorDia() { AbrirModulo(() => new VentaTotalDia()); }
        private void AbrirImpresora() { AbrirModulo(() => new Configurar_Impresora()); }

        // Igual que antes: si ya hay una ventana de ingreso de mercadería abierta, se la trae al frente.
        private void AbrirIngresoDeMercaderia()
        {
            PAGAR_A_PROVEEDOR existente = Application.OpenForms.OfType<PAGAR_A_PROVEEDOR>().FirstOrDefault();

            if (existente != null)
            {
                existente.WindowState = FormWindowState.Normal;
                existente.BringToFront();
                existente.Focus();
            }
            else
            {
                new PAGAR_A_PROVEEDOR().Show();
            }
        }
    }
}
