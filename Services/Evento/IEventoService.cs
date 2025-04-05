using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventosApi.Dtos;
using EventosApi.Models;
using EventosApi.Services.Base;

namespace EventosApi.Services
{
    public interface IEventoService : IGenericDtoService<Evento, EventoRequestDto, EventoDetalleResponseDto, int>
    {
        Task<IEnumerable<EventoDetalleResponseDto>> FindByNombreContainsAsync(string nombre);
        Task<IEnumerable<EventoDetalleResponseDto>> FindByTipoIdAsync(int tipoId);
        Task<IEnumerable<EventoDetalleResponseDto>> FindByFechaInicioAndFechaFinAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<string> DeleteOrCancelAsync(int id);
    }
}
