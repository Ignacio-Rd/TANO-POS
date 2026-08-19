using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class DetalleFiado
    {
        public int Id { get; set; }
        public int IdVenta { get; set; }
        public string IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioCostoUnitario { get; set; }
        public decimal PrecioVentaUnitario { get; set; }
        public decimal Monto => Cantidad * PrecioVentaUnitario;
        public string NombreProducto { get; set; } // ← propiedad simple
        public Articulos Articulo { get; set; }
        public Ventas Venta { get; set; }
    }
}
