using Datos;
using Entidades;

namespace Negocio
{
    public class AlumnoService
    {
        private readonly IAlumnoRepository _repository;

        public AlumnoService(IAlumnoRepository repository)
        {
            _repository = repository;
        }

        public void AltaAlumno(Alumno alumno)
        {
            ValidarFecha(alumno);

            if (_repository.ListarTodos().Any(a => a.DNI == alumno.DNI))
                throw new InvalidOperationException(
                    $"Ya existe un alumno con el DNI {alumno.DNI}.");

            _repository.Insertar(alumno);
        }

        public Alumno? ObtenerAlumno(string id)
        {
            return _repository.ObtenerPorId(id);
        }

        public void ActualizarAlumno(Alumno alumno)
        {
            if (string.IsNullOrEmpty(alumno.Id))
                throw new InvalidOperationException(
                    "El alumno no tiene identificador.");

            ValidarFecha(alumno);

            if (_repository.ListarTodos()
                .Any(a => a.DNI == alumno.DNI && a.Id != alumno.Id))
            {
                throw new InvalidOperationException(
                    $"Otro alumno ya tiene el DNI {alumno.DNI}.");
            }

            _repository.Actualizar(alumno);
        }

        public void EliminarAlumno(string id)
        {
            _repository.Eliminar(id);
        }

        public List<AlumnoListadoItem> ListarAlumnos(
            string? dni,
            string? apellido)
        {
            Dictionary<string, List<Curso>> cursosPorAlumno =
                _repository.ListarCursos()
                    .GroupBy(curso => curso.AlumnoId)
                    .ToDictionary(grupo => grupo.Key, grupo => grupo.ToList());

            IEnumerable<AlumnoListadoItem> resultado =
                _repository.ListarTodos().Select(alumno =>
                    new AlumnoListadoItem
                    {
                        Alumno = alumno,
                        Cursos = alumno.Id != null &&
                                  cursosPorAlumno.TryGetValue(
                                      alumno.Id,
                                      out List<Curso>? cursos)
                            ? cursos
                            : new List<Curso>()
                    });

            if (!string.IsNullOrWhiteSpace(dni))
            {
                resultado = resultado.Where(item =>
                    item.Alumno.DNI.Contains(dni.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(apellido))
            {
                resultado = resultado.Where(item =>
                    item.Alumno.Apellido.Contains(
                        apellido.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            return resultado
                .OrderBy(item => item.Alumno.Apellido)
                .ThenBy(item => item.Alumno.Nombre)
                .ToList();
        }

        private static void ValidarFecha(Alumno alumno)
        {
            if (alumno.FechaNacimiento > DateTime.Now)
                throw new InvalidOperationException(
                    "La fecha de nacimiento no puede ser del futuro.");
        }
    }
}
