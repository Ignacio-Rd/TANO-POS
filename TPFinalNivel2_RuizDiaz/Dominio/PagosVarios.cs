using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class PagosVarios
    {
        public int IdPago { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }  // ← string directo, sin objeto anidado
        public int IdMetodoPago { get; set; }
        public string MetodoPago { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
    }
}