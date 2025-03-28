using EventosApi.Configurations;
using EventosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventosApi.Repositories
{
    public class TipoRepository : ITipoRepository
    {
        private readonly AppDbContext _context;

        public TipoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tipo>> GetAllAsync() =>
            await _context.Tipos.ToListAsync();

        public async Task<Tipo?> GetByIdAsync(int id) =>
            await _context.Tipos.FindAsync(id);

        public async Task<Tipo> CreateAsync(Tipo tipo)
        {
            _context.Tipos.Add(tipo);
            await _context.SaveChangesAsync();
            return tipo;
        }

        public async Task<Tipo> UpdateAsync(Tipo tipo)
        {
            _context.Entry(tipo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return tipo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tipo = await _context.Tipos.FindAsync(id);
            if (tipo == null) return false;
            _context.Tipos.Remove(tipo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Tipo>> FindByNombreContainsAsync(string nombre)
        {
            return await _context.Tipos
                .Where(t => t.Nombre.Contains(nombre))
                .ToListAsync();
        }

    }
}
