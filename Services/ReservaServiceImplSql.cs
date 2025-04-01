using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Configurations;
using EventosApi.Dtos;
using EventosApi.Exceptions;
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

        // Metodos del crud override para que conecte el repositorio que SI tiene .Include y no devuelva nulls en los campos
        public override async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return await _reservaRepository.GetAllAsync();
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
                throw new BadRequestException("No se pueden hacer más de 10 reservas por evento/usuario.");

            // Validamos que el evento existe
            Evento? evento = await _eventoRepository.GetByIdAsync(reserva.IdEvento);
            if (evento == null)
                throw new NotFoundException("El evento no existe.");

            // Validamos que el evento no esté cerrado
            if (evento.Estado != EstadoEvento.ACEPTADO)
                throw new BadRequestException("El evento ya ha finalizado o ha sido cancelado.");

            // Validamos aforo
            int totalReservado = await _context.Reservas
                .Where(r => r.IdEvento == reserva.IdEvento)
                .SumAsync(r => r.Cantidad);

            if (evento.AforoMaximo.HasValue && totalReservado + reserva.Cantidad > evento.AforoMaximo.Value)
                throw new BadRequestException("No hay suficiente aforo disponible para completar esta reserva.");

            // Validamos duplicado
            Reserva? reservaExistente = await _reservaRepository.FindByEventoIdAndUsername(reserva.IdEvento, reserva.Username);
            if (reservaExistente != null)
            {
                if (reservaExistente.Cantidad >= 10)
                    throw new BadRequestException("Ya tienes una reserva con 10 personas para este evento.");

                throw new BadRequestException("Ya tienes una reserva para este evento.");
            }

            // Precio total
            reserva.PrecioVenta = evento.Precio.HasValue
                ? evento.Precio.Value * reserva.Cantidad
                : 0;

            // Guardamos correctamente con await
            Reserva nuevaReserva = await _reservaRepository.CreateAsync(reserva);

            return nuevaReserva;
        }

        public async Task<Reserva> UpdateReservaAsync(int idReserva, string username, ReservaUpdateDto dto)
        {
            Reserva? reserva = await _reservaRepository.GetByIdAsync(idReserva);

            if (reserva == null)
                throw new NotFoundException("No se encontró la reserva.");

            if (reserva.Username != username)
                throw new ForbiddenException("No tienes permisos para modificar esta reserva.");

            Evento? evento = await _eventoRepository.GetByIdAsync(reserva.IdEvento);

            if (evento == null)
                throw new NotFoundException("El evento asociado no existe.");

            if (evento.Estado != EstadoEvento.ACEPTADO ||
                (evento.FechaInicio.HasValue && evento.FechaInicio.Value.Date < DateTime.Today))
                throw new BadRequestException("No se puede modificar una reserva de un evento cancelado, terminado o pasado.");

            if (dto.Cantidad > 10)
                throw new BadRequestException("La cantidad máxima por reserva es 10.");

            // Verificamos aforo total disponible (excluyendo la cantidad actual del usuario)
            int totalReservado = await _context.Reservas
                .Where(r => r.IdEvento == reserva.IdEvento && r.IdReserva != reserva.IdReserva)
                .SumAsync(r => r.Cantidad);

            if (evento.AforoMaximo.HasValue && totalReservado + dto.Cantidad > evento.AforoMaximo.Value)
                throw new BadRequestException("No hay suficiente aforo disponible para actualizar la reserva.");

            reserva.Cantidad = dto.Cantidad;
            reserva.Observaciones = dto.Observaciones;
            reserva.PrecioVenta = evento.Precio.HasValue ? evento.Precio.Value * dto.Cantidad : 0;

            await _reservaRepository.UpdateAsync(reserva);
            return reserva;
        }

        public async Task<bool> DeleteReservaAsync(int idReserva, string username, bool isAdmin)
        {
            Reserva? reserva = await _reservaRepository.GetByIdAsync(idReserva);
            if (reserva == null)
                throw new NotFoundException("No se encontró la reserva.");

            if (!isAdmin && reserva.Username != username)
                throw new ForbiddenException("No tienes permisos para eliminar esta reserva.");

            return await _reservaRepository.DeleteAsync(idReserva);
        }
    }
}
