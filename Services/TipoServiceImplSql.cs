using EventosApi.Configurations;
using EventosApi.Models;
using EventosApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventosApi.Services
{
    public class TipoServiceImplSql : GenericoCRUDServiceImplSql<Tipo, int>, ITipoService
    {
        private readonly AppDbContext _context;

        private readonly ITipoRepository _tipoRepository;

        public TipoServiceImplSql(AppDbContext context, ITipoRepository tipoRepository, ILogger<TipoServiceImplSql> logger)
            : base(context, logger)
        {
            _context = context;
            _tipoRepository = tipoRepository;
        }

        // Único método requerido por la clase abstracta
        protected override DbSet<Tipo> GetDbSet()
        {
            return _context.Tipos;
        }

        // Implementación de método de ITipoService
        public async Task<IEnumerable<Tipo>> FindByNombreContainsAsync(string nombre)
        {
            return await _tipoRepository.FindByNombreContainsAsync(nombre);
        }
    }
}
