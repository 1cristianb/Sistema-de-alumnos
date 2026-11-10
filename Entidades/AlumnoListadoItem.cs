namespace Entidades
{
    // Modelo de lectura para mostrar cada alumno junto con sus cursos.
    public class AlumnoListadoItem
    {
        public Alumno Alumno { get; set; } = new Alumno();
        public List<Curso> Cursos { get; set; } = new List<Curso>();
    }
}
