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
    public class EventoServiceImplSql : GenericoCRUDServiceImplSql<Evento, int>, IEventoService
    {
        private readonly AppDbContext _context;
        private readonly IEventoRepository _eventoRepository;

        public EventoServiceImplSql(AppDbContext context, IEventoRepository eventoRepository, ILogger<EventoServiceImplSql> logger)
            : base(context, logger)
        {
            _context = context;
            _eventoRepository = eventoRepository;
        }

        // Implementación de método de IEventoService

        //Sobreescribimos el método GetAllAsync para incluir la propiedad de navegación Tipo
        public override async Task<IEnumerable<Evento>> GetAllAsync()
        {
            return await _context.Eventos
                .Include(e => e.Tipo)
                .ToListAsync();
        }

        public override async Task<Evento?> GetByIdAsync(int id)
        {
            return await _context.Eventos
                .Include(e => e.Tipo)
                .FirstOrDefaultAsync(e => e.IdEvento == id);
        }


        public async Task<IEnumerable<Evento>> FindByFechaFinAsync(DateTime fechaFin)
        {
            return await _eventoRepository.FindByFechaFinAsync(fechaFin);
        }

        public async Task<IEnumerable<Evento>> FindByFechaInicioAndFechaFinAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _eventoRepository.FindByFechaInicioAndFechaFinAsync(fechaInicio, fechaFin);
        }

        public async Task<IEnumerable<Evento>> FindByFechaInicioAsync(DateTime fechaInicio)
        {
            return await _eventoRepository.FindByFechaInicioAsync(fechaInicio);
        }

        public async Task<IEnumerable<Evento>> FindByNombreContainsAsync(string nombre)
        {
            return await _eventoRepository.FindByNombreContainsAsync(nombre);
        }

        public async Task<IEnumerable<Evento>> FindByTipoIdAsync(int tipoId)
        {
            return await _eventoRepository.FindByTipoIdAsync(tipoId);
        }

        protected override DbSet<Evento> GetDbSet()
        {
            return _context.Eventos;
        }

    }
}