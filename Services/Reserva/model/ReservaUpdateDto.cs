using EventosApi.Models;

namespace EventosApi.Dtos
{
    public class ReservaUpdateDto
    {
        public int Cantidad { get; set; }
        public string? Observaciones { get; set; }

        public void UpdateEntity(Reserva reserva)
        {
            reserva.Cantidad = Cantidad;
            reserva.Observaciones = Observaciones;
        }
    }
}
