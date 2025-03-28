using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Models
{
    public class Reserva
    {
        [Key]
        public int IdReserva { get; set; }

        [Required]
        public int IdEvento { get; set; }

        [Required, MaxLength(45)]
        public string Username { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal? PrecioVenta { get; set; }

        [MaxLength(200)]
        public string? Observaciones { get; set; }

        [Range(1, 10)]
        public int Cantidad { get; set; }

        // Relaciones de navegación, las infiere automaticamente si los nombres coinciden
        // con los de las propiedades de las clases relacionadas
        public Usuario Usuario { get; set; }
        public Evento Evento { get; set; }
    }
}