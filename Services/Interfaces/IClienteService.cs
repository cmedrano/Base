using AvicolaApp.Models;

namespace AvicolaApp.Services.Interfaces
{
    public interface IClienteService
    {
        Task<List<Cliente>> ObtenerTodosAsync();
        Task<Cliente?> ObtenerPorIdAsync(int id);
        Task<Cliente?> ObtenerPorCuitAsync(string cuit);
        Task GuardarAsync(Cliente cliente);
        Task ActualizarAsync(Cliente cliente);
        Task EliminarAsync(int id);
        Task<int> ObtenerTotalAsync();
    }
}