using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Dtos;
using EventosApi.Models;

namespace EventosApi.Services.Auth
{
    public interface IAuthService
    {
        Task<Usuario?> LoginAsync(LoginRequestDto dto);
        Task<Usuario> RegisterAsync(RegisterRequestDto dto);
        Task<Usuario?> GetUsuarioAutenticadoAsync(string username);

    }
}