using System.Collections.Generic;
using System.Threading.Tasks;
using EventosApi.Dtos;
using EventosApi.Models;
using EventosApi.Services.Base;

namespace EventosApi.Services
{
    public interface ITipoService : IGenericDtoService<Tipo, TipoRequestDto, TipoResponseDto, int>
    {
        Task<IEnumerable<TipoResponseDto>> FindByNombreContainsAsync(string nombre);
    }
}
