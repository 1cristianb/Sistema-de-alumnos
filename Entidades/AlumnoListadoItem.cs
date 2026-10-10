using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{    // Un alumno + de qué base viene + sus cursos. No se persiste, solo sirve para mostrar el listado unificado.
    public class AlumnoListadoItem
    {
        public Alumno Alumno { get; set; } = new Alumno();
        public string BaseDatos { get; set; } = string.Empty;
        public List<Curso> Cursos { get; set; } = new List<Curso>();
    }
}
