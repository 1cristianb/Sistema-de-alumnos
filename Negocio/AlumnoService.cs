using Datos;
using Entidades;

namespace Negocio
{
    public class AlumnoService
    {
        public void AltaAlumno(Alumno alumno, string tipoBD, string cadenaConexion)
        {
            ValidarFecha(alumno);
            IAlumnoRepository repo = DatabaseFactory.GetRepository(tipoBD, cadenaConexion);

            if (repo.ListarTodos().Any(a => a.DNI == alumno.DNI))
                throw new InvalidOperationException($"Ya existe un alumno con el DNI {alumno.DNI} en {tipoBD}.");

            repo.Insertar(alumno);
        }

        public Alumno? ObtenerAlumno(string id, string tipoBD, string cadenaConexion)
        {
            IAlumnoRepository repo = DatabaseFactory.GetRepository(tipoBD, cadenaConexion);
            return repo.ObtenerPorId(id);
        }

        public void ActualizarAlumno(Alumno alumno, string tipoBD, string cadenaConexion)
        {
            if (string.IsNullOrEmpty(alumno.Id))
                throw new InvalidOperationException("El alumno no tiene identificador.");

            ValidarFecha(alumno);
            IAlumnoRepository repo = DatabaseFactory.GetRepository(tipoBD, cadenaConexion);

            if (repo.ListarTodos().Any(a => a.DNI == alumno.DNI && a.Id != alumno.Id))
                throw new InvalidOperationException($"Otro alumno ya tiene el DNI {alumno.DNI} en {tipoBD}.");

            repo.Actualizar(alumno);
        }

        public void EliminarAlumno(string id, string tipoBD, string cadenaConexion)
        {
            IAlumnoRepository repo = DatabaseFactory.GetRepository(tipoBD, cadenaConexion);
            repo.Eliminar(id);
        }

        // Lista alumnos de TODAS las bases recibidas (tipoBD -> cadena) y aplica el buscador.
        // Si una base falla (ej: MongoDB apagado) no se cae: devuelve igual lo de las otras con una advertencia.
        public ResultadoListado ListarAlumnos(Dictionary<string, string> bases, string? dni, string? apellido)
        {
            var resultado = new ResultadoListado();

            foreach (var (tipoBD, cadena) in bases)
            {
                try
                {
                    IAlumnoRepository repo = DatabaseFactory.GetRepository(tipoBD, cadena);
                    var cursosPorAlumno = repo.ListarCursos()
                                              .GroupBy(c => c.AlumnoId)
                                              .ToDictionary(g => g.Key, g => g.ToList());

                    foreach (var alumno in repo.ListarTodos())
                    {
                        resultado.Items.Add(new AlumnoListadoItem
                        {
                            Alumno = alumno,
                            BaseDatos = tipoBD,
                            Cursos = alumno.Id != null && cursosPorAlumno.TryGetValue(alumno.Id, out var cursos)
                                        ? cursos
                                        : new List<Curso>()
                        });
                    }
                }
                catch (TimeoutException)
                {
                    resultado.Advertencias.Add($"No se pudo conectar a {tipoBD}. Verificá que el servidor esté corriendo.");
                }
                catch (Exception ex)
                {
                    resultado.Advertencias.Add($"Error al leer {tipoBD}: {ex.Message}");
                }
            }

            IEnumerable<AlumnoListadoItem> filtrados = resultado.Items;

            if (!string.IsNullOrWhiteSpace(dni))
                filtrados = filtrados.Where(i => i.Alumno.DNI != null && i.Alumno.DNI.Contains(dni.Trim()));

            if (!string.IsNullOrWhiteSpace(apellido))
                filtrados = filtrados.Where(i => i.Alumno.Apellido != null &&
                                                 i.Alumno.Apellido.Contains(apellido.Trim(), StringComparison.OrdinalIgnoreCase));

            resultado.Items = filtrados
                .OrderBy(i => i.Alumno.Apellido)
                .ThenBy(i => i.Alumno.Nombre)
                .ToList();

            return resultado;
        }

        private static void ValidarFecha(Alumno alumno)
        {
            if (alumno.FechaNacimiento > DateTime.Now)
                throw new InvalidOperationException("La fecha de nacimiento no puede ser del futuro.");
        }
    }
}