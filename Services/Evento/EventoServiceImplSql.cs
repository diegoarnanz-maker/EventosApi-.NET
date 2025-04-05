using EventosApi.Dtos;
using EventosApi.Exceptions;
using EventosApi.Models;
using EventosApi.Repositories;
using EventosApi.Services.Base;

namespace EventosApi.Services
{
    public class EventoServiceImplSql : IGenericDtoService<Evento, EventoRequestDto, EventoDetalleResponseDto, int>, IEventoService
    {
        private readonly IEventoRepository _eventoRepository;

        public EventoServiceImplSql(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        public async Task<EventoDetalleResponseDto> CreateAsync(EventoRequestDto dto)
        {
            Evento nuevoEvento = (Evento)dto;
            Evento guardado = await _eventoRepository.CreateAsync(nuevoEvento);

            // Recargar con la propiedad de navegación Tipo incluida
            Evento? eventoConTipo = await _eventoRepository.GetByIdWithTipoAsync(guardado.IdEvento);

            if (eventoConTipo == null)
                throw new NotFoundException("No se pudo recuperar el evento recién creado.");

            return (EventoDetalleResponseDto)eventoConTipo;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            Evento? evento = await _eventoRepository.GetByIdAsync(id);
            if (evento == null)
                throw new NotFoundException($"Evento con ID {id} no encontrado.");

            return await _eventoRepository.DeleteAsync(id);
        }

        public async Task<string> DeleteOrCancelAsync(int id)
        {
            Evento? evento = await _eventoRepository.GetByIdWithReservasAsync(id);
            if (evento == null)
                throw new NotFoundException($"Evento con ID {id} no encontrado.");

            if (evento.Reservas != null && evento.Reservas.Any())
            {
                evento.Estado = EstadoEvento.CANCELADO;
                await _eventoRepository.UpdateAsync(evento);
                return "El evento tiene reservas. Se ha cambiado su estado a CANCELADO.";
            }

            await _eventoRepository.DeleteAsync(id);
            return "Evento eliminado correctamente.";
        }

        public async Task<IEnumerable<EventoDetalleResponseDto>> GetAllDtosAsync()
        {
            var eventos = await _eventoRepository.GetAllWithTipoAsync();
            return eventos.Select(e => (EventoDetalleResponseDto)e);
        }

        public async Task<EventoDetalleResponseDto> GetDtoByIdAsync(int id)
        {
            Evento? evento = await _eventoRepository.GetByIdWithTipoAsync(id);
            if (evento == null)
                throw new NotFoundException($"Evento con ID {id} no encontrado.");

            return (EventoDetalleResponseDto)evento;
        }

        public async Task<EventoDetalleResponseDto> UpdateAsync(int id, EventoRequestDto dto)
        {
            Evento? evento = await _eventoRepository.GetByIdWithTipoAsync(id);
            if (evento == null)
                throw new NotFoundException($"Evento con ID {id} no encontrado.");

            dto.UpdateEntity(evento);
            Evento actualizado = await _eventoRepository.UpdateAsync(evento);
            return (EventoDetalleResponseDto)actualizado;
        }

        public async Task<IEnumerable<EventoDetalleResponseDto>> FindByNombreContainsAsync(string nombre)
        {
            var eventos = await _eventoRepository.FindByNombreContainsAsync(nombre);
            return eventos.Select(e => (EventoDetalleResponseDto)e);
        }

        public async Task<IEnumerable<EventoDetalleResponseDto>> FindByTipoIdAsync(int tipoId)
        {
            var eventos = await _eventoRepository.FindByTipoIdAsync(tipoId);
            return eventos.Select(e => (EventoDetalleResponseDto)e);
        }

        public async Task<IEnumerable<EventoDetalleResponseDto>> FindByFechaInicioAndFechaFinAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var eventos = await _eventoRepository.FindByFechaInicioAndFechaFinAsync(fechaInicio, fechaFin);
            return eventos.Select(e => (EventoDetalleResponseDto)e);
        }

    }
}
