using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    public class Usuario
    {
        public string? Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;

        // Formato: iteraciones.salt.hash (nunca se guarda la contraseña en texto plano)
        public string PasswordHash { get; set; } = string.Empty;
    }
}
