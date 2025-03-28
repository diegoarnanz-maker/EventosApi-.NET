using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Dtos
{
    public class RegisterRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? Direccion { get; set; }
    }
}