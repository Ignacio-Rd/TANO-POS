using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
   public class Marca
    {

        public int Id { get; set; }
        public string Descripcion { get; set; }

        public override string ToString()
        {
            return Descripcion;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Marca))
                return false;

            return this.Id == ((Marca)obj).Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
