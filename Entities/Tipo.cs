using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Models
{
    [Table("TIPOS")]
    public class Tipo
    {
        [Key]
        [Column("ID_TIPO")]
        public int IdTipo { get; set; }

        [Required, MaxLength(45)]
        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [MaxLength(200)]
        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }

        // Relación: un tipo tiene muchos eventos
        public ICollection<Evento> Eventos { get; set; }
    }
}
