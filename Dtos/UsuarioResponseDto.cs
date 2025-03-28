using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Dtos
{
    public class UsuarioResponseDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
    }
}