namespace Presentacion.Helpers
{
    public static class Errores
    {
        public static string Describir(Exception ex)
        {
            return ex is TimeoutException
                ? "No se pudo conectar a MongoDB. Verificá que el servidor esté corriendo."
                : ex.Message;
        }
    }
}
