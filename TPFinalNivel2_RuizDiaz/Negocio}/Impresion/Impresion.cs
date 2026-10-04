using System;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace Negocio_.Impresion
{
    public class ResultadoImpresion
    {
        public bool Exito { get; set; }
        public string Error { get; set; }
    }

    public class ServicioImpresion
    {
        // Cuánto esperamos a que el trabajo salga de la cola de Windows antes de avisar que quedó colgado.
        private const int EsperaVerificacionMs = 6000;
        private const int IntervaloVerificacionMs = 400;

        public async Task<ResultadoImpresion> ImprimirAsync(string nombreCola, Action<PrintDocument> ConfigurarPagina)
        {
            // WMI es lento: va fuera del hilo de la pantalla para no trabar la caja.
            var estado = await Task.Run(() => EstadoImpresoraWmi.Consultar(nombreCola));

            if (!estado.Existe)
                return Fallo($"La impresora '{nombreCola}' no está instalada o cambió de nombre en Windows.");

            if (estado.FueraDeLinea || estado.EnPausa)
            {
                await Task.Run(() =>
                {
                    if (estado.FueraDeLinea) EstadoImpresoraWmi.QuitarSinConexion(nombreCola);
                    if (estado.EnPausa) EstadoImpresoraWmi.Reanudar(nombreCola);
                });
            }

            string nombreDocumento = "Ticket POS " + Guid.NewGuid().ToString("N").Substring(0, 8);
            int intentos = estado.EsDeRed ? 3 : 1;
            int esperaMs = 800;
            string ultimoError = null;
            bool enviado = false;

            // Solo se reintenta si Print() lanza excepción (el trabajo no llegó a la cola).
            // Si el trabajo ya está en la cola NO se reenvía: saldría el ticket duplicado.
            for (int intento = 1; intento <= intentos; intento++)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        var pd = new PrintDocument();
                        pd.PrinterSettings.PrinterName = nombreCola;
                        pd.DocumentName = nombreDocumento;
                        pd.PrintController = new StandardPrintController(); // sin el diálogo "Imprimiendo..."
                        ConfigurarPagina(pd);
                        pd.Print();
                    });

                    enviado = true;
                    break;
                }
                catch (Exception ex)
                {
                    ultimoError = ex.Message;
                }

                if (intento < intentos)
                {
                    await Task.Delay(esperaMs);
                    esperaMs *= 2;   // 800ms, luego 1600ms
                }
            }

            if (!enviado)
                return Fallo(ultimoError ?? "No se pudo imprimir tras varios intentos.");

            return await VerificarSalidaAsync(nombreDocumento);
        }

        // Print() no avisa si el trabajo se queda trabado en la cola, así que lo verificamos.
        private async Task<ResultadoImpresion> VerificarSalidaAsync(string nombreDocumento)
        {
            string ultimoEstado = null;

            try
            {
                for (int espera = 0; espera < EsperaVerificacionMs; espera += IntervaloVerificacionMs)
                {
                    await Task.Delay(IntervaloVerificacionMs);

                    ultimoEstado = await Task.Run(() => EstadoImpresoraWmi.EstadoTrabajo(nombreDocumento));

                    if (ultimoEstado == null)
                        return new ResultadoImpresion { Exito = true };
                }
            }
            catch (Exception)
            {
                // Si no se puede consultar la cola, no alarmamos al cajero: el trabajo ya se envió.
                return new ResultadoImpresion { Exito = true };
            }

            return Fallo($"El ticket quedó en la cola de impresión sin salir (estado: {ultimoEstado}). " +
                         "Revisá que la impresora esté encendida, con papel y conectada.");
        }

        private static ResultadoImpresion Fallo(string error)
        {
            return new ResultadoImpresion { Exito = false, Error = error };
        }
    }
}
