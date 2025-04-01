using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventosApi.Dtos;
using EventosApi.Models;
using EventosApi.Services;
using EventosApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoController : ControllerBase
    {
        private readonly ITipoService _tipoService;
        private readonly IMapper _mapper;

        public TipoController(ITipoService tipoService, IMapper mapper)
        {
            _tipoService = tipoService;
            _mapper = mapper;
        }

        // RUTAS PÚBLICAS

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TipoResponseDto>>>> GetAll()
        {
            IEnumerable<Tipo> tipos = await _tipoService.GetAllAsync();
            IEnumerable<TipoResponseDto> tipoDtos = _mapper.Map<IEnumerable<TipoResponseDto>>(tipos);
            return Ok(new ApiResponse<IEnumerable<TipoResponseDto>>(tipoDtos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TipoResponseDto>>> GetById(int id)
        {
            Tipo tipo = await _tipoService.GetByIdAsync(id); // Si no lo encuentra, lanza NotFoundException desde el service
            TipoResponseDto dto = _mapper.Map<TipoResponseDto>(tipo);
            return Ok(new ApiResponse<TipoResponseDto>(dto));
        }

        [HttpGet("nombre/{nombre}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TipoResponseDto>>>> FindByNombreContainsAsync(string nombre)
        {
            IEnumerable<Tipo> tipos = await _tipoService.FindByNombreContainsAsync(nombre);
            IEnumerable<TipoResponseDto> tipoDtos = _mapper.Map<IEnumerable<TipoResponseDto>>(tipos);
            return Ok(new ApiResponse<IEnumerable<TipoResponseDto>>(tipoDtos));
        }

        // RUTAS PROTEGIDAS (SOLO ADMIN)

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<TipoResponseDto>>> Create(TipoRequestDto dto)
        {
            Tipo tipo = _mapper.Map<Tipo>(dto);
            Tipo newTipo = await _tipoService.CreateAsync(tipo);
            TipoResponseDto tipoResponseDto = _mapper.Map<TipoResponseDto>(newTipo);
            return Ok(new ApiResponse<TipoResponseDto>(tipoResponseDto));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<TipoResponseDto>>> Update(int id, TipoRequestDto dto)
        {
            Tipo tipo = await _tipoService.GetByIdAsync(id); // Lanza excepción si no lo encuentra

            _mapper.Map(dto, tipo);
            Tipo updatedTipo = await _tipoService.UpdateAsync(tipo);

            TipoResponseDto responseDto = _mapper.Map<TipoResponseDto>(updatedTipo);
            return Ok(new ApiResponse<TipoResponseDto>(responseDto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            bool deleted = await _tipoService.DeleteAsync(id);
            if (!deleted)
                throw new InvalidOperationException($"No se pudo eliminar el tipo con ID {id}"); // Será capturado por el middleware

            return Ok(new ApiResponse<string>($"Tipo con ID {id} eliminado correctamente"));
        }
    }
}
