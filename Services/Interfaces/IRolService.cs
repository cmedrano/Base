using AvicolaApp.Models;

namespace AvicolaApp.Services
{
    public interface IRolService
    {
        Task<List<Rol>> ObtenerTodosAsync();
    }
}