using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Models;

namespace EventosApi.Dtos
{
    public class EventoDetalleResponseDto
    {
        public int IdEvento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public int? Duracion { get; set; }
        public string? Direccion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Destacado { get; set; } = string.Empty;

        public int? AforoMaximo { get; set; }
        public int? MinimoAsistencia { get; set; }
        public decimal? Precio { get; set; }

        // Detalles del tipo
        public int IdTipo { get; set; }
        public string NombreTipo { get; set; } = string.Empty;

        // Si en algún momento queremos incluir la lista de reservas:
        // public List<ReservaSimpleDto> Reservas { get; set; }
    }

}