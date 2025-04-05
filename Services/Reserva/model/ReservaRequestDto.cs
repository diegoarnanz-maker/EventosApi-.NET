using System.ComponentModel.DataAnnotations;
using EventosApi.Models;

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

        public static explicit operator Reserva(ReservaRequestDto dto)
        {
            return new Reserva
            {
                IdEvento = dto.IdEvento,
                Cantidad = dto.Cantidad,
                Observaciones = dto.Observaciones,
                // El Username se asigna desde el contexto (usuario autenticado), no en este DTO
            };
        }
    }
}
