using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Configurations;
using EventosApi.Models;
using EventosApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventosApi.Services
{
    public class ReservaServiceImplSql : GenericoCRUDServiceImplSql<Reserva, int>, IReservaService
    {
        private readonly AppDbContext _context;
        private readonly IReservaRepository _reservaRepository;
        private readonly IEventoRepository _eventoRepository = null!;

        public ReservaServiceImplSql(AppDbContext context, IReservaRepository reservaRepository, ILogger<ReservaServiceImplSql> logger, IEventoRepository eventoRepository)
            : base(context, logger)
        {
            _context = context;
            _reservaRepository = reservaRepository;
            _eventoRepository = eventoRepository;
        }

        // Único método requerido por la clase abstracta
        protected override DbSet<Reserva> GetDbSet()
        {
            return _context.Reservas;
        }

        // Implementación de métodos de IReservaService
        public Task<IEnumerable<Reserva>> FindByEventoIdAsync(int idEvento)
        {
            return _reservaRepository.FindByEventoIdAsync(idEvento);
        }

        public Task<IEnumerable<Reserva>> FindByUsernameAsync(string username)
        {
            return _reservaRepository.FindByUsernameAsync(username);
        }

        public async Task<Reserva> CreateReservaAsync(Reserva reserva)
        {
            if (reserva.Cantidad > 10)
                throw new Exception("No se pueden hacer más de 10 reservas por evento/usuario.");

            // Validamos que el evento existe
            Evento? evento = await _eventoRepository.GetByIdAsync(reserva.IdEvento);
            if (evento == null)
                throw new Exception("El evento no existe.");

            // Validamos que el evento no esté cerrado
            if (evento.Estado != EstadoEvento.ACEPTADO)
                throw new InvalidOperationException("El evento ya ha finalizado o ha sido cancelado.");

            // Validamos aforo
            int totalReservado = await _context.Reservas
                .Where(r => r.IdEvento == reserva.IdEvento)
                .SumAsync(r => r.Cantidad);

            if (evento.AforoMaximo.HasValue && totalReservado + reserva.Cantidad > evento.AforoMaximo.Value)
                throw new InvalidOperationException("No hay suficiente aforo disponible para completar esta reserva.");

            // Validamos duplicado
            Reserva? reservaExistente = await _reservaRepository.FindByEventoIdAndUsername(reserva.IdEvento, reserva.Username);
            if (reservaExistente != null)
            {
                if (reservaExistente.Cantidad >= 10)
                    throw new InvalidOperationException("Ya tienes una reserva con 10 personas para este evento.");

                throw new InvalidOperationException("Ya tienes una reserva para este evento.");
            }

            // Precio total
            reserva.PrecioVenta = evento.Precio.HasValue
                ? evento.Precio.Value * reserva.Cantidad
                : 0;

            // Guardamos correctamente con await
            Reserva nuevaReserva = await _reservaRepository.CreateAsync(reserva);

            return nuevaReserva;
        }

    }
}