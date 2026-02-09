using AvicolaApp.Data;
using AvicolaApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AvicolaApp.Services
{
    public class RolService : IRolService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const string ROLES_CACHE_KEY = "roles_list";

        public RolService(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<Rol>> ObtenerTodosAsync()
        {
            if (!_cache.TryGetValue(ROLES_CACHE_KEY, out List<Rol>? roles))
            {
                roles = await _context.Roles.AsNoTracking().ToListAsync();
                _cache.Set(ROLES_CACHE_KEY, roles, TimeSpan.FromHours(1));
            }

            return roles!;
        }
    }
}