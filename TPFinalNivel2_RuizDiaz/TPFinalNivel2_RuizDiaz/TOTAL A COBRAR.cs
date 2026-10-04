using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPFinalNivel2_RuizDiaz;
using Dominio;
using Negocios;
using Negocio_;
using System.Globalization;

namespace TPFinalNivel2_RuizDiaz
{
    public partial class TOTAL_A_COBRAR : Form
    {
        // Variables globales del formulario
        private List<Articulos> _listaArticulos;
        private string _nombreMetodo;
        private decimal _subtotalSinRecargo;
        decimal totalacum;
        int metodopago;
        TICKET _ticketParaImprimir;
        int idventagenerada;

        // Actualiza tu constructor
        public TOTAL_A_COBRAR(decimal total, decimal subtotal, List<Articulos> lista, int idMetodo, string nombreMetodo)
        {
            InitializeComponent();

            _listaArticulos = lista;
            _nombreMetodo = nombreMetodo;
            metodopago = idMetodo;

            totalacum = total;              // El monto con recargo (para la DB)
            _subtotalSinRecargo = subtotal;  // El monto puro (para el Ticket)


            LblTotal.Text = $"Total a cobrar: {total.ToString("C")}";
        }

        public void button1_Click(object sender, EventArgs e)
        {
            VentaNegocio ventaNegocio = new VentaNegocio();
            Venta_Por_Producto_Negocio vppNegocio = new Venta_Por_Producto_Negocio();
            ArticuloNegocio negocio = new ArticuloNegocio();
            MetodoDePagoNegocio fondos = new MetodoDePagoNegocio();

            try
            {
                int idVentaGenerada = ventaNegocio.Agregar_venta(DateTime.Now, totalacum, metodopago);

                idventagenerada = idVentaGenerada;

                Ventas venta = new Ventas();


                // RECIÉN ACÁ IMPACTAMOS LA BASE DE DATOS
                ArticuloNegocio negocioArt = new ArticuloNegocio();

                // Agrupamos por código para no ir 50 veces a la base de datos
                // Si no querés usar GroupBy, podés usar un foreach simple y llamar a Descontar_Stock(art.codigo, 1)
                var productosAgrupados = _listaArticulos.GroupBy(x => x.codigo)
                                                       .Select(g => new { Codigo = g.Key, Total = g.Count() });

                foreach (var item in productosAgrupados)
                {
                    negocioArt.Descontar_Stock(item.Codigo, item.Total);
                }



                if (idVentaGenerada > 0)
                {
                    _ticketParaImprimir = vppNegocio.Agregar_Venta_Producto(
                        _listaArticulos,
                        DateTime.Now,
                        idVentaGenerada,
                        _subtotalSinRecargo,
                        _nombreMetodo,
                        totalacum
                    );

                    //aplicamos el cambio a los fondos según con que metodo pagó el cliente

                    /*fondos.SumarAlFondo(metodopago, totalacum);*/

                    // MessageBox.Show("Venta guardada con éxito.");

                    // CAMBIOS AQUÍ:
                    //  button1.Enabled = false;
                    // button1.Visible = false;// Deshabilitamos el botón de confirmar (ya se guardó)
                    // BtnImprimir.Visible = true;    // Habilitamos imprimir
                    // btnFinalizar.Visible = true;
                    // BtnImprimir.Enabled = true;    // Habilitamos imprimir
                    // btnFinalizar.Enabled = true;   // Habilitamos un botón de "Solo Finalizar"
                    // BtnFiar.Visible = false;
                    //  btnFinalizar.Focus();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void TOTAL_A_COBRAR_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Si _ticketParaImprimir ya tiene datos, es porque el button1 (Aceptar)
            // ya se ejecutó y la venta ESTÁ en la base de datos.
           // if (_ticketParaImprimir != null)
            //{
              //  this.DialogResult = DialogResult.OK;
            //}
        }

        private void Imprimir_PrintPage(object sender, PrintPageEventArgs e)
        {
            TICKET ticket = _ticketParaImprimir;

            Font fontNormal = new Font("Consolas", 9);
            Font fontHeader = new Font("Consolas", 12, FontStyle.Bold);
            Font fontTotal = new Font("Consolas", 11, FontStyle.Bold);

            int x = e.MarginBounds.Left;   // antes: 5
            int y = e.MarginBounds.Top;    // antes: 20
            int lineHeight = 14;

            // ------------------ ENCABEZADO ------------------
            e.Graphics.DrawString("--- KIOSCO 'EL TANO' ---", fontHeader, Brushes.Black, x, y);
            y += lineHeight * 2;
            e.Graphics.DrawString($"Venta Nº: {ticket.IdVenta}", fontNormal, Brushes.Black, x, y);
            y += lineHeight;
            e.Graphics.DrawString($"Fecha: {ticket.fechaVenta.ToString("dd/MM/yyyy HH:mm")}", fontNormal, Brushes.Black, x, y);
            y += lineHeight * 2;

            // ------------------ DETALLE ------------------
            e.Graphics.DrawString("----------------------------------", fontNormal, Brushes.Black, x, y);
            y += lineHeight;
            e.Graphics.DrawString("Cant.  Producto          Importe", fontNormal, Brushes.Black, x, y);
            y += lineHeight;
            e.Graphics.DrawString("----------------------------------", fontNormal, Brushes.Black, x, y);
            y += lineHeight;

            foreach (var linea in ticket.venta_Por_Productos)
            {
                string productoNombre = linea.Producto.Length > 20 ? linea.Producto.Substring(0, 20) : linea.Producto.PadRight(20);

                string lineaTexto =
                    linea.Cantidad.ToString().PadRight(6) +
                    productoNombre +
                    linea.PrecioUnitario.ToString("N2").PadLeft(7);

                e.Graphics.DrawString(lineaTexto, fontNormal, Brushes.Black, x, y);
                y += lineHeight;
            }

            // ------------------ TOTALES ------------------
            y += lineHeight;
            e.Graphics.DrawString("----------------------------------", fontNormal, Brushes.Black, x, y);
            y += lineHeight;

            e.Graphics.DrawString($"SUBTOTAL: {ticket.total.ToString("C")}", fontTotal, Brushes.Black, x, y);
            y += lineHeight;

            e.Graphics.DrawString($"RECARGO/IMP ({ticket.impuestos}): {(ticket.TotalFinal - ticket.total).ToString("C")}", fontTotal, Brushes.Black, x, y);
            y += lineHeight * 2;

            e.Graphics.DrawString("TOTAL FINAL:", fontHeader, Brushes.Black, x, y);
            e.Graphics.DrawString(ticket.TotalFinal.ToString("C"), fontHeader, Brushes.Black, x + 100, y);
            y += lineHeight * 3;

            e.Graphics.DrawString("¡GRACIAS POR SU COMPRA!", fontNormal, Brushes.Black, x + 10, y);
        }


        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            BtnImprimir.Enabled = false;
            // Primero ejecutamos toda la lógica de guardado de la venta
            VentaNegocio ventaNegocio = new VentaNegocio();
            Venta_Por_Producto_Negocio vppNegocio = new Venta_Por_Producto_Negocio();
            ArticuloNegocio negocioArt = new ArticuloNegocio();
            MetodoDePagoNegocio fondos = new MetodoDePagoNegocio();

            try
            {
                int idVentaGenerada = ventaNegocio.Agregar_venta(DateTime.Now, totalacum, metodopago);
                idventagenerada = idVentaGenerada;

                var productosAgrupados = _listaArticulos.GroupBy(x => x.codigo)
                                                       .Select(g => new { Codigo = g.Key, Total = g.Count() });
                foreach (var item in productosAgrupados)
                {
                    negocioArt.Descontar_Stock(item.Codigo, item.Total);
                }

                if (idVentaGenerada > 0)
                {
                    _ticketParaImprimir = vppNegocio.Agregar_Venta_Producto(
                        _listaArticulos,
                        DateTime.Now,
                        idVentaGenerada,
                        _subtotalSinRecargo,
                        _nombreMetodo,
                        totalacum
                    );

                    /*fondos.SumarAlFondo(metodopago, totalacum);*/

                    // Ahora procedemos con la impresión
                    ImpresionNeogcio negocioImp = new ImpresionNeogcio();
                    PrintDocument pd = new PrintDocument();
                    string nombreImpresora = "";

                    nombreImpresora = negocioImp.ObtenerImpresora();

                    if (string.IsNullOrEmpty(nombreImpresora) || nombreImpresora == "Seleccione Impresora")
                    {
                        MessageBox.Show("No has configurado ninguna impresora. Por favor, ve al menú de ajustes.", "Configuración Faltante");
                        // La venta YA se guardó, cerramos igual
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }

                    pd.PrinterSettings.PrinterName = nombreImpresora;

                    if (!pd.PrinterSettings.IsValid)
                    {
                        MessageBox.Show($"La impresora '{nombreImpresora}' no está instalada o cambió de nombre en Windows.", "Impresora no encontrada");
                        // La venta YA se guardó, cerramos igual
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }

                    pd.PrintPage += new PrintPageEventHandler(Imprimir_PrintPage);
                    pd.Print();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                RegistrarError(ex.Message, "Error en Impresión con guardado");
                MessageBox.Show("Ocurrió un error inesperado. Se ha guardado un reporte en la carpeta del programa.", "Error");
            }


        }

        private void RegistrarError(string mensaje, string origen)
        {
            try
            {
                // Crea (o abre) un archivo de texto en la carpeta de la aplicación
                string rutaLog = AppDomain.CurrentDomain.BaseDirectory + "log_errores.txt";

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(rutaLog, true))
                {
                    sw.WriteLine($"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] - ORIGEN: {origen}");
                    sw.WriteLine($"ERROR: {mensaje}");
                    sw.WriteLine(new string('-', 50));
                }
            }
            catch
            {
                // Si no se puede ni escribir el log, no hacemos nada para no romper la app
            }
        }

        private void TOTAL_A_COBRAR_Load(object sender, EventArgs e)
        {
            lblMetodo.Text = _nombreMetodo;

            this.KeyPreview = true;
            this.KeyDown += TOTAL_A_COBRAR_KeyDown;
        }

     


        /*/ private void btnFinalizar_Click(object sender, EventArgs e)
         {
             // Al asignar OK, el Formulario Principal limpiará la lista de productos
             this.DialogResult = DialogResult.OK;
             this.Close();
         }*/

        private void btnAbrirFiado_Click(object sender, EventArgs e)
        {
            // 1. Evitamos re-entrada deshabilitando el botón inmediatamente
            BtnFiar.Enabled = false;

            // 2. Usamos el 'using' para asegurar que el formulario se destruya al cerrar
            using (FIADO pantallaFiado = new FIADO(totalacum, _subtotalSinRecargo, _listaArticulos, 11, _nombreMetodo, idventagenerada))
            {
                // Importante: No llamar a ShowDialog directamente en un IF sin guardar el resultado
                DialogResult respuesta = pantallaFiado.ShowDialog();

                if (respuesta == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // Si el usuario canceló el fiado, volvemos a habilitar el botón
                    // y limpiamos cualquier mensaje de teclado pendiente
                    BtnFiar.Enabled = true;
                    Application.DoEvents(); // <--- Esto limpia la cola de mensajes de Windows
                }
            }
        }

       /* private bool ValidarPago()
        {
            errorProvider1.Clear();

            // Usamos tu método ValidarDecimal que está muy bueno
            if (!ValidarDecimal(textBox1.Text, out decimal monto) || monto < 0)
            {
                errorProvider1.SetError(textBox1, "Ingrese un monto de pago válido.");
                return false;
            }
            return true;
        }*/

     /*   private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                // 1. Validamos. Si devuelve false, salimos con 'return' para no romper nada.
                if (!ValidarPago()) return;

                // 2. Si llegamos acá, es porque el número es seguro.
                decimal pagoCliente = decimal.Parse(textBox1.Text);

                // 3. LA CUENTA: Pago - Total (Si me da 1000 y sale 800, el vuelto es 200)
                decimal vuelto = pagoCliente - totalacum;

                // 4. Mostrar resultado
                lblVuelto.Text = vuelto.ToString("C2");

                // Opcional: Color de alerta si paga de menos
                lblVuelto.ForeColor = vuelto < 0 ? Color.Red : Color.DarkGreen;

                e.SuppressKeyPress = true; // Quita el sonido "Plin"

                button1.Focus();

            }

        }*/

    /*    private bool Campos_Validos()
        {
            bool esValido = true;
            errorProvider1.Clear();

            // 1. Seguridad en Cantidad (Enteros)
            if (!int.TryParse(textBox1.Text, out int cant) || cant <= 0)
            {
                errorProvider1.SetError(textBox1, "La cantidad debe ser un número entero mayor a cero.");
                esValido = false;
            }

            // 2. Seguridad en Precios (Usando el método del punto 1)
            if (!ValidarDecimal(textBox1.Text, out decimal costo) || costo <= 0)
            {
                errorProvider1.SetError(textBox1, "Precio de costo inválido (use punto o coma para decimales).");
                esValido = false;
            }

            if (!ValidarDecimal(textBox1.Text, out decimal venta) || venta <= 0)
            {
                errorProvider1.SetError(textBox1, "Precio de venta inválido.");
                esValido = false;
            }

            return esValido;
        }*/

       /* private bool Campos_Valido_Cantidad()
        {
            bool esValido = true;

            // 1. Limpiar errores anteriores
            errorProvider1.Clear();

            // 2. Validar Cantidad
            if (!int.TryParse(textBox1.Text, out int cant) || cant <= 0)
            {
                errorProvider1.SetError(textBox1, "Ingrese una cantidad válida mayor a cero.");
                esValido = false;
            }

            return esValido;



        }*/

        private bool ValidarDecimal(string texto, out decimal resultado)
        {
            // Permite tanto punto como coma, convirtiéndolo a un formato estándar
            return decimal.TryParse(texto.Replace(',', '.'),
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out resultado);
        }

        private void TOTAL_A_COBRAR_KeyDown(object sender, KeyEventArgs e)
        {
           
             if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }




        }
    }
}
    
    





        
    