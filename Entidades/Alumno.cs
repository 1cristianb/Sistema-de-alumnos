using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entidades
{
    public class Alumno
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "El legajo es obligatorio")]
        public string Legajo { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una carrera")]
        public string Carrera { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El puntaje es obligatorio")]
        [Range(1, 1000, ErrorMessage = "El puntaje debe estar entre 1 y 1000")]
        public int PuntajeIngreso { get; set; }
    }
}
