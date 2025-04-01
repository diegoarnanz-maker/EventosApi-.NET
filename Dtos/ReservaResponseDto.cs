using System;

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
    }
}
