using AvicolaApp.Data;
using AvicolaApp.Models;
using AvicolaApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AvicolaApp.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;

        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _context.Clientes
                .Where(c => c.Activo)
                .AsNoTracking()
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente?> ObtenerPorCuitAsync(string cuit)
        {
            return await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Cuit == cuit && c.Activo);
        }

        public async Task GuardarAsync(Cliente cliente)
        {
            cliente.FechaRegistro = DateTime.UtcNow;
            cliente.SaldoCuentaCorriente = 0;
            cliente.Activo = true;

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Cliente cliente)
        {
            var clienteExistente = await _context.Clientes.FindAsync(cliente.Id);

            if (clienteExistente != null)
            {
                clienteExistente.NombreRazonSocial = cliente.NombreRazonSocial;
                clienteExistente.Cuit = cliente.Cuit;
                clienteExistente.Domicilio = cliente.Domicilio;
                clienteExistente.Email = cliente.Email;
                clienteExistente.Telefono = cliente.Telefono;
                clienteExistente.Celular = cliente.Celular;
                clienteExistente.Fax = cliente.Fax;
                clienteExistente.Localidad = cliente.Localidad;
                clienteExistente.TipoCliente = cliente.TipoCliente;

                _context.Clientes.Update(clienteExistente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                cliente.Activo = false;
                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> ObtenerTotalAsync()
        {
            return await _context.Clientes.Where(c => c.Activo).CountAsync();
        }
    }
}