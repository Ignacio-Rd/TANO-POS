namespace Negocio_.Calculos
{
    // Cuentas del arqueo de caja, separadas de las consultas SQL de ObtenerResumenInforme.
    public static class ConciliacionCaja
    {
        // Efectivo neto = lo que entró en efectivo (ventas + cobros a deudores) menos lo que salió.
        public static decimal EfectivoNeto(decimal entradasEfectivo, decimal pagosAProveedores, decimal gastosVarios)
        {
            return entradasEfectivo - pagosAProveedores - gastosVarios;
        }

        // Neto de un método digital = ventas + cobros a deudores - pagos a proveedores - gastos varios.
        public static decimal NetoMetodo(decimal ventas, decimal cobros, decimal pagosAProveedores, decimal gastosVarios)
        {
            return ventas + cobros - pagosAProveedores - gastosVarios;
        }
    }
}
