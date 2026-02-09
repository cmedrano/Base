using AvicolaApp.Models;

namespace AvicolaApp.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ObtenerTodosAsync();
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario?> ObtenerPorNombreOEmailAsync(string nombreOEmail);
        Task GuardarAsync(Usuario usuario);
        Task ActualizarAsync(Usuario usuario);
        Task EliminarLogicamenteAsync(int id);
        Task<int> ObtenerTotalAsync();
    }
}