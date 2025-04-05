using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Configurations;
using EventosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventosApi.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> GetByUsernameAsync(string username)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Usuarios.AnyAsync(u => u.Username == username);
        }

        public async Task<Usuario?> FindByUsernameAsync(string username)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
