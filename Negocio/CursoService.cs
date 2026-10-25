using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using Entidades;

namespace Negocio
{
    public class CursoService
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public CursoService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public void AltaCurso(Curso curso, string tipoBD)
        {
            IAlumnoRepository repo =
                _repositoryFactory.CrearAlumnoRepository(tipoBD);

            if (repo.ObtenerPorId(curso.AlumnoId) == null)
                throw new InvalidOperationException(
                    $"El alumno indicado no existe en {tipoBD}.");

            bool repetido = repo.ListarCursos().Any(c =>
                c.AlumnoId == curso.AlumnoId &&
                c.Anio == curso.Anio &&
                string.Equals(
                    c.NombreCurso,
                    curso.NombreCurso,
                    StringComparison.OrdinalIgnoreCase));

            if (repetido)
                throw new InvalidOperationException(
                    "El alumno ya está inscripto en ese curso para ese año.");

            repo.InsertarCurso(curso);
        }

        public void EliminarCurso(string cursoId, string tipoBD)
        {
            IAlumnoRepository repo =
                _repositoryFactory.CrearAlumnoRepository(tipoBD);

            repo.EliminarCurso(cursoId);
        }
    }
}