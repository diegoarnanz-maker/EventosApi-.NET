using System;
using EventosApi.Models;

namespace EventosApi.Services.User.model
{
    public class UsuarioCreateDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = "USER";
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? Direccion { get; set; }

        public static explicit operator Usuario(UsuarioCreateDto dto)
        {
            return new Usuario
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password, // Se debe encriptar en el servicio
                Rol = dto.Rol,
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Direccion = dto.Direccion,
                Enabled = true,
                FechaRegistro = DateTime.UtcNow
            };
        }
    }
}
