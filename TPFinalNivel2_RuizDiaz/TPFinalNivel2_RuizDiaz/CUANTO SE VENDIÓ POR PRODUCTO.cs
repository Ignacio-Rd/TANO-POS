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
    public partial class CUANTO_SE_VENDIÓ_POR_PRODUCTO : Form
    {


        public CUANTO_SE_VENDIÓ_POR_PRODUCTO()
        {
            InitializeComponent();
            ArticuloNegocio negocio = new ArticuloNegocio();

            // Cbx_Productos.DataSource = negocio.ListarArticulo();

            //            Cbx_Productos.DisplayMember = "Nombre";

            // 3. (Opcional pero recomendado) INDICA QUÉ PROPIEDAD SE USA COMO VALOR INTERNO
            //          Cbx_Productos.ValueMember = "Id";

            // ⬇️ AJUSTE PARA MOSTRAR SOLO LA HORA (SIN MINUTOS NI SEGUNDOS) ⬇️

            // Configuración DTMINICIO
            DtpHoraInicio.Format = DateTimePickerFormat.Custom; // Necesario para usar CustomFormat
            DtpHoraInicio.CustomFormat = "HH:mm";                  // Muestra la hora en formato 24 horas (ej: 14)
            DtpHoraInicio.ShowUpDown = true;
            DtpHoraInicio.Value = DateTime.Today;

            // Configuración DTMFINAL
            DtpHoraFin.Format = DateTimePickerFormat.Custom;
            DtpHoraFin.CustomFormat = "HH:mm";
            DtpHoraFin.ShowUpDown = true;
            DtpHoraInicio.Value = DateTime.Now;
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            // 1. Extraemos las partes de la fecha del picker principal (DTMINICIO)
            int anio = DTMINICIO.Value.Year;
            int mes = DTMINICIO.Value.Month;
            int dia = DTMINICIO.Value.Day;

            // 2. Validación de seguridad: Que el inicio no sea después del fin
            if (DtpHoraInicio.Value.TimeOfDay > DtpHoraFin.Value.TimeOfDay)
            {
                MessageBox.Show("La hora de inicio no puede ser mayor a la hora de fin.",
                                "Rango Horario Inválido",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // 3. Construimos el INICIO: Fecha elegida + Hora y Minuto del picker de inicio
            DateTime fechaCompletaInicio = new DateTime(anio, mes, dia,
                                                       DtpHoraInicio.Value.Hour,
                                                       DtpHoraInicio.Value.Minute, 0);

            // 4. Construimos el FIN: Fecha elegida + Hora y Minuto del picker de fin + 59 segundos
            // Esto captura las ventas que ocurran hasta el último segundo de ese minuto.
            DateTime fechaCompletaFin = new DateTime(anio, mes, dia,
                                                     DtpHoraFin.Value.Hour,
                                                     DtpHoraFin.Value.Minute, 59);

            Venta_Por_Producto_Negocio productoNegocio = new Venta_Por_Producto_Negocio();

            try
            {
                // 5. Ejecutamos la búsqueda
                // En el Formulario:
                var resultado = productoNegocio.CargarVentasEnDataGridView(txtCodigoProducto.Text, fechaCompletaInicio, fechaCompletaFin);

                DGVproductos.DataSource = null;
                DGVproductos.DataSource = resultado;

                if (DGVproductos.Columns.Contains("TotalRecaudado"))
                {
                    // Formato "N2" pone mil separadores y 2 decimales (ej: 30.450,00)
                    DGVproductos.Columns["TotalRecaudado"].DefaultCellStyle.Format = "N2";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message);
            }
        }

        private void CUANTO_SE_VENDIÓ_POR_PRODUCTO_Load(object sender, EventArgs e)
        {
            // Establece el primer picker en el inicio del día (00:00)
            DtpHoraInicio.Value = DateTime.Today;

            // Establece el segundo picker en el último minuto del día (23:59)
            // Usamos DateTime.Today para obtener la fecha actual con hora 00:00 y le sumamos el tiempo
            DtpHoraFin.Value = DateTime.Today.AddHours(23).AddMinutes(59);
        }

        private void txtCodigoProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Bloqueamos el "beep" de Windows para cualquier caso de Enter
                e.Handled = true;
                e.SuppressKeyPress = true;

                try
                {
                    ArticuloNegocio negocio = new ArticuloNegocio();
                    Articulos articulo = negocio.Obtener_Por_Codigo(txtCodigoProducto.Text, false).FirstOrDefault();

                    if (articulo != null)
                    {
                        // Usamos el operador ternario para poner un texto por defecto si la propiedad es nula
                        string nombre = !string.IsNullOrEmpty(articulo.Nombre) ? articulo.Nombre : "Sin Nombre";
                        string descripcion = !string.IsNullOrEmpty(articulo.Descripcion) ? articulo.Descripcion : "Sin Descripción";

                        lblProducto.Text = $"{nombre} - {descripcion}";

                    }
                    else // Es lo mismo que if (articulo == null)
                    {
                        if (string.IsNullOrWhiteSpace(txtCodigoProducto.Text))
                        {
                            MessageBox.Show("ESCRIBA UN CÓDIGO");
                            return;
                        }

                        MessageBox.Show("Artículo no encontrado");


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar: " + ex.Message);
                }
            }
    }
        } } 
