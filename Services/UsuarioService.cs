using AvicolaApp.Data;
using AvicolaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AvicolaApp.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _context.Usuarios
                .Where(u => u.Activo)
                .Include(u => u.Rol)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .Where(u => u.Activo)
                .Include(u => u.Rol)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> ObtenerPorNombreOEmailAsync(string nombreOEmail)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Activo)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.UserName == nombreOEmail || u.UserEmail == nombreOEmail);
            
            // Asegurar que el Rol está completamente cargado
            if (usuario != null && usuario.Rol == null)
            {
                await _context.Entry(usuario).Reference(u => u.Rol).LoadAsync();
            }
            
            return usuario;
        }

        public async Task GuardarAsync(Usuario usuario)
        {
            usuario.CreateDate = DateTime.UtcNow;
            usuario.Activo = true;
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Usuario usuario)
        {
            try
            {
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                
                // Recargar la entidad para incluir las relaciones (Rol)
                await _context.Entry(usuario).ReloadAsync();
                await _context.Entry(usuario).Reference(u => u.Rol).LoadAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar usuario: {ex.Message}", ex);
            }
        }

        public async Task EliminarLogicamenteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.Activo = false;
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> ObtenerTotalAsync()
        {
            return await _context.Usuarios
                .Where(u => u.Activo)
                .CountAsync();
        }
    }
}