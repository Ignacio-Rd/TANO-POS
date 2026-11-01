using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
   public class Ventas
    {

        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Total { get; set; }

        public int Id_metodo { get; set; }

        public Metodo_de_pago metododepago { get; set; }

        public int? IdCliente { get; set; }

        // Las listas por fecha mezclan ventas con cobros de deudores (tablas distintas, ids que se pueden repetir).
        // Un cobro NO es una venta y no se puede borrar como tal.
        [System.ComponentModel.Browsable(false)]
        public bool EsCobroDeuda { get; set; }

    }
}
