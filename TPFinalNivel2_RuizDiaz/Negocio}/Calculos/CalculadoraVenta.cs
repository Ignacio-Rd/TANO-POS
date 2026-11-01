using System;
using System.Collections.Generic;
using System.Linq;
using Dominio;

namespace Negocio_.Calculos
{
    // Cálculos puros de una venta (sin base de datos). Reproducen exactamente las cuentas que
    // antes estaban escritas en línea dentro de MetodoDePagoNegocio y Venta_Por_Producto_Negocio.
    public static class CalculadoraVenta
    {
        // Suma de los precios de venta de cada unidad cargada en la registradora.
        public static decimal Subtotal(IEnumerable<Articulos> articulos)
        {
            if (articulos == null)
                return 0m;

            return articulos.Where(a => a != null).Sum(a => a.Precio);
        }

        // Total con recargo del método de pago. NO redondea (igual que antes): la base redondea al guardar.
        public static decimal AplicarRecargo(decimal importe, decimal porcentaje)
        {
            return importe + importe * (porcentaje / 100);
        }

        // Recargo de una línea del ticket, redondeado a 2 decimales (mitades hacia arriba, igual que SQL Server).
        public static decimal RecargoDeLinea(decimal importeBase, decimal porcentaje)
        {
            return Math.Round(importeBase * (porcentaje / 100), 2, MidpointRounding.AwayFromZero);
        }

        // Importe de una línea del ticket con su recargo, redondeado a 2 decimales.
        public static decimal ImporteDeLineaConRecargo(decimal importeBase, decimal porcentaje)
        {
            return Math.Round(importeBase + importeBase * (porcentaje / 100), 2, MidpointRounding.AwayFromZero);
        }
    }
}
