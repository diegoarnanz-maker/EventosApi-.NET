using System;
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
        public int IdTipo { get; set; }
        public string NombreTipo { get; set; } = string.Empty;

        public static explicit operator EventoDetalleResponseDto(Evento e)
        {
            return new EventoDetalleResponseDto
            {
                IdEvento = e.IdEvento,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                FechaInicio = e.FechaInicio,
                Duracion = e.Duracion,
                Direccion = e.Direccion,
                Estado = e.Estado.ToString(),
                Destacado = e.Destacado.ToString(),
                AforoMaximo = e.AforoMaximo,
                MinimoAsistencia = e.MinimoAsistencia,
                Precio = e.Precio,
                IdTipo = e.IdTipo,
                NombreTipo = e.Tipo?.Nombre ?? string.Empty
            };
        }
    }
}
