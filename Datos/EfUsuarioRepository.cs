using Entidades;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Datos;

public class EfUsuarioRepository : IUsuarioRepository
{
    private readonly UniversidadDbContext _context;

    public EfUsuarioRepository(UniversidadDbContext context)
    {
        _context = context;
    }

    public void Insertar(Usuario usuario)
    {
        usuario.Id = ObjectId.GenerateNewId().ToString();

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
    }

    public Usuario? ObtenerPorNombre(string nombreUsuario)
    {
        return _context.Usuarios
            .AsNoTracking()
            .FirstOrDefault(
                usuario => usuario.NombreUsuario == nombreUsuario);
    }

    public int Contar()
    {
        return _context.Usuarios.Count();
    }
}