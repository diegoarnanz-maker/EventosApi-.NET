using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventosApi.Dtos;
using EventosApi.Models;
using EventosApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReservaService _reservaService;

        public ReservaController(IMapper mapper, IReservaService reservaService)
        {
            _mapper = mapper;
            _reservaService = reservaService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<ReservaResponseDto>>> GetAll()
        {
            IEnumerable<Reserva> reservas = await _reservaService.GetAllAsync();
            IEnumerable<ReservaResponseDto> reservaDtos = _mapper.Map<IEnumerable<ReservaResponseDto>>(reservas);
            return Ok(reservaDtos);
        }

        [HttpGet("/mis-reservas")]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult<IEnumerable<ReservaResponseDto>>> GetMisReservas()
        {
            string? username = User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

            IEnumerable<Reserva> reservas = await _reservaService.FindByUsernameAsync(username);
            IEnumerable<ReservaResponseDto> reservaDtos = _mapper.Map<IEnumerable<ReservaResponseDto>>(reservas);
            return Ok(reservaDtos);
        }

        [HttpGet("/evento/{idEvento}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<ReservaResponseDto>>> GetByEventoId(int idEvento)
        {
            IEnumerable<Reserva> reservas = await _reservaService.FindByEventoIdAsync(idEvento);
            IEnumerable<ReservaResponseDto> dto = _mapper.Map<IEnumerable<ReservaResponseDto>>(reservas);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult<ReservaResponseDto>> Create([FromBody] ReservaRequestDto dto)
        {
            string? username = User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

            Reserva reserva = _mapper.Map<Reserva>(dto);
            reserva.Username = username;

            Reserva nuevaReserva = await _reservaService.CreateReservaAsync(reserva);
            ReservaResponseDto responseDto = _mapper.Map<ReservaResponseDto>(nuevaReserva);

            return CreatedAtAction(nameof(GetAll), new { id = responseDto.IdReserva }, responseDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ReservaResponseDto>> Update(int id, [FromBody] ReservaUpdateDto dto)
        {
            string? username = User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

            Reserva reservaActualizada = await _reservaService.UpdateReservaAsync(id, username, dto);
            ReservaResponseDto responseDto = _mapper.Map<ReservaResponseDto>(reservaActualizada);
            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<string>> Delete(int id)
        {
            string? username = User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedAccessException("No se pudo identificar al usuario.");

            bool isAdmin = User.IsInRole("ADMIN");
            bool eliminada = await _reservaService.DeleteReservaAsync(id, username, isAdmin);

            return Ok("Reserva eliminada correctamente.");
        }
    }
}
