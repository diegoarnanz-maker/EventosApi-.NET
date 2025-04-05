using EventosApi.Models;

namespace EventosApi.Repositories
{
    public interface IReservaRepository
    {
        Task<IEnumerable<Reserva>> GetAllAsync();
        Task<Reserva?> GetByIdAsync(int id);
        Task<Reserva> CreateAsync(Reserva reserva);
        Task<Reserva> UpdateAsync(Reserva reserva);
        Task<bool> DeleteAsync(int id);

        // Métodos personalizados
        Task<IEnumerable<Reserva>> FindByUsernameAsync(string username);
        Task<IEnumerable<Reserva>> FindByEventoIdAsync(int idEvento);
        Task<Reserva?> FindByEventoIdAndUsername(int idEvento, string username);
    }
}
