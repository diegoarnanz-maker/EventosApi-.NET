using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Dtos;
using EventosApi.Models;

namespace EventosApi.Services
{
    public interface IReservaService : IGenericoService<Reserva, int>
    {
        Task<Reserva> CreateReservaAsync(Reserva reserva);
        Task<Reserva> UpdateReservaAsync(int idReserva, string username, ReservaUpdateDto dto);
        Task<bool> DeleteReservaAsync(int idReserva, string username, bool isAdmin);
        Task<IEnumerable<Reserva>> FindByUsernameAsync(string username);
        Task<IEnumerable<Reserva>> FindByEventoIdAsync(int idEvento);
    }
}