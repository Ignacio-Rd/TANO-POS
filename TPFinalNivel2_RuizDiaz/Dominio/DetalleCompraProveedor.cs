using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class DetalleCompraProveedor
    {
        public int IdCompra { get; set; } // El "dueño" de este renglón
        public string IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioCosto { get; set; }
    }
}
