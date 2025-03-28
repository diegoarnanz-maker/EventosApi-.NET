using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventosApi.Services
{
    public abstract class GenericoCRUDServiceImplSql<T, ID> : IGenericoService<T, ID> where T : class
    {
        private readonly DbContext _context;
        private readonly ILogger _logger;

        protected GenericoCRUDServiceImplSql(DbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método que deben implementar las subclases para devolver el DbSet concreto (como tu getRepository() en Spring)
        protected abstract DbSet<T> GetDbSet();

        // Virtual es como @Override en Java, permite que las subclases lo sobreescriban
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await GetDbSet().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todos los registros de {Entity}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<T?> GetByIdAsync(ID id)
        {
            try
            {
                return await GetDbSet().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar entidad {Entity} con ID {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await GetDbSet().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear entidad {Entity}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar entidad {Entity}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<bool> DeleteAsync(ID id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null) return false;

                GetDbSet().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar entidad {Entity} con ID {Id}", typeof(T).Name, id);
                throw;
            }
        }
    }
}
