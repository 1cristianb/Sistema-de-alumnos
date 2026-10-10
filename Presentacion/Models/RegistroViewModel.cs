using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El usuario debe tener entre 3 y 30 caracteres")]
        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe repetir la contraseña")]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Repetir contraseña")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}
