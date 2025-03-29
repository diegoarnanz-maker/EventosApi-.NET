using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Dtos
{
    public class EventoResponseDto
    {
        public int IdEvento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime? FechaInicio { get; set; }
        public int? Duracion { get; set; }
        public decimal? Precio { get; set; }
        public string NombreTipo { get; set; } = string.Empty;
    }

}