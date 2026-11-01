using System;
using System.IO;

namespace Negocio_
{
    // Escribe avisos en log_errores.txt (junto al .exe), con el mismo formato que usan las pantallas.
    // Nunca lanza excepciones: registrar un aviso no debe romper una venta.
    public static class RegistroErrores
    {
        // Cuando el archivo pasa de este tamaño se guarda como log_errores.anterior.txt y se empieza uno nuevo
        // (en un local que abre las 24 horas, un log sin límite termina creciendo para siempre).
        private const long TamanoMaximoBytes = 5L * 1024 * 1024;

        private static readonly object Candado = new object();

        public static string Ruta
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_errores.txt"); }
        }

        public static void Registrar(string mensaje, string origen)
        {
            Escribir(origen, "ERROR: " + mensaje);
        }

        // Con el detalle completo (tipo, mensaje, pila de llamadas y excepciones internas), útil para diagnosticar.
        public static void Registrar(Exception excepcion, string origen)
        {
            Escribir(origen, "ERROR: " + (excepcion == null ? "(sin detalle)" : excepcion.ToString()));
        }

        private static void Escribir(string origen, string contenido)
        {
            try
            {
                lock (Candado)
                {
                    string ruta = Ruta;
                    Rotar(ruta);

                    using (var escritor = new StreamWriter(ruta, true))
                    {
                        escritor.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] - ORIGEN: " + origen);
                        escritor.WriteLine(contenido);
                        escritor.WriteLine(new string('-', 50));
                    }
                }
            }
            catch
            {
                // Si ni siquiera se puede escribir el log, no hacemos nada.
            }
        }

        private static void Rotar(string ruta)
        {
            var archivo = new FileInfo(ruta);
            if (!archivo.Exists || archivo.Length < TamanoMaximoBytes)
                return;

            string anterior = Path.Combine(archivo.DirectoryName, "log_errores.anterior.txt");
            if (File.Exists(anterior))
                File.Delete(anterior);
            File.Move(ruta, anterior);
        }
    }
}
