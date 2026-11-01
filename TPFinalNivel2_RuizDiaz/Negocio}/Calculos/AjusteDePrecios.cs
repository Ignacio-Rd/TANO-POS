using System;

namespace Negocio_.Calculos
{
    // Arma el UPDATE de precios para "Cambiar precios" (por código, marca o rubro).
    // Antes la misma tabla de tipos estaba copiada en tres clases y un tipo desconocido generaba SQL inválido.
    public static class AjusteDePrecios
    {
        public static string SentenciaUpdate(string tipoAjuste)
        {
            switch (tipoAjuste)
            {
                case "PORC_COSTOVENTA":
                    return "UPDATE Articulos SET PrecioCosto *= (1 + @val/100), Precio *= (1 + @val/100)";
                case "CIFRA_COSTOVENTA":
                    return "UPDATE Articulos SET PrecioCosto += @val, Precio += @val";
                case "PORC_VENTA":
                    return "UPDATE Articulos SET Precio *= (1 + @val/100)";
                case "CIFRA_VENTA":
                    return "UPDATE Articulos SET Precio += @val";
                case "PORC_COSTO":
                    return "UPDATE Articulos SET PrecioCosto *= (1 + @val/100)";
                case "CIFRA_COSTO":
                    return "UPDATE Articulos SET PrecioCosto += @val";
                default:
                    throw new ArgumentException(
                        "Tipo de ajuste de precio desconocido: '" + tipoAjuste + "'.", nameof(tipoAjuste));
            }
        }
    }
}
