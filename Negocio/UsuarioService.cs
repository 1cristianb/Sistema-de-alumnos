using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Negocio
{
    public class UsuarioService
    {
        private const int Iteraciones = 210_000;
        private const int TamanoSalt = 16;
        private const int TamanoHash = 32;

        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public void Registrar(string nombreUsuario, string password)
        {
            string nombre = Normalizar(nombreUsuario);

            if (_repository.ObtenerPorNombre(nombre) != null)
                throw new InvalidOperationException("Ese nombre de usuario ya está en uso.");

            _repository.Insertar(new Usuario { NombreUsuario = nombre, PasswordHash = Hashear(password) });
        }

        // Devuelve el usuario si las credenciales son correctas; null si no.
        public Usuario? Autenticar(string nombreUsuario, string password)
        {
            var usuario = _repository.ObtenerPorNombre(Normalizar(nombreUsuario));

            if (usuario == null || !Verificar(password, usuario.PasswordHash))
                return null;

            return usuario;
        }

        // Crea admin / admin123 si todavía no hay usuarios. Devuelve true si lo creó.
        public bool CrearUsuarioInicial()
        {
            if (_repository.Contar() > 0) return false;

            _repository.Insertar(new Usuario { NombreUsuario = "admin", PasswordHash = Hashear("admin123") });
            return true;
        }

        private static string Normalizar(string nombre) => nombre.Trim().ToLowerInvariant();

        // PBKDF2 con sal aleatoria. Se guarda como "iteraciones.salt.hash"
        private static string Hashear(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(TamanoSalt);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iteraciones, HashAlgorithmName.SHA512, TamanoHash);
            return $"{Iteraciones}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        private static bool Verificar(string password, string guardado)
        {
            var partes = guardado.Split('.');
            if (partes.Length != 3 || !int.TryParse(partes[0], out int iteraciones)) return false;

            byte[] salt = Convert.FromBase64String(partes[1]);
            byte[] hashEsperado = Convert.FromBase64String(partes[2]);
            byte[] hashCalculado = Rfc2898DeriveBytes.Pbkdf2(password, salt, iteraciones, HashAlgorithmName.SHA512, hashEsperado.Length);

            return CryptographicOperations.FixedTimeEquals(hashCalculado, hashEsperado);
        }
    }
}