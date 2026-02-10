using AvicolaApp.Models;
using AvicolaApp.Models.DTOs;

namespace AvicolaApp.Services.Interfaces
{
    public interface IClienteService
    {
        Task<List<Cliente>> ObtenerTodosAsync();
        Task<Cliente?> ObtenerPorIdAsync(int id);
        Task<Cliente?> ObtenerPorCuitAsync(string cuit);
        Task<Cliente?> ObtenerPorEmailAsync(string email);
        Task GuardarAsync(Cliente cliente);
        Task ActualizarAsync(Cliente cliente);
        Task EliminarAsync(int id);
        Task<int> ObtenerTotalAsync();
        Task<PaginatedResult<Cliente>> ObtenerPaginadosAsync(int pageNumber, int pageSize, string? searchNombre = null, string? searchFantasia = null);
    }
}