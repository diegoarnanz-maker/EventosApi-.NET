using System;
using EventosApi.Models;

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

        public static explicit operator EventoResponseDto(Evento e)
        {
            return new EventoResponseDto
            {
                IdEvento = e.IdEvento,
                Nombre = e.Nombre,
                FechaInicio = e.FechaInicio,
                Duracion = e.Duracion,
                Precio = e.Precio,
                NombreTipo = e.Tipo?.Nombre ?? string.Empty
            };
        }
    }
}
