using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entidades
{
    public class Alumno
    {
        // Clave generada por el motor NoSQL (MongoDB/LiteDB); no se exige en el alta.
        public string? Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, MinimumLength = 2,
        ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        [RegularExpression(
        @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+([ '-][A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$",
        ErrorMessage = "El nombre solo puede contener letras, espacios, apóstrofes y guiones")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50, MinimumLength = 2,
        ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres")]
        [RegularExpression(
         @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+([ '-][A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$",
        ErrorMessage = "El apellido solo puede contener letras, espacios, apóstrofes y guiones")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 7,
        ErrorMessage = "El DNI debe tener entre 7 y 8 dígitos")]
        [RegularExpression(@"^[0-9]+$",
        ErrorMessage = "El DNI solo debe contener números")]
        public string DNI { get; set; } = string.Empty;

        [Required(ErrorMessage = "El legajo es obligatorio")]
        [RegularExpression(@"^L-\d{1,5}$",
        ErrorMessage = "El legajo debe tener el formato L- seguido de hasta 5 números (Ej: L-1005)")]
        public string Legajo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(30, MinimumLength = 6,
        ErrorMessage = "El teléfono debe tener entre 6 y 30 caracteres")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(150, MinimumLength = 3,
        ErrorMessage = "La dirección debe tener entre 3 y 150 caracteres")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una carrera")]
        [AllowedValues(
            "Licenciatura en Gestión de la Tecnología",
            "Ingeniería en Informática",
            "Tecnicatura Universitaria en Web",
            ErrorMessage = "Debe seleccionar una carrera válida de la lista")]
        public string Carrera { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [FechaNacimientoValida]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El puntaje es obligatorio")]
        [Range(1, 1000, ErrorMessage = "El puntaje debe estar entre 1 y 1000")]
        public int PuntajeIngreso { get; set; }
    }
}
