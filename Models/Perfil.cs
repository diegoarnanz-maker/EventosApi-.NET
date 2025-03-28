using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Models
{
    public class Perfil
    {
        [Key]
        public int IdPerfil { get; set; }

        [Required, MaxLength(45)]
        public string Nombre { get; set; }

        // Relación inversa con Usuario
        public ICollection<Usuario> Usuarios { get; set; }
    }
}