using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Metodo_de_pago
    {
        public int id { get; set; }

        public string MetodoPago { get; set; }

        public decimal porcentaje { get; set;}


        public override string ToString()
        {
            return MetodoPago;
        }

     

    }
}
