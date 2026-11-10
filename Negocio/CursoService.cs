using Datos;
using Entidades;

namespace Negocio
{
    public class CursoService
    {
        private readonly IAlumnoRepository _repository;

        public CursoService(IAlumnoRepository repository)
        {
            _repository = repository;
        }

        public void AltaCurso(Curso curso)
        {
            if (_repository.ObtenerPorId(curso.AlumnoId) == null)
                throw new InvalidOperationException(
                    "El alumno indicado no existe.");

            bool repetido = _repository.ListarCursos().Any(c =>
                c.AlumnoId == curso.AlumnoId &&
                c.Anio == curso.Anio &&
                string.Equals(
                    c.NombreCurso,
                    curso.NombreCurso,
                    StringComparison.OrdinalIgnoreCase));

            if (repetido)
                throw new InvalidOperationException(
                    "El alumno ya está inscripto en ese curso para ese año.");

            _repository.InsertarCurso(curso);
        }

        public void EliminarCurso(string cursoId)
        {
            _repository.EliminarCurso(cursoId);
        }
    }
}
