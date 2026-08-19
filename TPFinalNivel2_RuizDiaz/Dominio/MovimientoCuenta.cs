using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class MovimientoCuenta
    {

        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Tipo { get; set; } // Dirá "VENTA" o "PAGO"
        public int IdVenta { get; set; }
    }
}
