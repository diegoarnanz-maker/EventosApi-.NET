using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Models
{
    [Table("RESERVAS")]
    public class Reserva
    {
        [Key]
        [Column("ID_RESERVA")]
        public int IdReserva { get; set; }

        [Required]
        [Column("ID_EVENTO")]
        public int IdEvento { get; set; }

        [Required, MaxLength(45)]
        [Column("USERNAME")]
        public string Username { get; set; }

        [Column("PRECIO_VENTA", TypeName = "decimal(9,2)")]
        public decimal? PrecioVenta { get; set; }

        [MaxLength(200)]
        [Column("OBSERVACIONES")]
        public string? Observaciones { get; set; }

        [Range(1, 10)]
        [Column("CANTIDAD")]
        public int Cantidad { get; set; }

        // Relaciones de navegación
        [ForeignKey("Username")]
        public Usuario Usuario { get; set; }

        [ForeignKey("IdEvento")]
        public Evento Evento { get; set; }
    }
}
