using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Models;

namespace EventosApi.Services
{
    public interface IEventoService : IGenericoService<Evento, int>
    {
        Task<IEnumerable<Evento>> FindByNombreContainsAsync(string nombre);
        Task<IEnumerable<Evento>> FindByTipoIdAsync(int tipoId);
        Task<IEnumerable<Evento>> FindByFechaInicioAsync(DateTime fechaInicio);
        Task<IEnumerable<Evento>> FindByFechaFinAsync(DateTime fechaFin);
        Task<IEnumerable<Evento>> FindByFechaInicioAndFechaFinAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}