using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventosApi.Models
{
    public class Evento
    {
        [Key]
        public int IdEvento { get; set; }

        [Required, MaxLength(50)]
        public string Nombre { get; set; }

        [MaxLength(200)]
        public string? Descripcion { get; set; }

        public DateTime? FechaInicio { get; set; }

        public int? Duracion { get; set; }

        [MaxLength(100)]
        public string? Direccion { get; set; }

        public EstadoEvento? Estado { get; set; }

        public Destacado? Destacado { get; set; }

        public int? AforoMaximo { get; set; }

        public int? MinimoAsistencia { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal? Precio { get; set; }

        public int IdTipo { get; set; }
        public Tipo Tipo { get; set; }

        public ICollection<Reserva> Reservas { get; set; }
    }

    public enum EstadoEvento
    {
        ACEPTADO,
        TERMINADO,
        CANCELADO
    }

    public enum Destacado
    {
        S,
        N
    }
}
