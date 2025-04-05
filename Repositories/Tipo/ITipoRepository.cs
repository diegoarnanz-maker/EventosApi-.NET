using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Models;

namespace EventosApi.Repositories
{
    public interface ITipoRepository
    {
        Task<IEnumerable<Tipo>> GetAllAsync();
        Task<Tipo?> GetByIdAsync(int id);
        Task<Tipo> CreateAsync(Tipo tipo);
        Task<Tipo> UpdateAsync(Tipo tipo);
        Task<bool> DeleteAsync(int id);

        // Metodos distintos a los CRUD
        Task<IEnumerable<Tipo>> FindByNombreContainsAsync(string nombre);

    }
}