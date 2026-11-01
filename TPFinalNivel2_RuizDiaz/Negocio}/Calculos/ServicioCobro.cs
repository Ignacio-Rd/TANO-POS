using System;
using System.Collections.Generic;
using Dominio;

namespace Negocio_.Calculos
{
    public class ResultadoCobro
    {
        public decimal Subtotal { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal Recargo { get; set; }
        public decimal Total { get; set; }
    }

    // Arma el total de una venta según el método de pago. Depende solo de IPorcentajeMetodoPago,
    // así que se puede probar sin base de datos.
    public class ServicioCobro
    {
        private readonly IPorcentajeMetodoPago _porcentajes;

        public ServicioCobro(IPorcentajeMetodoPago porcentajes)
        {
            if (porcentajes == null)
                throw new ArgumentNullException(nameof(porcentajes));

            _porcentajes = porcentajes;
        }

        // Si no hay método elegido, el importe queda igual (y no se consulta nada).
        public decimal AplicarRecargo(decimal importe, string metodo)
        {
            if (string.IsNullOrEmpty(metodo))
                return importe;

            return CalculadoraVenta.AplicarRecargo(importe, _porcentajes.ObtenerPorcentaje(metodo));
        }

        public ResultadoCobro Calcular(IEnumerable<Articulos> articulos, string metodo)
        {
            decimal subtotal = CalculadoraVenta.Subtotal(articulos);
            decimal porcentaje = string.IsNullOrEmpty(metodo) ? 0m : _porcentajes.ObtenerPorcentaje(metodo);
            decimal total = CalculadoraVenta.AplicarRecargo(subtotal, porcentaje);

            return new ResultadoCobro
            {
                Subtotal = subtotal,
                Porcentaje = porcentaje,
                Recargo = total - subtotal,
                Total = total
            };
        }
    }
}
