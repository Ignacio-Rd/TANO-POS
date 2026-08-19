using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Proveedor
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cuit { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; }

        // Sobreescribimos el ToString para que si lo usas en un ComboBox 
        // se vea el nombre directamente.
        public override string ToString()
        {
            return Nombre;
        }
    }
}
