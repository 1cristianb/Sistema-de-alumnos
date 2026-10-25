namespace Datos
{
    public interface IRepositoryFactory
    {
        IAlumnoRepository CrearAlumnoRepository(string tipoBD);
        IUsuarioRepository CrearUsuarioRepository();
    }
}