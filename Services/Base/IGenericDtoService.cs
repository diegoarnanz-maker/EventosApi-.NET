using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Services.Base
{
    public interface IGenericDtoService<TEntity, TRequestDto, TResponseDto, ID>
    {
        Task<IEnumerable<TResponseDto>> GetAllDtosAsync();
        Task<TResponseDto> GetDtoByIdAsync(ID id);
        Task<TResponseDto> CreateAsync(TRequestDto dto);
        Task<TResponseDto> UpdateAsync(ID id, TRequestDto dto);
        Task<bool> DeleteAsync(ID id);
    }

}