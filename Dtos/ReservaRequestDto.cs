using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Dtos
{
    public class ReservaRequestDto
    {
        [Required]
        public int IdEvento { get; set; }

        [Range(1, 10)]
        public int Cantidad { get; set; }

        [MaxLength(200)]
        public string? Observaciones { get; set; }
    }
}