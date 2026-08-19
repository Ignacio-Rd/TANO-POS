using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class PagoAProveedores
    {
        public int Id { get; set; }
        public int IdProveedor { get; set; }

        [DisplayName("Proveedor")]
        public Proveedor NombreProveedor { get; set; }

        public int IdCompra { get; set; }

        [DisplayName("Total de compra")]
        public decimal TotalCompra
        {
            get { return Compra != null ? Compra.Total : 0; }
        }

        [DisplayName("Cuanto pagaste")]
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int IdMetodo { get; set; }

        [DisplayName("Metodo de Pago")]
        public Metodo_de_pago metodopago { get; set; }

        public Compras Compra { get; set; }

        [DisplayName("N° Compra")] // Esta sí se va a ver y mostrará el Id
        public int NumeroCompra
        {
            get { return Compra != null ? Compra.Id : 0; }
        }
    }
}
