using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Negocio_.Impresion
{
    public class ResultadoImpresion
    {
        public bool Exito { get; set; }
        public string Error { get; set; }
    }

    public class  ServicioImpresion
    {
        public async Task<ResultadoImpresion> ImprimirAsync (string nombreCola, Action<PrintDocument> ConfigurarPagina)
        {
            var estado = EstadoImpresoraWmi.Consultar(nombreCola);

            if (!estado.Existe)
                return new ResultadoImpresion { Exito = false, Error =
                $"La impresora '{nombreCola}' no está instalada en Windows."
                };

            if (estado.FueraDeLinea)
            {
                EstadoImpresoraWmi.QuitarSinConexion(nombreCola); 
            }

            int intentos = estado.EsDeRed ? 3 : 1;

            int esperaMs = 800;

            for (int intento = 1; intento <= intentos; intento++)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        var pd = new PrintDocument();
                        pd.PrinterSettings.PrinterName = nombreCola;
                        ConfigurarPagina(pd);
                        pd.Print();
                    });

                    return new ResultadoImpresion { Exito = true };
                }
                catch (Exception ex) when (intento < intentos)
                {
                    Thread.Sleep(esperaMs);
                    esperaMs *= 2;   // 800ms, luego 1600ms, etc.
                }
                catch (Exception ex)
                {
                    return new ResultadoImpresion { Exito = false, Error = ex.Message };
                }
            }

            return new ResultadoImpresion { Exito = false, Error = "No se pudo imprimir tras varios intentos." };
        }
    }
}
        
   


