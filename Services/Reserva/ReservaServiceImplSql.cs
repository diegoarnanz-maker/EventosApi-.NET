using EventosApi.Dtos;
using EventosApi.Exceptions;
using EventosApi.Models;
using EventosApi.Repositories;
using EventosApi.Services.Base;

namespace EventosApi.Services
{
    public class ReservaServiceImplSql : IGenericDtoService<Reserva, ReservaRequestDto, ReservaResponseDto, int>, IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IEventoRepository _eventoRepository;

        public ReservaServiceImplSql(IReservaRepository reservaRepository, IEventoRepository eventoRepository)
        {
            _reservaRepository = reservaRepository;
            _eventoRepository = eventoRepository;
        }

        public async Task<ReservaResponseDto> CreateAsync(ReservaRequestDto dto)
        {
            Reserva reserva = (Reserva)dto;

            Evento? evento = await _eventoRepository.GetByIdAsync(reserva.IdEvento)
                              ?? throw new NotFoundException("El evento no existe.");

            reserva.PrecioVenta = evento.Precio.HasValue ? evento.Precio.Value * reserva.Cantidad : 0;

            Reserva creada = await _reservaRepository.CreateAsync(reserva);
            return (ReservaResponseDto)creada;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Reserva? reserva = await _reservaRepository.GetByIdAsync(id)
                              ?? throw new NotFoundException($"Reserva con ID {id} no encontrada.");

            return await _reservaRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ReservaResponseDto>> GetAllDtosAsync()
        {
            var reservas = await _reservaRepository.GetAllAsync();
            return reservas.Select(r => (ReservaResponseDto)r);
        }

        public async Task<ReservaResponseDto> GetDtoByIdAsync(int id)
        {
            var reserva = await _reservaRepository.GetByIdAsync(id)
                          ?? throw new NotFoundException($"Reserva con ID {id} no encontrada.");
            return (ReservaResponseDto)reserva;
        }

        public async Task<ReservaResponseDto> UpdateAsync(int id, ReservaRequestDto dto)
        {
            var reserva = await _reservaRepository.GetByIdAsync(id)
                          ?? throw new NotFoundException($"Reserva con ID {id} no encontrada.");

            reserva.Cantidad = dto.Cantidad;
            reserva.Observaciones = dto.Observaciones;

            var evento = await _eventoRepository.GetByIdAsync(reserva.IdEvento);
            reserva.PrecioVenta = evento?.Precio.HasValue == true ? evento.Precio.Value * dto.Cantidad : 0;

            var actualizada = await _reservaRepository.UpdateAsync(reserva);
            return (ReservaResponseDto)actualizada;
        }

        public async Task<bool> DeleteAsync(int idReserva, string username, bool isAdmin)
        {
            var reserva = await _reservaRepository.GetByIdAsync(idReserva)
                          ?? throw new NotFoundException("Reserva no encontrada.");

            if (!isAdmin && reserva.Username != username)
                throw new ForbiddenException("No tienes permiso para eliminar esta reserva.");

            return await _reservaRepository.DeleteAsync(idReserva);
        }

        public async Task<IEnumerable<ReservaResponseDto>> FindByEventoIdAsync(int idEvento)
        {
            var reservas = await _reservaRepository.FindByEventoIdAsync(idEvento);
            return reservas.Select(r => (ReservaResponseDto)r);
        }

        public async Task<IEnumerable<ReservaResponseDto>> FindByUsernameAsync(string username)
        {
            var reservas = await _reservaRepository.FindByUsernameAsync(username);
            return reservas.Select(r => (ReservaResponseDto)r);
        }

        public async Task<ReservaResponseDto> UpdateAsync(int idReserva, string username, ReservaUpdateDto dto)
        {
            var reserva = await _reservaRepository.GetByIdAsync(idReserva)
                          ?? throw new NotFoundException("No se encontró la reserva.");

            if (reserva.Username != username)
                throw new ForbiddenException("No tienes permisos para modificar esta reserva.");

            Evento? evento = await _eventoRepository.GetByIdAsync(reserva.IdEvento)
                             ?? throw new NotFoundException("El evento ya no existe.");

            // Validaciones
            if (evento.Estado != EstadoEvento.ACEPTADO ||
                (evento.FechaInicio.HasValue && evento.FechaInicio < DateTime.Now))
                throw new BadRequestException("No se puede modificar una reserva de un evento cancelado, finalizado o pasado.");

            int totalReservado = (await _reservaRepository.FindByEventoIdAsync(reserva.IdEvento))
                                 .Where(r => r.IdReserva != idReserva)
                                 .Sum(r => r.Cantidad);

            if (evento.AforoMaximo.HasValue && totalReservado + dto.Cantidad > evento.AforoMaximo.Value)
                throw new BadRequestException("No hay suficiente aforo disponible para actualizar la reserva.");

            // Actualizar campos
            reserva.Cantidad = dto.Cantidad;
            reserva.Observaciones = dto.Observaciones;
            reserva.PrecioVenta = evento.Precio.HasValue ? evento.Precio.Value * dto.Cantidad : 0;

            var actualizada = await _reservaRepository.UpdateAsync(reserva);
            return (ReservaResponseDto)actualizada;
        }

        public async Task<ReservaResponseDto> CreateAsync(ReservaRequestDto dto, string username)
        {
            Reserva reserva = (Reserva)dto;
            reserva.Username = username;

            Evento? evento = await _eventoRepository.GetByIdAsync(reserva.IdEvento)
                              ?? throw new NotFoundException("El evento no existe.");

            // Validaciones
            if (evento.Estado != EstadoEvento.ACEPTADO)
                throw new BadRequestException("El evento no está disponible para reservas.");

            int totalReservado = (await _reservaRepository.FindByEventoIdAsync(reserva.IdEvento))
                                 .Sum(r => r.Cantidad);

            if (evento.AforoMaximo.HasValue && totalReservado + reserva.Cantidad > evento.AforoMaximo.Value)
                throw new BadRequestException("No hay suficiente aforo disponible.");

            // Calcular precio total
            reserva.PrecioVenta = evento.Precio.HasValue ? evento.Precio.Value * reserva.Cantidad : 0;

            // Comprobar duplicado
            var existente = await _reservaRepository.FindByEventoIdAndUsername(reserva.IdEvento, username);
            if (existente != null)
                throw new BadRequestException("Ya existe una reserva para este evento con tu usuario.");

            var creada = await _reservaRepository.CreateAsync(reserva);
            return (ReservaResponseDto)creada;
        }

    }

}
