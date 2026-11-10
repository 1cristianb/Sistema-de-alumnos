using Entidades;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Datos;

public class EfMongoRepository : IAlumnoRepository
{
    private readonly UniversidadDbContext _context;

    public EfMongoRepository(UniversidadDbContext context)
    {
        _context = context;
    }

    public void Insertar(Alumno alumno)
    {
        alumno.Id = ObjectId.GenerateNewId().ToString();

        _context.Alumnos.Add(alumno);
        _context.SaveChanges();
    }

    public List<Alumno> ListarTodos()
    {
        return _context.Alumnos
            .AsNoTracking()
            .ToList();
    }

    public Alumno? ObtenerPorId(string id)
    {
        return _context.Alumnos
            .AsNoTracking()
            .FirstOrDefault(alumno => alumno.Id == id);
    }

    public void Actualizar(Alumno alumno)
    {
        _context.Alumnos.Update(alumno);
        _context.SaveChanges();
    }

    public void Eliminar(string id)
    {
        _context.Cursos
            .Where(curso => curso.AlumnoId == id)
            .ExecuteDelete();

        Alumno? alumno = _context.Alumnos
            .FirstOrDefault(alumno => alumno.Id == id);

        if (alumno != null)
        {
            _context.Alumnos.Remove(alumno);
            _context.SaveChanges();
        }
    }

    public void InsertarCurso(Curso curso)
    {
        curso.Id = ObjectId.GenerateNewId().ToString();

        _context.Cursos.Add(curso);
        _context.SaveChanges();
    }

    public List<Curso> ListarCursos()
    {
        return _context.Cursos
            .AsNoTracking()
            .ToList();
    }

    public void EliminarCurso(string cursoId)
    {
        Curso? curso = _context.Cursos
            .FirstOrDefault(curso => curso.Id == cursoId);

        if (curso != null)
        {
            _context.Cursos.Remove(curso);
            _context.SaveChanges();
        }
    }
}
