using System;
using System.Text.Json.Serialization;
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

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Destacado? Destacado { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EstadoEvento? Estado { get; set; }

        public int? AforoMaximo { get; set; }
        public int? MinimoAsistencia { get; set; }
        public decimal? Precio { get; set; }
        public int IdTipo { get; set; }

        public static explicit operator Evento(EventoRequestDto dto)
        {
            return new Evento
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                FechaInicio = dto.FechaInicio,
                Duracion = dto.Duracion,
                Direccion = dto.Direccion,
                Estado = dto.Estado ?? EstadoEvento.ACEPTADO,
                Destacado = dto.Destacado,
                AforoMaximo = dto.AforoMaximo,
                MinimoAsistencia = dto.MinimoAsistencia,
                Precio = dto.Precio,
                IdTipo = dto.IdTipo
            };
        }

        public void UpdateEntity(Evento evento)
        {
            evento.Nombre = Nombre;
            evento.Descripcion = Descripcion;
            evento.FechaInicio = FechaInicio;
            evento.Duracion = Duracion;
            evento.Direccion = Direccion;
            evento.Estado = Estado ?? evento.Estado;
            evento.Destacado = Destacado ?? evento.Destacado;
            evento.AforoMaximo = AforoMaximo;
            evento.MinimoAsistencia = MinimoAsistencia;
            evento.Precio = Precio;
            evento.IdTipo = IdTipo;
        }

    }
}
