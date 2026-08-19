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
    public partial class VentaTotalDia : Form
    {
        public VentaTotalDia()
        {
            InitializeComponent();
        }

        private void BtnTotalDia_Click(object sender, EventArgs e)
        {
            DateTime fecha = new DateTime();
            fecha = DtmTotalDia.Value;
            VentaNegocio negocio = new VentaNegocio();
            decimal total = 0;
            total = negocio.TotalDia(fecha);

            LblTotalDia.Text = $"Total Recaudado: {total:C}";
        }

        private void FiltrarRango_Click(object sender, EventArgs e)
        {
            VentaNegocio negocio = new VentaNegocio();

            // 1. Extraemos la FECHA pura del calendario principal (sin horas)
            DateTime fechaSeleccionada = DtmTotalDia.Value.Date;

            // 2. Construimos el INICIO: Fecha seleccionada + Hora del primer picker
            // Forzamos minutos y segundos a 0 para que sea exacto
            DateTime inicio = fechaSeleccionada.AddHours(HoraInicio.Value.Hour);

            // 3. Construimos el FIN: Fecha seleccionada + Hora del segundo picker
            // IMPORTANTE: Le sumamos 59 min y 59 seg para cubrir TODA la hora final
            DateTime fin = fechaSeleccionada.AddHours(HoraFin.Value.Hour).AddMinutes(59).AddSeconds(59);

            // 4. Llamamos a la base de datos
            decimal total = negocio.TotalPorRango(inicio, fin);

            // 5. Mostramos
            LblTotalDia.Text = $"Total Recaudado: {total:C}";
        }


        private void visible_rango_CheckedChanged(object sender, EventArgs e)
        {
            HoraInicio.Visible = visible_rango.Checked;
    HoraFin.Visible = visible_rango.Checked;
    FiltrarRango.Visible = visible_rango.Checked;
        }

        private void VentaTotalDia_Load(object sender, EventArgs e)
        {
            HoraInicio.Format = DateTimePickerFormat.Custom;
            HoraInicio.CustomFormat = "HH"; // Solo la hora

            HoraFin.Format = DateTimePickerFormat.Custom;
            HoraFin.CustomFormat = "HH";
        }
    }
      

    
}
