using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventosApi.Models
{
    [Table("USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("USERNAME")]
        [MaxLength(45)]
        public string Username { get; set; }

        [Column("PASSWORD")]
        [StringLength(150)]
        public string Password { get; set; }

        [Required]
        [Column("EMAIL")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Column("NOMBRE")]
        [MaxLength(30)]
        public string? Nombre { get; set; }

        [Column("APELLIDOS")]
        [MaxLength(45)]
        public string? Apellidos { get; set; }

        [Column("DIRECCION")]
        [MaxLength(100)]
        public string? Direccion { get; set; }

        [Column("ENABLED")]
        public bool Enabled { get; set; } = true;

        [Column("FECHA_REGISTRO")]
        public DateTime? FechaRegistro { get; set; }

        // Relación muchos a muchos con Perfil
        public ICollection<Perfil> Perfiles { get; set; }
    }
}
