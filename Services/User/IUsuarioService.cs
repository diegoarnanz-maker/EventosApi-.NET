using System.Collections.Generic;
using System.Threading.Tasks;
using EventosApi.Dtos;
using EventosApi.Models;
using EventosApi.Services.Base;
using EventosApi.Services.User.model;

namespace EventosApi.Services
{
    public interface IUsuarioService : IGenericDtoService<Usuario, UsuarioCreateDto, UsuarioResponseDto, string>
    {
        Task<bool> ExistsByUsernameAsync(string username);
        Task<Usuario?> FindByUsernameAsync(string username);
        Task<IEnumerable<UsuarioResponseDto>> FindByNombreAsync(string nombre);
        Task<IEnumerable<UsuarioResponseDto>> FindByRolAsync(string rol);
        Task<UsuarioResponseDto> UpdateAsync(string username, UsuarioUpdateDto dto);
        Task<UsuarioResponseDto> ChangePasswordAsync(string username, UsuarioChangePasswordDto dto);
        Task<UsuarioResponseDto> DesactivarUsuarioAsync(string username);
        Task<UsuarioDetalleResponseDto> GetDetalleByIdAsync(string username);
        Task<UsuarioResponseDto> ActivarUsuarioAsync(string username);

    }
}
