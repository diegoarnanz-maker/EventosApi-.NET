using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventosApi.Services
{
    public interface IGenericoService<T, ID> where T : class
    {
        // Devuelve una colección genérica que se puede recorrer, sin imponer una estructura específica como List<T>. Buenas practicas.
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(ID id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(ID id);
    }
}

