using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio

{
    public class Articulos
    {

        public int Id { get; set; }

        public string codigo { get; set; }

        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        public Marca Marca { get; set; }

        [DisplayName("Rubro")]
        public categoria Categoria { get; set; }

        public Metodo_de_pago metodoDePago { get; set; }

        public string URL { get; set; }

        [DisplayName("Precio Costo")]
        public decimal PrecioCosto { get; set; }

        [DisplayName("Precio Venta")]
        public decimal Precio { get; set; }

        

        public int Stock { get; set; }

        



    }
}
