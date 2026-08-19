using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
   public class Venta_Por_Producto
    {

        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Producto { get; set; }

        public int Cantidad { get; set; }

        public int IdVenta { get; set; } 

        public decimal PrecioUnitario { get; set; }

        public decimal Recargo { get; set; }

        [DisplayName("Importe")]

        public decimal ImporteLinea { get; set; }

        
    }
}
