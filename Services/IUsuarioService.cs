using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Dtos;
using EventosApi.Models;

namespace EventosApi.Services
{
    public interface IUsuarioService : IGenericoService<Usuario, int>
    {
        Task<bool> ExistsByUsernameAsync(string username);
        string HashPassword(Usuario usuario, string rawPassword);
        Task<Usuario> RegisterAsync(RegisterRequestDto dto);
        Task<Usuario?> FindByUsernameAsync(string username);

    }
}