using System.Collections.Generic;
using System.Threading.Tasks;
using EventosApi.Models;

namespace EventosApi.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByUsernameAsync(string username);
        Task<Usuario> CreateAsync(Usuario usuario);
        Task<Usuario> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(string username);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<IEnumerable<Usuario>> FindByNombreAsync(string nombre);
        Task<IEnumerable<Usuario>> FindByRolAsync(string rol);
    }
}
