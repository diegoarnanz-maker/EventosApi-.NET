using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Models;

namespace EventosApi.Repositories
{
    public interface IEventoRepository
    {

        Task<IEnumerable<Evento>> GetAllAsync();
        Task<Evento?> GetByIdAsync(int id);
        Task<Evento> CreateAsync(Evento evento);
        Task<Evento> UpdateAsync(Evento evento);
        Task<bool> DeleteAsync(int id);

        // Métodos distintos a los CRUD
        Task<IEnumerable<Evento>> FindByNombreContainsAsync(string nombre);
        Task<IEnumerable<Evento>> FindByTipoIdAsync(int tipoId);
        Task<IEnumerable<Evento>> FindByFechaInicioAsync(DateTime fechaInicio);
        Task<IEnumerable<Evento>> FindByFechaFinAsync(DateTime fechaFin);
        Task<IEnumerable<Evento>> FindByFechaInicioAndFechaFinAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}