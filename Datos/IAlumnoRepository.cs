using Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public interface IAlumnoRepository
    {
        void Insertar(Alumno alumno);
        List<Alumno> ListarTodos();
        Alumno? ObtenerPorId(string id);
        void Actualizar(Alumno alumno);
        void Eliminar(string id);

        void InsertarCurso(Curso curso);
        List<Curso> ListarCursos();
        void EliminarCurso(string cursoId);
    }
}