using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Compras
    {
        public int Id { get; set; }
        public int IdProveedor { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
