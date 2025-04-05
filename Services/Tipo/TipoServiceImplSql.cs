using EventosApi.Dtos;
using EventosApi.Exceptions;
using EventosApi.Models;
using EventosApi.Repositories;
using EventosApi.Services.Base;

namespace EventosApi.Services
{
    public class TipoServiceImplSql : IGenericDtoService<Tipo, TipoRequestDto, TipoResponseDto, int>, ITipoService
    {
        private readonly ITipoRepository _tipoRepository;

        public TipoServiceImplSql(ITipoRepository tipoRepository)
        {
            _tipoRepository = tipoRepository;
        }

        public async Task<IEnumerable<TipoResponseDto>> GetAllDtosAsync()
        {
            var tipos = await _tipoRepository.GetAllAsync();
            return tipos.Select(ToDto);
        }

        public async Task<TipoResponseDto> GetDtoByIdAsync(int id)
        {
            var tipo = await _tipoRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException($"Tipo con ID {id} no encontrado.");
            return ToDto(tipo);
        }

        public async Task<TipoResponseDto> CreateAsync(TipoRequestDto dto)
        {
            var entity = FromDto(dto);
            var saved = await _tipoRepository.CreateAsync(entity);
            return ToDto(saved);
        }

        public async Task<TipoResponseDto> UpdateAsync(int id, TipoRequestDto dto)
        {
            var tipo = await _tipoRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException($"Tipo con ID {id} no encontrado.");

            UpdateEntityFromDto(dto, tipo);
            var updated = await _tipoRepository.UpdateAsync(tipo);
            return ToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tipo = await _tipoRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException($"Tipo con ID {id} no encontrado.");

            return await _tipoRepository.DeleteAsync(tipo.IdTipo);
        }

        public async Task<IEnumerable<TipoResponseDto>> FindByNombreContainsAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new BadRequestException("El nombre no puede estar vacío.");

            var resultados = await _tipoRepository.FindByNombreContainsAsync(nombre);

            if (!resultados.Any())
                throw new NotFoundException("No se encontraron tipos que coincidan con el criterio de búsqueda.");

            return resultados.Select(ToDto);
        }

        // Métodos auxiliares de mapeo
        private Tipo FromDto(TipoRequestDto dto)
        {
            return new Tipo
            {
                Nombre = dto.Nombre
            };
        }

        private void UpdateEntityFromDto(TipoRequestDto dto, Tipo entity)
        {
            entity.Nombre = dto.Nombre;
        }

        private TipoResponseDto ToDto(Tipo entity)
        {
            return new TipoResponseDto
            {
                IdTipo = entity.IdTipo,
                Nombre = entity.Nombre,
                Descripcion = entity.Descripcion
            };
        }
    }
}
