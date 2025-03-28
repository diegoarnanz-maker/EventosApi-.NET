using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Models
{
    public class Usuario
    {
        [Key]
        [MaxLength(45)]
        public string Username { get; set; }

        [Required, MaxLength(45)]
        public string Password { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(30)]
        public string? Nombre { get; set; }

        [MaxLength(45)]
        public string? Apellidos { get; set; }

        [MaxLength(100)]
        public string? Direccion { get; set; }

        public bool Enabled { get; set; } = true;

        public DateTime? FechaRegistro { get; set; }

        // Relación muchos a muchos con Perfil (sin clase intermedia)
        public ICollection<Perfil> Perfiles { get; set; }
    }
}