using EventosApi.Dtos;
using EventosApi.Exceptions;
using EventosApi.Models;
using EventosApi.Services;
using EventosApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : BaseController
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReservaResponseDto>>>> GetAll()
        {
            var reservas = await _reservaService.GetAllDtosAsync();
            return Ok(SuccessResponse(reservas));
        }

        [HttpGet("/mis-reservas")]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReservaResponseDto>>>> GetMisReservas()
        {
            var reservas = await _reservaService.FindByUsernameAsync(Username);
            return Ok(SuccessResponse(reservas));
        }

        [HttpGet("/evento/{idEvento}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReservaResponseDto>>>> GetByEventoId(int idEvento)
        {
            var reservas = await _reservaService.FindByEventoIdAsync(idEvento);
            return Ok(SuccessResponse(reservas));
        }

        [HttpPost]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult<ApiResponse<ReservaResponseDto>>> Create([FromBody] ReservaRequestDto dto)
        {
            var nuevaReserva = await _reservaService.CreateAsync(dto, Username);
            return CreatedAtAction(nameof(GetAll), new { id = nuevaReserva.IdReserva }, SuccessResponse(nuevaReserva));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ReservaResponseDto>>> Update(int id, [FromBody] ReservaUpdateDto dto)
        {
            var actualizada = await _reservaService.UpdateAsync(id, Username, dto);
            return Ok(SuccessResponse(actualizada));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            bool eliminada = await _reservaService.DeleteAsync(id, Username, IsAdmin());

            if (!eliminada)
                return BadRequest(FailedResponse<string>("No se pudo eliminar la reserva."));

            return Ok(SuccessResponse("Reserva eliminada correctamente."));
        }
    }
}
