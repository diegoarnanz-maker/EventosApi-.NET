using System;
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
    public class EventoController : BaseController
    {
        private readonly IEventoService _eventoService;

        public EventoController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventoDetalleResponseDto>>>> GetAll()
        {
            var eventos = await _eventoService.GetAllDtosAsync();
            return Ok(SuccessResponse(eventos));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<EventoDetalleResponseDto>>> GetById(int id)
        {
            var dto = await _eventoService.GetDtoByIdAsync(id);
            return Ok(SuccessResponse(dto));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<EventoDetalleResponseDto>>> Create([FromBody] EventoRequestDto dto)
        {
            var nuevoDto = await _eventoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = nuevoDto.IdEvento }, SuccessResponse(nuevoDto));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<EventoDetalleResponseDto>>> Update(int id, [FromBody] EventoRequestDto dto)
        {
            var actualizado = await _eventoService.UpdateAsync(id, dto);
            return Ok(SuccessResponse(actualizado));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            var mensaje = await _eventoService.DeleteOrCancelAsync(id);
            return Ok(SuccessResponse(mensaje));
        }

        [HttpGet("nombre/{nombre}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventoDetalleResponseDto>>>> GetByNombre(string nombre)
        {
            var eventos = await _eventoService.FindByNombreContainsAsync(nombre);
            return Ok(SuccessResponse(eventos));
        }

        [HttpGet("tipo/{idTipo}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventoDetalleResponseDto>>>> GetByTipo(int idTipo)
        {
            var eventos = await _eventoService.FindByTipoIdAsync(idTipo);
            return Ok(SuccessResponse(eventos));
        }

        [HttpGet("entre-fechas")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventoDetalleResponseDto>>>> GetByFechaInicioYFin([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var eventos = await _eventoService.FindByFechaInicioAndFechaFinAsync(fechaInicio, fechaFin);
            return Ok(SuccessResponse(eventos));
        }
    }
}
