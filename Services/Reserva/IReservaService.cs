using System.Collections.Generic;
using System.Threading.Tasks;
using EventosApi.Dtos;

namespace EventosApi.Services
{
    public interface IReservaService
    {
        Task<IEnumerable<ReservaResponseDto>> GetAllDtosAsync();
        Task<ReservaResponseDto> GetDtoByIdAsync(int id);
        Task<ReservaResponseDto> CreateAsync(ReservaRequestDto dto, string username);
        Task<ReservaResponseDto> UpdateAsync(int idReserva, string username, ReservaUpdateDto dto);
        Task<bool> DeleteAsync(int idReserva, string username, bool isAdmin);

        Task<IEnumerable<ReservaResponseDto>> FindByUsernameAsync(string username);
        Task<IEnumerable<ReservaResponseDto>> FindByEventoIdAsync(int idEvento);
    }
}
