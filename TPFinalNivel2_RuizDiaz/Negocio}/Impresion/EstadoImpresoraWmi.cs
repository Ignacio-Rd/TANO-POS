using System;
using System.Linq;
using System.Management;
using Dominio;

namespace Negocio_.Impresion
{
    public static class EstadoImpresoraWmi
    {
        // Win32_Printer.ExtendedPrinterStatus: 8 = pausada
        private const int EstadoPausada = 8;

        // Puertos que NO son de red. Cualquier otro (IP_x.x.x.x, WSD-..., etc.) se considera impresora de red.
        private static readonly string[] PuertosLocales = { "USB", "LPT", "COM", "FILE", "NUL", "PORTPROMPT", "XPSPORT", "MICROSOFT" };

        public static EstadoImpresora Consultar(string nombreCola)
        {
            var resultado = new EstadoImpresora();

            foreach (ManagementObject impresora in BuscarImpresora(nombreCola, "WorkOffline, ExtendedPrinterStatus, PortName"))
            {
                resultado.Existe = true;
                resultado.FueraDeLinea = Convert.ToBoolean(impresora["WorkOffline"]);
                resultado.EnPausa = impresora["ExtendedPrinterStatus"] != null
                                    && Convert.ToInt32(impresora["ExtendedPrinterStatus"]) == EstadoPausada;
                resultado.EsDeRed = EsPuertoDeRed(Convert.ToString(impresora["PortName"]));
            }

            return resultado;
        }

        // Destraba el flag "Usar impresora sin conexión". Si no hay permisos, no es un error: se intenta imprimir igual.
        public static void QuitarSinConexion(string nombreCola)
        {
            try
            {
                foreach (ManagementObject impresora in BuscarImpresora(nombreCola, "*"))
                {
                    if (Convert.ToBoolean(impresora["WorkOffline"]))
                    {
                        impresora["WorkOffline"] = false;
                        impresora.Put();
                    }
                }
            }
            catch (Exception)
            {
                // Best effort.
            }
        }

        // Reanuda la cola si quedó pausada. Mismo criterio: si falla, se intenta imprimir igual.
        public static void Reanudar(string nombreCola)
        {
            try
            {
                foreach (ManagementObject impresora in BuscarImpresora(nombreCola, "*"))
                {
                    impresora.InvokeMethod("Resume", null);
                }
            }
            catch (Exception)
            {
                // Best effort.
            }
        }

        // Devuelve null si el documento ya no está en la cola de Windows (salió); si sigue, devuelve su estado.
        public static string EstadoTrabajo(string nombreDocumento)
        {
            var buscador = new ManagementObjectSearcher("SELECT Document, JobStatus FROM Win32_PrintJob");

            foreach (ManagementObject trabajo in buscador.Get())
            {
                if (Convert.ToString(trabajo["Document"]) == nombreDocumento)
                {
                    string estado = Convert.ToString(trabajo["JobStatus"]);
                    return string.IsNullOrEmpty(estado) ? "En cola" : estado;
                }
            }

            return null;
        }

        private static ManagementObjectCollection BuscarImpresora(string nombreCola, string propiedades)
        {
            string nombre = nombreCola.Replace("\\", "\\\\").Replace("'", "\\'");
            var buscador = new ManagementObjectSearcher($"SELECT {propiedades} FROM Win32_Printer WHERE Name = '{nombre}'");
            return buscador.Get();
        }

        private static bool EsPuertoDeRed(string puerto)
        {
            if (string.IsNullOrEmpty(puerto))
                return false;

            return !PuertosLocales.Any(p => puerto.StartsWith(p, StringComparison.OrdinalIgnoreCase));
        }
    }
}
