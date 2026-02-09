using AvicolaApp.Models;

namespace AvicolaApp.Services.Interfaces
{
    public interface IAutenticacionService
    {
        Task<Usuario?> ObtenerUsuarioPorNombreOEmailAsync(string nombreOEmail);
        string HashearPassword(string password);
        bool VerificarPassword(string passwordIngresado, string hashAlmacenado);
    }
}