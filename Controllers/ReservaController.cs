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
        public async
        Task<ActionResult<IEnumerable<ReservaResponseDto>>> GetAll()
        {
            IEnumerable<Reserva> reservas = await _reservaService.GetAllAsync();

            IEnumerable<ReservaResponseDto> reservaDtos = _mapper.Map<IEnumerable<ReservaResponseDto>>(reservas);

            return Ok(reservaDtos);
        }

        [HttpPost]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult<ReservaResponseDto>> Create([FromBody] ReservaRequestDto dto)
        {
            // Mapea el DTO a la entidad
            Reserva reserva = _mapper.Map<Reserva>(dto);

            // Obtener el username del usuario autenticado
            string username = User?.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("No se pudo identificar al usuario autenticado.");
            }

            // Asignar el username a la entidad antes de guardar
            reserva.Username = username;

            // Crear reserva
            Reserva nuevaReserva = await _reservaService.CreateReservaAsync(reserva);

            // Devolver respuesta
            ReservaResponseDto responseDto = _mapper.Map<ReservaResponseDto>(nuevaReserva);
            return CreatedAtAction(nameof(GetAll), new { id = responseDto.IdReserva }, responseDto);
        }


    }
}