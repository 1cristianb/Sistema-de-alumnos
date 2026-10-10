namespace Presentacion.Helpers
{
    public static class ConfiguracionExtensions
    {
        public static readonly string[] Bases = { "LiteDB", "MongoDB" };

        public static string ObtenerCadena(this IConfiguration config, string tipoBD)
        {
            return tipoBD switch
            {
                "MongoDB" => config.GetConnectionString("MongoDbConnection")
                             ?? throw new InvalidOperationException("Falta 'MongoDbConnection' en appsettings.json."),
                "LiteDB" => config.GetConnectionString("LiteDbConnection")
                            ?? throw new InvalidOperationException("Falta 'LiteDbConnection' en appsettings.json."),
                _ => throw new ArgumentException("Base de datos no soportada")
            };
        }

        // "Todas" (o vacío) => ambas bases; si no, solo la indicada
        public static Dictionary<string, string> ObtenerBases(this IConfiguration config, string? tipoBD)
        {
            return Bases
                .Where(b => string.IsNullOrEmpty(tipoBD) || tipoBD == "Todas" || b == tipoBD)
                .ToDictionary(b => b, b => config.ObtenerCadena(b));
        }
    }

    public static class Errores
    {
        // Mensaje corto y entendible (el de MongoDB cuando está apagado es larguísimo)
        public static string Describir(Exception ex, string tipoBD)
        {
            return ex is TimeoutException
                ? $"No se pudo conectar a {tipoBD}. Verificá que el servidor esté corriendo."
                : ex.Message;
        }
    }
}