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

            List<Alumno> alumnos = _repository.ListarTodos();

            if (alumnos.Any(a => a.DNI == alumno.DNI))
                throw new InvalidOperationException(
                    $"Ya existe un alumno con el DNI {alumno.DNI}.");

            if (TieneLegajoDuplicado(alumnos, alumno))
                throw new InvalidOperationException(
                    $"Ya existe un alumno con el legajo {alumno.Legajo}.");

            if (TieneEmailDuplicado(alumnos, alumno))
                throw new InvalidOperationException(
                    $"Ya existe un alumno con el email {alumno.Email}.");

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

            List<Alumno> alumnos = _repository.ListarTodos();

            if (alumnos.Any(a =>
                a.DNI == alumno.DNI && a.Id != alumno.Id))
            {
                throw new InvalidOperationException(
                    $"Otro alumno ya tiene el DNI {alumno.DNI}.");
            }

            if (TieneLegajoDuplicado(alumnos, alumno))
                throw new InvalidOperationException(
                    $"Otro alumno ya tiene el legajo {alumno.Legajo}.");

            if (TieneEmailDuplicado(alumnos, alumno))
                throw new InvalidOperationException(
                    $"Otro alumno ya tiene el email {alumno.Email}.");

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

        private static bool TieneLegajoDuplicado(
        List<Alumno> alumnos,
        Alumno alumno)
        {
            if (string.IsNullOrWhiteSpace(alumno.Legajo))
                return false;

            return alumnos.Any(existente =>
                existente.Id != alumno.Id &&
                string.Equals(
                    existente.Legajo?.Trim(),
                    alumno.Legajo.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }

        // Comprueba si otro alumno ya utiliza el mismo email.
        private static bool TieneEmailDuplicado(
            List<Alumno> alumnos,
            Alumno alumno)
        {
            if (string.IsNullOrWhiteSpace(alumno.Email))
                return false;

            return alumnos.Any(existente =>
                existente.Id != alumno.Id &&
                string.Equals(
                    existente.Email?.Trim(),
                    alumno.Email.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }
    }
}
    