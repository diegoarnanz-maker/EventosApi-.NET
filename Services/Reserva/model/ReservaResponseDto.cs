using System;
using EventosApi.Models;

namespace EventosApi.Dtos
{
    public class ReservaResponseDto
    {
        public int IdReserva { get; set; }

        public int IdEvento { get; set; }
        public string NombreEvento { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;

        public decimal? PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public string? Observaciones { get; set; }

        public DateTime? FechaEvento { get; set; }
        public string? DireccionEvento { get; set; }

        public static explicit operator ReservaResponseDto(Reserva reserva)
        {
            return new ReservaResponseDto
            {
                IdReserva = reserva.IdReserva,
                IdEvento = reserva.IdEvento,
                NombreEvento = reserva.Evento?.Nombre ?? string.Empty,
                Username = reserva.Usuario?.Username ?? string.Empty,
                EmailUsuario = reserva.Usuario?.Email ?? string.Empty,
                PrecioVenta = reserva.PrecioVenta,
                Cantidad = reserva.Cantidad,
                Observaciones = reserva.Observaciones,
                FechaEvento = reserva.Evento?.FechaInicio,
                DireccionEvento = reserva.Evento?.Direccion
            };
        }
    }
}
