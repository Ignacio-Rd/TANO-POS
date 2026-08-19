using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class TICKET
    {
        public int IdVenta { get; set; }

        public DateTime fechaVenta { get; set; }

        public List<Venta_Por_Producto> venta_Por_Productos { get; set; }

        public decimal total { get; set; }

        public string impuestos { get; set; }

        public decimal TotalFinal { get; set; }
    }
}
