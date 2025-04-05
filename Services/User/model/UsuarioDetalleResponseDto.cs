using System;
using EventosApi.Models;

namespace EventosApi.Dtos
{
    public class UsuarioDetalleResponseDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string Rol { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public bool Enabled { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public static explicit operator UsuarioDetalleResponseDto(Usuario usuario)
        {
            return new UsuarioDetalleResponseDto
            {
                Username = usuario.Username,
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Rol = usuario.Rol,
                Direccion = usuario.Direccion,
                Enabled = usuario.Enabled,
                FechaRegistro = usuario.FechaRegistro
            };
        }
    }
}
