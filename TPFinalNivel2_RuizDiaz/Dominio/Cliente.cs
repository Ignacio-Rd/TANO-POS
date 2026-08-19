using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Cliente
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NumeroID { get; set; }
        public bool Estado { get; set; } // El tipo 'bit' de SQL se mapea como 'bool' en C#

        // Esta propiedad no está en la DB, pero te sirve para mostrar 
        // "Juan - 40123456" en los ComboBox de la interfaz
        public string NombreYDocumento
        {
            get { return $"{Nombre}"; }
        }
    }
}
