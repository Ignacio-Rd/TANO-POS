namespace Negocio_.Calculos
{
    // Permite calcular recargos sin depender de la base: en producción lo implementa
    // MetodoDePagoNegocio (lee la tabla metodos_de_pago) y en las pruebas se reemplaza por un mock.
    public interface IPorcentajeMetodoPago
    {
        decimal ObtenerPorcentaje(string metodo);
    }
}
