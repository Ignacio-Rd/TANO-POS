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

    }
}
