using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Management;

namespace Negocio_.Impresion
{
    public static class EstadoImpresoraWmi
    {
       public static EstadoImpresora Consultar(String NombreCola) {
            var resultado = new EstadoImpresora();
            var buscador = new ManagementObjectSearcher(
                $"SELECT WorkOffline, Network, PrinterStatus FROM Win32_Printer WHERE Name = '{NombreCola.Replace("'", "''")}'");

            foreach (ManagementObject printer in buscador.Get())
            {
                resultado.Existe = true;
                resultado.FueraDeLinea = Convert.ToBoolean(printer["WorkOffline"]);
                resultado.EsDeRed = Convert.ToBoolean(printer["Network"]);
            }

            return resultado;
        }

        public static void QuitarSinConexion(String NombreCola){

            var buscador = new ManagementObjectSearcher($"SELECT * FROM Win32_Printer WHERE Name = '{NombreCola.Replace("'", "''")}'");

            foreach (ManagementObject impresora in buscador.Get())
            {
                if ((bool)impresora["WorkOffline"])
                {
                    impresora["WorkOffline"] = false;
                    impresora.Put();   // aplica el cambio, equivale a destildar "Usar impresora sin conexión"
                }

        }
    }
            
  }


    }
    
