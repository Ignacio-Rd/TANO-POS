using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class DetalleCompra
    {

        public string IdProducto { get; set; }
        public string NombreProducto { get; set; } // Útil para mostrar en la grilla
        public int Cantidad
        {
            get; set;
        }
        [DisplayName("PrecioVenta")]
        public decimal PrecioVenta { get; set; }
        public decimal PrecioCosto { get; set; }
        public decimal Monto { get; set; }
    }
}
