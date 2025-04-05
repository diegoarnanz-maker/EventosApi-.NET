using System;
using System.Threading.Tasks;
using EventosApi.Dtos;
using EventosApi.Models;
using EventosApi.Repositories;
using Microsoft.AspNetCore.Identity;

namespace EventosApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public AuthService(IUsuarioRepository usuarioRepository, IPasswordHasher<Usuario> passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Usuario?> LoginAsync(LoginRequestDto dto)
        {
            var usuario = await _usuarioRepository.FindByUsernameAsync(dto.Username);
            if (usuario == null || !usuario.Enabled)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, dto.Password);
            return result == PasswordVerificationResult.Failed ? null : usuario;
        }

        public async Task<Usuario> RegisterAsync(RegisterRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("El nombre de usuario y la contraseña son obligatorios.");

            var existingUser = await _usuarioRepository.FindByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new InvalidOperationException("El nombre de usuario ya está en uso.");

            var nuevoUsuario = new Usuario
            {
                Username = dto.Username,
                Email = dto.Email,
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Direccion = dto.Direccion,
                Enabled = true,
                FechaRegistro = DateTime.UtcNow,
                Rol = "USER"
            };

            nuevoUsuario.Password = _passwordHasher.HashPassword(nuevoUsuario, dto.Password);

            return await _usuarioRepository.CreateAsync(nuevoUsuario);
        }

        public async Task<Usuario?> GetUsuarioAutenticadoAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var usuario = await _usuarioRepository.FindByUsernameAsync(username);
            return usuario?.Enabled == true ? usuario : null;
        }
    }
}
