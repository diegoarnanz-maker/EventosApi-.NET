using System;
using EventosApi.Models;

namespace EventosApi.Dtos
{
    public class UsuarioResponseDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string Rol { get; set; } = string.Empty;

        public static explicit operator UsuarioResponseDto(Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                Username = usuario.Username,
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Rol = usuario.Rol
            };
        }
    }
}
