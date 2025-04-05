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
        Task<bool> ExistsByUsernameAsync(string username);

        Task<Usuario?> FindByUsernameAsync(string username);

    }
}
