using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Models;

namespace EventosApi.Services
{
    public interface ITipoService : IGenericoService<Tipo, int>
    {
        Task<IEnumerable<Tipo>> FindByNombreContainsAsync(string nombre);

    }
}