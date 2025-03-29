using EventosApi.Configurations;
using EventosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventosApi.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly AppDbContext _context;

        public EventoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Evento>> GetAllAsync()
        {
            return await _context.Eventos
                .Include(e => e.Tipo)
                .ToListAsync();
        }

        public async Task<Evento?> GetByIdAsync(int id)
        {
            return await _context.Eventos
                .Include(e => e.Tipo)
                .FirstOrDefaultAsync(e => e.IdEvento == id);
        }

        public async Task<Evento> CreateAsync(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task<Evento> UpdateAsync(Evento evento)
        {
            _context.Entry(evento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return false;
            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return true;
        }

        // Métodos especiales

        // Devuelve todos los eventos cuyo nombre contiene una cadena parcial
        // List<Evento> findByNombreContaining(String nombre);
        public async Task<IEnumerable<Evento>> FindByNombreContainsAsync(string nombre)
        {
            return await _context.Eventos
                .Include(e => e.Tipo)
                .Where(e => e.Nombre.Contains(nombre))
                .ToListAsync();
        }

        // Devuelve todos los eventos que pertenecen a un tipo específico
        //List<Evento> findByTipoId(int tipoId);
        public async Task<IEnumerable<Evento>> FindByTipoIdAsync(int tipoId)
        {
            return await _context.Eventos
                .Include(e => e.Tipo)
                .Where(e => e.IdTipo == tipoId)
                .ToListAsync();
        }

        // Devuelve todos los eventos que comienzan exactamente en la fecha proporcionada
        // @Query("SELECT e FROM Evento e WHERE DATE(e.fechaInicio) = :fechaInicio")
        // List<Evento> findByFechaInicio(Date fechaInicio);
        public async Task<IEnumerable<Evento>> FindByFechaInicioAsync(DateTime fechaInicio)
        {
            return await _context.Eventos
                .Include(e => e.Tipo)
                .Where(e => e.FechaInicio.HasValue && e.FechaInicio.Value.Date == fechaInicio.Date)
                .ToListAsync();
        }

        // Devuelve todos los eventos cuya fecha de finalización calculada (inicio + duración) coincide con la proporcionada
        // @Query("""
        // SELECT e FROM Evento e 
        // WHERE e.fechaInicio IS NOT NULL 
        // AND e.duracion IS NOT NULL 
        // AND FUNCTION('DATE_ADD', e.fechaInicio, FUNCTION('INTERVAL', e.duracion, 'DAY')) = :fechaFin
        // """)
        // List<Evento> findByFechaFin(@Param("fechaFin") LocalDate fechaFin);
        public async Task<IEnumerable<Evento>> FindByFechaFinAsync(DateTime fechaFin)
        {
            return await _context.Eventos
                .Include(e => e.Tipo)
                .Where(e => e.FechaInicio.HasValue &&
                            e.Duracion.HasValue &&
                            e.FechaInicio.Value.AddDays(e.Duracion.Value).Date == fechaFin.Date)
                .ToListAsync();
        }

        // Devuelve todos los eventos cuya fecha de inicio esté entre dos fechas (inclusive)
        // List<Evento> findByFechaInicioBetween(LocalDate fechaInicio, LocalDate fechaFin);
        public async Task<IEnumerable<Evento>> FindByFechaInicioAndFechaFinAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Eventos
                .Include(e => e.Tipo)
                .Where(e => e.FechaInicio.HasValue &&
                            e.FechaInicio.Value.Date >= fechaInicio.Date &&
                            e.FechaInicio.Value.Date <= fechaFin.Date)
                .ToListAsync();
        }

    }
}
