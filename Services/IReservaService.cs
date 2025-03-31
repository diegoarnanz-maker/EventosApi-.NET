using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Models;

namespace EventosApi.Services
{
    public interface IReservaService : IGenericoService<Reserva, int>
    {
        Task<Reserva> CreateReservaAsync(Reserva reserva);
        Task<IEnumerable<Reserva>> FindByUsernameAsync(string username);
        Task<IEnumerable<Reserva>> FindByEventoIdAsync(int idEvento);
    }
}