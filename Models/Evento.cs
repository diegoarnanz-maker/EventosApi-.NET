using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventosApi.Models
{
    [Table("EVENTOS")]
    public class Evento
    {
        [Key]
        [Column("ID_EVENTO")]
        public int IdEvento { get; set; }

        [Required, MaxLength(50)]
        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [MaxLength(200)]
        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }

        [Column("FECHA_INICIO")]
        public DateTime? FechaInicio { get; set; }

        [Column("DURACION")]
        public int? Duracion { get; set; }

        [MaxLength(100)]
        [Column("DIRECCION")]
        public string? Direccion { get; set; }

        [Column("ESTADO")]
        public EstadoEvento? Estado { get; set; }

        [Column("DESTACADO")]
        public Destacado? Destacado { get; set; }

        [Column("AFORO_MAXIMO")]
        public int? AforoMaximo { get; set; }

        [Column("MINIMO_ASISTENCIA")]
        public int? MinimoAsistencia { get; set; }

        [Column("PRECIO", TypeName = "decimal(9,2)")]
        public decimal? Precio { get; set; }

        [Column("ID_TIPO")]
        public int IdTipo { get; set; }

        [ForeignKey("IdTipo")]
        public virtual Tipo Tipo { get; set; }
        
        // Relación uno a muchos: un evento puede tener muchas reservas
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
