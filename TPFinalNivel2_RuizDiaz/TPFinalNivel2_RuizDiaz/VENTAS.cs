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
    public partial class Venta_Por_Venta : Form
    {
        private List<Ventas> listaventas = new List<Ventas>();

        private List<TotalPorFecha> listafechas = new List<TotalPorFecha>();

        

        private Ventas ventas = null;


        public Venta_Por_Venta()
        {
            InitializeComponent();

            // Configuración DtpInicio
            DtpHoraInicio.Format = DateTimePickerFormat.Custom;
            DtpHoraInicio.CustomFormat = "HH:mm"; // <--- AGREGAMOS :mm PARA LOS MINUTOS
            DtpHoraInicio.ShowUpDown = true;

            // Configuración DtpFin
            DtpHoraFin.Format = DateTimePickerFormat.Custom;
            DtpHoraFin.CustomFormat = "HH:mm";    // <--- AGREGAMOS :mm PARA LOS MINUTOS
            DtpHoraFin.ShowUpDown = true;

            DtpHoraInicio.Value = DateTime.Today;

            // Hora Fin: Hoy a las 23:59
            DtpHoraFin.Value = DateTime.Today.AddHours(23).AddMinutes(59);

        }

        private void cargar()
        {
            VentaNegocio negocio = new VentaNegocio();
            listaventas = negocio.Listar_ventas();
            DgvVentas.DataSource = listaventas;
            OcultarColumnas();

        }

        private void cargarVolver()
        {
            VentaNegocio negocio = new VentaNegocio();

            listaventas = negocio.Listar_ventas();

            DgvVentas.DataSource = listaventas;
        }

        private void CargarGrillaFiltrada(List<Ventas> lista)
        {
            // Ya no le pedimos nada al 'negocio' aquí adentro.
            // Solo usamos la lista que nos llegó por el paréntesis.
            DgvVentas.DataSource = lista;
            OcultarColumnas();

            if (lista.Count == 0)
            {
                MessageBox.Show("No hay ventas en la fecha seleccionada.");
            }

            CalcularTotal(lista);
        }


        private void cargarFechaTotal (List<TotalPorFecha> totalPorFechas, DateTime fecha)
        {
            TtotalPorFechNegocio negocio = new TtotalPorFechNegocio();

            totalPorFechas = negocio.TotalPorFechas(fecha);

          //  DgvTotalDia.DataSource = totalPorFechas;


        }

        private void VENTAS_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void BtnFiltrarFecha_Click(object sender, EventArgs e)
        {
            DateTime fechaseleccionada = DtpFiltro.Value.Date;
            VentaNegocio negocio = new VentaNegocio();

            // 1. Obtenemos la lista una sola vez
            List<Ventas> listafecha = negocio.FiltrarFecha(fechaseleccionada);

            // 2. La asignamos directamente o usamos el método de carga corregido
            CargarGrillaFiltrada(listafecha);

            
            ActualizarInterfaz(listafecha);
        }

        private void BtnFiltrarTotal_Click(object sender, EventArgs e)
        {
            DateTime fechaseleccionada = DtpFiltro.Value.Date;

            TtotalPorFechNegocio negocio = new TtotalPorFechNegocio();

            List<TotalPorFecha> totalPorFechas = negocio.TotalPorFechas(fechaseleccionada);

          

        }

        private void BtnVolverVenta_Click(object sender, EventArgs e)
        {
            VentaNegocio negocio = new VentaNegocio();

            listaventas = negocio.Listar_ventas();


            DgvVentas.DataSource = listaventas;

            DGVVentas_Agrupadas.DataSource = null;
            DGVVentas_Agrupadas.Columns.Clear();
            label14.Text = "---";      // Efectivo
            label10.Text = "---";      // Mercado Pago
            lblCrédito.Text = "---";       // Crédito
            lblDébito.Text = "---";        // Débito
            label12.Text = "---";  // Transferencia
            lblUala.Text = "---";            // Ualá
            lblFiado.Text = "---";        // Fiado
            lblRecaudacionTotal.Text = "---";

            OcultarColumnas();

            

        }

        private void BtnVolverFecha_Click(object sender, EventArgs e)
        {

            TtotalPorFechNegocio negocio = new TtotalPorFechNegocio();

            listafechas = negocio.listarFecha();

            

            DgvVentas.DataSource = listafechas;

            OcultarColumnas();

        }

        private void BtnBorrarVenta_Click(object sender, EventArgs e)
        {
            VentaNegocio negocio = new VentaNegocio();

            try
            {
                // 1. Verificamos que haya una fila seleccionada
                if (DgvVentas.CurrentRow == null)
                {
                    MessageBox.Show("Por favor, seleccione una venta de la lista para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 2. Pedimos confirmación con un ícono de advertencia (como en tu captura 275)
                DialogResult respuesta = MessageBox.Show("¿Está seguro de eliminar esta venta? Esta acción no se puede deshacer.",
                                                         "Eliminando Venta",
                                                         MessageBoxButtons.YesNo,
                                                         MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    // 3. Obtenemos el objeto seleccionado
                    Ventas seleccionado = (Ventas)DgvVentas.CurrentRow.DataBoundItem;

                    // 4. Ejecutamos el borrado
                    negocio.EliminarVenta(seleccionado.Id);

                    MessageBox.Show("Venta eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 5. Refrescamos la grilla y el totalizador para que el número baje
                    cargar(); // O el método que uses para recargar la lista actual
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
           
        }

        private void FiltroRango_Click(object sender, EventArgs e)
        {
            int anio = DtpFiltro.Value.Year;
            int mes = DtpFiltro.Value.Month;
            int dia = DtpFiltro.Value.Day;

            if (DtpHoraInicio.Value.TimeOfDay > DtpHoraFin.Value.TimeOfDay)
            {
                MessageBox.Show("La hora de inicio no puede ser mayor a la hora de fin.", "Rango Horario Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime HoraInicio = new DateTime(anio, mes, dia, DtpHoraInicio.Value.Hour, DtpHoraInicio.Value.Minute, 0);
            DateTime HoraFin = new DateTime(anio, mes, dia, DtpHoraFin.Value.Hour, DtpHoraFin.Value.Minute, 59);

            VentaNegocio negocio = new VentaNegocio();
            decimal deudaHistorica; // Variable para recibir el valor out

            try
            {
                // Llamamos al nuevo método con el parámetro out
                List<Ventas> listaRango = negocio.ConsultaRango(HoraInicio, HoraFin, out deudaHistorica);

                DgvVentas.DataSource = null;
                DgvVentas.DataSource = listaRango;
                OcultarColumnas();

                if (listaRango.Count == 0)
                {
                    MessageBox.Show($"No se hallaron ventas entre las {HoraInicio:HH:mm} y las {HoraFin:HH:mm}.");
                }

                // Llamamos a tu interfaz enviándole la lista y la deuda calculada
                ActualizarInterfaz(listaRango);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void OcultarColumnas()
        {
            DgvVentas.Columns["Id"].Visible = false;
            DgvVentas.Columns["id_metodo"].Visible = false;
            DgvVentas.Columns["IdCliente"].Visible = false;
            
         
        }

        public void OcultarColumnasDgvAgrup()
        {
            DGVVentas_Agrupadas.Columns["Id"].Visible = false;
            DGVVentas_Agrupadas.Columns["IdVenta"].Visible = false;
            DGVVentas_Agrupadas.Columns["Fecha"].Visible = false;
        }

        private void DtpHoraInicio_ValueChanged(object sender, EventArgs e)
        {

        }

        private void DgvVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

      

        private void Buscar_Articulo_Click(object sender, EventArgs e)
        {
            CUANTO_SE_VENDIÓ_POR_PRODUCTO formulario = new CUANTO_SE_VENDIÓ_POR_PRODUCTO();
            formulario.ShowDialog();
        }

        private void TotalDía_Click(object sender, EventArgs e)
        {
            VentaTotalDia ventadia = new VentaTotalDia();

            ventadia.Show();
        }

        private void CalcularTotal(List<Ventas> lista)
        {
            // Usamos LINQ para sumar la propiedad .Total de cada objeto en la lista
            decimal totalSumado = lista.Sum(x => x.Total);

            // Lo mostramos en el Label con formato de moneda
            
            lblRecaudacionTotal.Text = $"Total Recaudado: {totalSumado.ToString("C")}";

            // Opcional: Cambiar el color si es 0
            
           
        }

        private void ActualizarInterfaz(List<Ventas> lista)
        {
            VentaNegocio negocio = new VentaNegocio();

            // Calcular por cada método de pago
            decimal totalEfectivo = lista
                .Where(x => x.metododepago.MetodoPago == "Efectivo")
                .Sum(x => x.Total);

            decimal totalMercadoPago = lista
                .Where(x => x.metododepago.MetodoPago == "Mercado Pago")
                .Sum(x => x.Total);

            decimal totalCredito = lista
                .Where(x => x.metododepago.MetodoPago == "Crédito")
                .Sum(x => x.Total);

            decimal totalDebito = lista
                .Where(x => x.metododepago.MetodoPago == "Débito")
                .Sum(x => x.Total);

            decimal totalTransferencia = lista
                .Where(x => x.metododepago.MetodoPago == "Transferencia")
                .Sum(x => x.Total);

            decimal totalUala = lista
                .Where(x => x.metododepago.MetodoPago == "Ualá")
                .Sum(x => x.Total);

            decimal totalFiadoHoy = lista
                .Where(x => x.metododepago.MetodoPago == "FIADO")
                .Sum(x => x.Total);

            decimal totalRealEntrante = lista
                .Where(x => x.metododepago.MetodoPago != "FIADO")
                .Sum(x => x.Total);

            // Actualizar labels
            // Actualizar labels con color
            label14.Text = totalEfectivo.ToString("C2");
            label14.ForeColor = totalEfectivo > 0 ? Color.DarkGreen : Color.DarkRed;

            label10.Text = totalMercadoPago.ToString("C2");
            label10.ForeColor = totalMercadoPago > 0 ? Color.DarkGreen : Color.DarkRed;

            lblCrédito.Text = totalCredito.ToString("C2");
            lblCrédito.ForeColor = totalCredito > 0 ? Color.DarkGreen : Color.DarkRed;

            lblDébito.Text = totalDebito.ToString("C2");
            lblDébito.ForeColor = totalDebito > 0 ? Color.DarkGreen : Color.DarkRed;

            label12.Text = totalTransferencia.ToString("C2");
            label12.ForeColor = totalTransferencia > 0 ? Color.DarkGreen : Color.DarkRed;

            lblUala.Text = totalUala.ToString("C2");
            lblUala.ForeColor = totalUala > 0 ? Color.DarkGreen : Color.DarkRed;

            lblFiado.Text = totalFiadoHoy.ToString("C2");
            lblFiado.ForeColor = totalFiadoHoy > 0 ? Color.DarkGreen : Color.DarkRed;

            lblRecaudacionTotal.Text = totalRealEntrante.ToString("C2");
            lblRecaudacionTotal.ForeColor = totalRealEntrante > 0 ? Color.DarkGreen : Color.DarkRed;

        }

        private void BtnFrmFiados_Click(object sender, EventArgs e)
        {
            ListaDeudores formulario = new ListaDeudores();
            formulario.ShowDialog();
            cargarVolver();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FechaInformeDeVentas formulario = new FechaInformeDeVentas();

            formulario.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void DgvVentas_SelectionChanged(object sender, EventArgs e)
        {

            if (DgvVentas.CurrentRow == null) return;
            if (DgvVentas.CurrentRow.Index < 0) return;

            try
            {
                Venta_Por_Producto_Negocio negocio = new Venta_Por_Producto_Negocio();
                int IdSeleccionado = Convert.ToInt32(DgvVentas.CurrentRow.Cells["Id"].Value);
                DGVVentas_Agrupadas.DataSource = negocio.ObtenerDetallePorId(IdSeleccionado);
                OcultarColumnasDgvAgrup();
            }
            catch { }
        }
    }
}
