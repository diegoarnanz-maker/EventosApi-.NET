using System.Collections.Generic;
using System.Threading.Tasks;
using EventosApi.Dtos;
using EventosApi.Services;
using EventosApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoController : BaseController
    {
        private readonly ITipoService _tipoService;

        public TipoController(ITipoService tipoService)
        {
            _tipoService = tipoService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<TipoResponseDto>>>> GetAll()
        {
            var tipos = await _tipoService.GetAllDtosAsync();
            return Ok(SuccessResponse(tipos));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<TipoResponseDto>>> GetById(int id)
        {
            var dto = await _tipoService.GetDtoByIdAsync(id);
            return Ok(SuccessResponse(dto));
        }

        [HttpGet("nombre/{nombre}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<TipoResponseDto>>>> FindByNombreContainsAsync(string nombre)
        {
            var tipoDtos = await _tipoService.FindByNombreContainsAsync(nombre);
            return Ok(SuccessResponse(tipoDtos));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<TipoResponseDto>>> Create([FromBody] TipoRequestDto dto)
        {
            var tipoResponseDto = await _tipoService.CreateAsync(dto);
            return Ok(SuccessResponse(tipoResponseDto));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<TipoResponseDto>>> Update(int id, [FromBody] TipoRequestDto dto)
        {
            var updatedDto = await _tipoService.UpdateAsync(id, dto);
            return Ok(SuccessResponse(updatedDto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            bool deleted = await _tipoService.DeleteAsync(id);
            if (!deleted)
                return BadRequest(FailedResponse<string>($"No se pudo eliminar el tipo con ID {id}"));

            return Ok(SuccessResponse($"Tipo con ID {id} eliminado correctamente"));
        }
    }
}
