using Datos;
using Entidades;

namespace Negocio
{
    public class DataSeeder
    {
        private const string Ingenieria = "Ingeniería en Informática";
        private const string Gestion = "Licenciatura en Gestión de la Tecnología";
        private const string TecWeb = "Tecnicatura Universitaria en Web";

        private readonly IAlumnoRepository _repository;
        private readonly UsuarioService _usuarioService;

        public DataSeeder(
            IAlumnoRepository repository,
            UsuarioService usuarioService)
        {
            _repository = repository;
            _usuarioService = usuarioService;
        }

        public List<string> Sembrar()
        {
            var log = new List<string>();

            try
            {
                if (_usuarioService.CrearUsuarioInicial())
                    log.Add("Usuario inicial creado: admin / admin123");
            }
            catch (Exception ex)
            {
                log.Add($"No se pudo crear el usuario inicial: {ex.Message}");
            }

            SembrarAlumnos(AlumnosEjemplo(), log);
            return log;
        }

        private void SembrarAlumnos(
            List<(Alumno, List<Curso>)> datos,
            List<string> log)
        {
            try
            {
                if (_repository.ListarTodos().Count > 0)
                {
                    log.Add("MongoDB: ya tiene datos, no se cargan ejemplos.");
                    return;
                }

                foreach ((Alumno alumno, List<Curso> cursos) in datos)
                {
                    _repository.Insertar(alumno);

                    foreach (Curso curso in cursos)
                    {
                        curso.AlumnoId = alumno.Id!;
                        _repository.InsertarCurso(curso);
                    }
                }

                log.Add(
                    $"MongoDB: se cargaron {datos.Count} alumnos de ejemplo.");
            }
            catch (TimeoutException)
            {
                log.Add(
                    "MongoDB: no se pudo conectar, se omiten los datos de ejemplo.");
            }
            catch (Exception ex)
            {
                log.Add($"MongoDB: error al cargar ejemplos: {ex.Message}");
            }
        }

        private static List<(Alumno, List<Curso>)> AlumnosEjemplo() => new()
        {
            Crear("Sofía", "Martínez", "43111222", "L-2001", Gestion,
                new DateTime(2003, 5, 18), 780,
                "sofia.martinez@alumno.unlam.edu.ar", "11 4411-5064",
                "Illia 1200, González Catán",
                ("Programación Avanzada II", 2026),
                ("Gestión de Proyectos", 2026)),
            Crear("Joaquín", "Ramírez", "42222333", "L-2002", Ingenieria,
                new DateTime(2002, 9, 9), 650,
                "joaquin.ramirez@alumno.unlam.edu.ar", "11 5522-6075",
                "Brown 90, San Justo", ("Bases de Datos", 2026)),
            Crear("Valentina", "Sosa", "41333444", "L-2003", TecWeb,
                new DateTime(2001, 12, 1), 890,
                "valentina.sosa@alumno.unlam.edu.ar", "11 4433-7086",
                "Belgrano 410, Morón", ("Diseño Web", 2026),
                ("Programación Avanzada II", 2026)),
            Crear("Tomás", "Acosta", "40444555", "L-2004", Ingenieria,
                new DateTime(2000, 4, 27), 430,
                "tomas.acosta@alumno.unlam.edu.ar", "11 6644-8097",
                "Salta 2300, Laferrere")
        };

        private static (Alumno, List<Curso>) Crear(
            string nombre,
            string apellido,
            string dni,
            string legajo,
            string carrera,
            DateTime nacimiento,
            int puntaje,
            string email,
            string telefono,
            string direccion,
            params (string curso, int anio)[] cursos)
        {
            var alumno = new Alumno
            {
                Nombre = nombre,
                Apellido = apellido,
                DNI = dni,
                Legajo = legajo,
                Carrera = carrera,
                FechaNacimiento = nacimiento,
                PuntajeIngreso = puntaje,
                Email = email,
                Telefono = telefono,
                Direccion = direccion
            };

            var listaCursos = new List<Curso>();
            foreach ((string curso, int anio) in cursos)
                listaCursos.Add(new Curso { NombreCurso = curso, Anio = anio });

            return (alumno, listaCursos);
        }
    }
}
