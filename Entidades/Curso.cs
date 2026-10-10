using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entidades
{
    public class Curso
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "El alumno es obligatorio")]
        public string AlumnoId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del curso es obligatorio")]
        public string NombreCurso { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(2000, 2100, ErrorMessage = "El año debe estar entre 2000 y 2100")]
        public int Anio { get; set; }
    }
}
