using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace TPFinalNivel2_RuizDiaz.Infraestructura
{
    // Garantiza que el programa esté abierto UNA sola vez en la computadora: dos cajas abiertas sobre la misma base
    // se pisan entre sí y confunden a quien atiende. Se usa un mutex con nombre del sistema operativo.
    public sealed class InstanciaUnica : IDisposable
    {
        // "Global\" => vale para toda la computadora, aunque haya varias sesiones de Windows abiertas.
        public const string NombreAplicacion = @"Global\TANO_POS_INSTANCIA_UNICA";

        private Mutex _mutex;

        private InstanciaUnica(Mutex mutex)
        {
            _mutex = mutex;
        }

        // Devuelve null si el programa ya está abierto (en esta u otra sesión de Windows).
        public static InstanciaUnica Tomar(string nombre)
        {
            try
            {
                bool esNueva;
                var mutex = new Mutex(true, nombre, out esNueva);

                if (!esNueva)
                {
                    mutex.Dispose();
                    return null;
                }

                return new InstanciaUnica(mutex);
            }
            catch (UnauthorizedAccessException)
            {
                // El mutex existe pero lo creó otro usuario de Windows: también cuenta como "ya está abierto".
                return null;
            }
        }

        public void Dispose()
        {
            if (_mutex == null)
                return;

            try { _mutex.ReleaseMutex(); }
            catch (ApplicationException) { }

            _mutex.Dispose();
            _mutex = null;
        }

        // Trae al frente la ventana de la otra copia. Devuelve false si no se encontró ninguna ventana (por ejemplo, si todavía está arrancando).
        public static bool TraerAlFrenteLaOtra()
        {
            Process actual = Process.GetCurrentProcess();

            foreach (Process otro in Process.GetProcessesByName(actual.ProcessName))
            {
                if (otro.Id == actual.Id)
                    continue;

                IntPtr ventana = otro.MainWindowHandle;
                if (ventana == IntPtr.Zero)
                    continue;

                if (IsIconic(ventana))
                    ShowWindow(ventana, SW_RESTORE);

                return SetForegroundWindow(ventana);
            }

            return false;
        }

        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
    }
}
