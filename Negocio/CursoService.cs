using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Negocio
{
    public class CursoService
    {
        // Alta del curso + asociación con su alumno (Curso.AlumnoId)
        public void AltaCurso(Curso curso, string tipoBD, string cadenaConexion)
        {
            IAlumnoRepository repo = DatabaseFactory.GetRepository(tipoBD, cadenaConexion);

            if (repo.ObtenerPorId(curso.AlumnoId) == null)
                throw new InvalidOperationException($"El alumno indicado no existe en {tipoBD}.");

            bool repetido = repo.ListarCursos().Any(c =>
                c.AlumnoId == curso.AlumnoId &&
                c.Anio == curso.Anio &&
                string.Equals(c.NombreCurso, curso.NombreCurso, StringComparison.OrdinalIgnoreCase));

            if (repetido)
                throw new InvalidOperationException("El alumno ya está inscripto en ese curso para ese año.");

            repo.InsertarCurso(curso);
        }

        public void EliminarCurso(string cursoId, string tipoBD, string cadenaConexion)
        {
            IAlumnoRepository repo = DatabaseFactory.GetRepository(tipoBD, cadenaConexion);
            repo.EliminarCurso(cursoId);
        }
    }
}