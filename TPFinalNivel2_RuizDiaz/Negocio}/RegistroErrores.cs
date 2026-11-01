using System;
using System.IO;

namespace Negocio_
{
    // Escribe avisos en log_errores.txt (junto al .exe), con el mismo formato que usan las pantallas.
    // Nunca lanza excepciones: registrar un aviso no debe romper una venta.
    public static class RegistroErrores
    {
        public static void Registrar(string mensaje, string origen)
        {
            try
            {
                string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_errores.txt");

                using (var escritor = new StreamWriter(ruta, true))
                {
                    escritor.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] - ORIGEN: " + origen);
                    escritor.WriteLine("ERROR: " + mensaje);
                    escritor.WriteLine(new string('-', 50));
                }
            }
            catch
            {
                // Si ni siquiera se puede escribir el log, no hacemos nada.
            }
        }
    }
}
