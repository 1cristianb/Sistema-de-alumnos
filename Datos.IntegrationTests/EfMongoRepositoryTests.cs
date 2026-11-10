using Entidades;

namespace Datos.IntegrationTests;

public class EfMongoRepositoryTests : MongoIntegrationTestBase
{
    [Fact]
    public void Alumno_CrudCompleto_PersisteLosCambios()
    {
        Alumno alumno = CrearAlumno();

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfMongoRepository(context);
            repository.Insertar(alumno);
        }

        Assert.False(string.IsNullOrWhiteSpace(alumno.Id));

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfMongoRepository(context);
            Alumno? guardado = repository.ObtenerPorId(alumno.Id!);

            Assert.NotNull(guardado);
            Assert.Equal(alumno.DNI, guardado.DNI);

            guardado.Telefono = "11 9999-9999";
            repository.Actualizar(guardado);
        }

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfMongoRepository(context);
            Alumno? actualizado = repository.ObtenerPorId(alumno.Id!);

            Assert.NotNull(actualizado);
            Assert.Equal("11 9999-9999", actualizado.Telefono);

            repository.Eliminar(alumno.Id!);
        }

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfMongoRepository(context);
            Assert.Null(repository.ObtenerPorId(alumno.Id!));
        }
    }

    [Fact]
    public void EliminarAlumno_EliminaTambienSusCursos()
    {
        Alumno alumno = CrearAlumno();

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfMongoRepository(context);
            repository.Insertar(alumno);
            repository.InsertarCurso(new Curso
            {
                AlumnoId = alumno.Id!,
                NombreCurso = "Programación Avanzada II",
                Anio = 2026
            });
            repository.InsertarCurso(new Curso
            {
                AlumnoId = alumno.Id!,
                NombreCurso = "Bases de Datos",
                Anio = 2026
            });
        }

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfMongoRepository(context);
            Assert.Equal(2, repository.ListarCursos().Count);

            repository.Eliminar(alumno.Id!);
        }

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfMongoRepository(context);
            Assert.Null(repository.ObtenerPorId(alumno.Id!));
            Assert.Empty(repository.ListarCursos());
        }
    }

    [Fact]
    public void EliminarCurso_NoEliminaElAlumno()
    {
        Alumno alumno = CrearAlumno();
        var curso = new Curso
        {
            NombreCurso = "Bases de Datos",
            Anio = 2026
        };

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfMongoRepository(context);
            repository.Insertar(alumno);

            curso.AlumnoId = alumno.Id!;
            repository.InsertarCurso(curso);
        }

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfMongoRepository(context);
            repository.EliminarCurso(curso.Id!);
        }

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfMongoRepository(context);
            Assert.NotNull(repository.ObtenerPorId(alumno.Id!));
            Assert.Empty(repository.ListarCursos());
        }
    }

    private static Alumno CrearAlumno()
    {
        string identificador =
            Random.Shared.Next(10_000_000, 99_999_999).ToString();

        return new Alumno
        {
            Nombre = "Lucía",
            Apellido = "Fernández",
            DNI = identificador,
            Legajo = "L-1001",
            Email = $"lucia.{identificador}@example.com",
            Telefono = "11 4444-5555",
            Direccion = "San Justo 123",
            Carrera = "Ingeniería en Informática",
            FechaNacimiento = new DateTime(2001, 3, 14),
            PuntajeIngreso = 850
        };
    }
}
