using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    // Valida que la fecha exista, no sea futura y tenga una antigüedad razonable.
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public sealed class FechaNacimientoValidaAttribute : ValidationAttribute
    {
        private const int EdadMaxima = 120;

        public FechaNacimientoValidaAttribute()
            : base("La fecha de nacimiento no es válida.")
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string campo = validationContext.MemberName ?? string.Empty;

            if (value is not DateTime fecha)
                return new ValidationResult("La fecha de nacimiento no es válida.", new[] { campo });

            if (fecha == default)
                return new ValidationResult("La fecha de nacimiento es obligatoria.", new[] { campo });

            if (fecha.Date > DateTime.Today)
                return new ValidationResult("La fecha de nacimiento no puede ser del futuro.", new[] { campo });

            if (fecha.Date < DateTime.Today.AddYears(-EdadMaxima))
                return new ValidationResult(
                    $"La fecha de nacimiento no puede superar los {EdadMaxima} años de antigüedad.",
                    new[] { campo });

            return ValidationResult.Success;
        }
    }
}
