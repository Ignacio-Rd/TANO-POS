using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class InformeCaja
    {

        public decimal EfectivoVentas { get; set; }     // Ventas cash del día
        public decimal CobroDeudores { get; set; }      // Lo que pagaron de deudas viejas
        public decimal VentasFiadasNuevas { get; set; } // Lo que se llevaron sin pagar hoy
        public decimal SaldoHistorico { get; set; }     // El total que me debe todo el mundo
        public decimal PagosAProveedores { get; set; } // Lo que salió de la caja

        // Propiedad calculada: Lo que tenés que tener en la mano
        public decimal EfectivoTotalReal => EfectivoVentas;
        public List<MetodoResumen> DesgloseMetodos { get; set; } = new List<MetodoResumen>();
        public decimal GastosVariosEfectivo { get; set; }  // Gastos varios pagados en efectivo
        public decimal FondoInicialEfectivo { get; set; }


        // En la clase InformeCaja
        public decimal TotalGeneralRecaudado
        {
            get
            {
                // Solo sumamos lo que ES DINERO REAL (Cash + Digital)
                decimal digital = DesgloseMetodos.Sum(x => x.Total);
                return EfectivoVentas + digital;
                // Nota: EfectivoVentas ya incluye CobroDeudores y resta PagosProveedores
            }
            set { }

            
        }

    }

    public class MetodoResumen
    {
        public string Nombre { get; set; }
        public decimal Total { get; set; }
        public decimal GastosVarios { get; set; }       // Gastos varios con este método
        public decimal PagosProveedores { get; set; }   // Pagos proveedores con este método
        public decimal FondoInicial { get; set; }       // fondo_inicial de metodos_de_pago
    }
}
   
