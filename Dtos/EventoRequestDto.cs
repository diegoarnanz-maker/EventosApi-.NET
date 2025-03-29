using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EventosApi.Models;

namespace EventosApi.Dtos
{
    public class EventoRequestDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public int? Duracion { get; set; }
        public string? Direccion { get; set; }

        // Por el tema de ser enun, se debe usar para convertir a string
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Destacado? Destacado { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EstadoEvento? Estado { get; set; }  // Nullable

        public int? AforoMaximo { get; set; }
        public int? MinimoAsistencia { get; set; }
        public decimal? Precio { get; set; }
        public int IdTipo { get; set; }
    }

}