using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio_;

namespace TPFinalNivel2_RuizDiaz.Infraestructura
{
    // Red de seguridad para errores que ninguna pantalla atrapó. Antes, una excepción así mostraba el cuadro técnico
    // de .NET o cerraba el programa en plena venta. Ahora: queda el detalle completo en log_errores.txt, la persona ve
    // un aviso claro y puede seguir trabajando. (Las ventas son "todo o nada", así que un error no deja datos a medias.)
    public static class ManejadorErrores
    {
        private static bool _avisando;

        public static void Instalar()
        {
            // Los errores de pantalla llegan a ThreadException en vez de abrir el diálogo estándar de .NET.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (emisor, e) => Atender(e.Exception, "Error no controlado (pantalla)", false, MostrarAviso);

            // Errores fuera de la pantalla (hilos de fondo, arranque). Si el sistema operativo va a cerrar el programa, avisa antes.
            AppDomain.CurrentDomain.UnhandledException += (emisor, e) =>
                Atender(e.ExceptionObject as Exception, "Error fatal", e.IsTerminating, MostrarAviso);

            // Tareas (async/await) cuyo error nadie leyó: se anotan y se dan por atendidas.
            TaskScheduler.UnobservedTaskException += (emisor, e) =>
            {
                RegistroErrores.Registrar(e.Exception, "Tarea sin observar");
                e.SetObserved();
            };
        }

        // Anota el error y avisa a la persona. 'avisar' se puede reemplazar (las pruebas lo hacen para no abrir ventanas).
        public static void Atender(Exception excepcion, string origen, bool seCierra, Action<string, bool> avisar)
        {
            RegistroErrores.Registrar(excepcion, origen);

            // Si mientras se muestra el aviso salta otro error, solo se anota: no se apilan cuadros de diálogo.
            if (_avisando)
                return;

            _avisando = true;
            try
            {
                avisar(Mensaje(seCierra), seCierra);
            }
            catch
            {
                // Si ni el aviso se pudo mostrar, el error ya quedó anotado.
            }
            finally
            {
                _avisando = false;
            }
        }

        public static string Mensaje(bool seCierra)
        {
            string comun = "Ocurrió un error inesperado y quedó anotado en el archivo log_errores.txt (carpeta del programa).";

            if (seCierra)
                return comun + Environment.NewLine + Environment.NewLine +
                       "El programa se va a cerrar. Volvé a abrirlo: las ventas ya cobradas están guardadas.";

            return comun + Environment.NewLine + Environment.NewLine +
                   "Podés seguir trabajando: las ventas se guardan completas o no se guardan, así que no quedó nada a medias. " +
                   "Si el error se repite, cerrá y volvé a abrir el programa y avisá.";
        }

        private static void MostrarAviso(string mensaje, bool seCierra)
        {
            MessageBox.Show(mensaje, "TANO POS", MessageBoxButtons.OK, seCierra ? MessageBoxIcon.Stop : MessageBoxIcon.Warning);
        }
    }
}
