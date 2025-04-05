using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Configurations;
using EventosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventosApi.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly AppDbContext _context;

        public ReservaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Implementación de métodos de IReservaRepository

        public async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Evento)
                .ToListAsync();
        }

        public async Task<Reserva?> GetByIdAsync(int id)
        {
            return await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Evento)
                .FirstOrDefaultAsync(r => r.IdReserva == id);
        }

        public async Task<Reserva> CreateAsync(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }

        public async Task<Reserva> UpdateAsync(Reserva reserva)
        {
            _context.Entry(reserva).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return reserva;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return false;

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Reserva>> FindByUsernameAsync(string username)
        {
            return await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Evento)
                .Where(r => r.Username == username)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> FindByEventoIdAsync(int idEvento)
        {
            return await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Evento)
                .Where(r => r.IdEvento == idEvento)
                .ToListAsync();
        }

        public async Task<Reserva?> FindByEventoIdAndUsername(int idEvento, string username)
        {
            return await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Evento)
                .FirstOrDefaultAsync(r => r.IdEvento == idEvento && r.Username == username);
        }
    }
}