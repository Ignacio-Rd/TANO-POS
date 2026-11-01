using System.Configuration;

namespace Negocios
{
    // Único lugar donde se obtiene la cadena de conexión a la base.
    // Se lee del App.config de la aplicación: <connectionStrings> con name="CatalogoDB".
    internal static class ConexionConfig
    {
        private const string NombreConexion = "CatalogoDB";

        // Respaldo para instalaciones cuyo App.config todavía no trae la cadena (así no se rompe un cliente en producción).
        private const string CadenaPorDefecto = "server=.\\SQLEXPRESS; database=CATALOGO_DB_RECUPERADA; integrated security=true";

        public static string Cadena
        {
            get
            {
                var configurada = ConfigurationManager.ConnectionStrings[NombreConexion];
                return configurada != null ? configurada.ConnectionString : CadenaPorDefecto;
            }
        }
    }
}
