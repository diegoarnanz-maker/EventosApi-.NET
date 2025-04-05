using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EventosApi.Services.Base
{
    public abstract class GenericDtoService<TEntity, TRequestDto, TResponseDto, ID>
        : IGenericDtoService<TEntity, TRequestDto, TResponseDto, ID>
        where TEntity : class
    {
        private readonly DbContext _context;

        protected GenericDtoService(DbContext context)
        {
            _context = context;
        }

        protected abstract DbSet<TEntity> GetDbSet();
        protected abstract TEntity FromDto(TRequestDto dto);
        protected abstract void UpdateEntityFromDto(TRequestDto dto, TEntity entity);
        protected abstract TResponseDto ToDto(TEntity entity);

        public async Task<IEnumerable<TResponseDto>> GetAllDtosAsync()
        {
            var entities = await GetDbSet().ToListAsync();
            return entities.Select(ToDto);
        }

        public async Task<TResponseDto> GetDtoByIdAsync(ID id)
        {
            var entity = await GetDbSet().FindAsync(id);
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} con ID {id} no encontrado.");

            return ToDto(entity);
        }

        public async Task<TResponseDto> CreateAsync(TRequestDto dto)
        {
            var entity = FromDto(dto);
            await GetDbSet().AddAsync(entity);
            await _context.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<TResponseDto> UpdateAsync(ID id, TRequestDto dto)
        {
            var entity = await GetDbSet().FindAsync(id);
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} con ID {id} no encontrado.");

            UpdateEntityFromDto(dto, entity);
            await _context.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> DeleteAsync(ID id)
        {
            var entity = await GetDbSet().FindAsync(id);
            if (entity == null)
                return false;

            GetDbSet().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}