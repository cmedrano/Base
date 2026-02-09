using AvicolaApp.Services.Interfaces;
using AvicolaApp.Models;
using BCrypt.Net;

namespace AvicolaApp.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly IUsuarioService _usuarioService;

        public AutenticacionService(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task<Usuario?> ObtenerUsuarioPorNombreOEmailAsync(string nombreOEmail)
        {
            return await _usuarioService.ObtenerPorNombreOEmailAsync(nombreOEmail);
        }

        public string HashearPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public bool VerificarPassword(string passwordIngresado, string hashAlmacenado)
        {
            try
            {
                // Solo aceptar hashes BCrypt válidos
                if (!EsHashBCryptValido(hashAlmacenado))
                {
                    return false;
                }

                return BCrypt.Net.BCrypt.Verify(passwordIngresado, hashAlmacenado);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica que el hash sea un BCrypt válido
        /// </summary>
        private bool EsHashBCryptValido(string hash)
        {
            if (string.IsNullOrEmpty(hash))
                return false;

            return hash.StartsWith("$2a$") ||
                   hash.StartsWith("$2b$") ||
                   hash.StartsWith("$2x$") ||
                   hash.StartsWith("$2y$") ||
                   hash.StartsWith("$2c$");
        }
    }
}