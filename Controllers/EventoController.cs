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
    public class EventoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEventoService _eventoService;

        public EventoController(IMapper mapper, IEventoService eventoService)
        {
            _mapper = mapper;
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventoResponseDto>>>> GetAll()
        {
            IEnumerable<Evento> eventos = await _eventoService.GetAllAsync();

            IEnumerable<EventoResponseDto> eventoDtos = _mapper.Map<IEnumerable<EventoResponseDto>>(eventos);

            // ApiResponse<IEnumerable<EventoResponseDto>> response = new ApiResponse<IEnumerable<EventoResponseDto>>(eventoDtos);
            var response = new ApiResponse<IEnumerable<EventoResponseDto>>(eventoDtos);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async
        Task<ActionResult<ApiResponse<EventoDetalleResponseDto>>> GetById(int id)
        {
            Evento? evento = await _eventoService.GetByIdAsync(id);

            if (evento == null)
            {
                return NotFound(new ApiResponse<EventoDetalleResponseDto>("Evento no encontrado"));
            }

            EventoDetalleResponseDto eventoDto = _mapper.Map<EventoDetalleResponseDto>(evento);

            var response = new ApiResponse<EventoDetalleResponseDto>(eventoDto);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<EventoDetalleResponseDto>>> Create([FromBody] EventoRequestDto dto)
        {
            Evento evento = _mapper.Map<Evento>(dto);

            // Estado por defecto
            evento.Estado = EstadoEvento.ACEPTADO;

            await _eventoService.CreateAsync(evento);

            // Recargar para incluir el tipo
            Evento eventoCompleto = (await _eventoService.GetByIdAsync(evento.IdEvento))!;

            EventoDetalleResponseDto eventoDto = _mapper.Map<EventoDetalleResponseDto>(eventoCompleto);

            var response = new ApiResponse<EventoDetalleResponseDto>(eventoDto, "Evento creado con éxito");

            return CreatedAtAction(nameof(GetById), new { id = evento.IdEvento }, response);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<EventoDetalleResponseDto>>> Update(int id, [FromBody] EventoRequestDto dto)
        {
            Evento? evento = await _eventoService.GetByIdAsync(id);

            if (evento == null)
            {
                return NotFound(new ApiResponse<EventoDetalleResponseDto>("Evento no encontrado"));
            }

            evento = _mapper.Map(dto, evento);

            await _eventoService.UpdateAsync(evento);

            Evento eventoActualizado = (await _eventoService.GetByIdAsync(id))!;

            EventoDetalleResponseDto eventoDto = _mapper.Map<EventoDetalleResponseDto>(eventoActualizado);

            var response = new ApiResponse<EventoDetalleResponseDto>(eventoDto, "Evento actualizado con éxito");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            Evento? evento = await _eventoService.GetByIdAsync(id);

            if (evento == null)
            {
                return NotFound(new ApiResponse<string>("Evento no encontrado"));
            }

            if (evento.Reservas != null && evento.Reservas.Any())
            {
                // Si tiene reservas, marco el evento como cancelado
                // HAY QUE PROBARLO CUANDO CREEMOS RESERVACONTROLLER POST
                evento.Estado = EstadoEvento.CANCELADO;
                await _eventoService.UpdateAsync(evento);
                return Ok(new ApiResponse<string>("El evento tiene reservas. Se ha cambiado su estado a CANCELADO."));
            }

            // Si no tiene reservas, lo elimino
            await _eventoService.DeleteAsync(id);
            return Ok(new ApiResponse<string>("Evento eliminado correctamente."));
        }

        // Endpoins distintos a CRUD
        [HttpGet("nombre/{nombre}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventoDetalleResponseDto>>>> GetByNombre(string nombre)
        {
            var eventos = await _eventoService.FindByNombreContainsAsync(nombre);
            var dto = _mapper.Map<IEnumerable<EventoDetalleResponseDto>>(eventos);
            return Ok(new ApiResponse<IEnumerable<EventoDetalleResponseDto>>(dto));
        }

        [HttpGet("tipo/{idTipo}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventoDetalleResponseDto>>>> GetByTipo(int idTipo)
        {
            var eventos = await _eventoService.FindByTipoIdAsync(idTipo);
            var dto = _mapper.Map<IEnumerable<EventoDetalleResponseDto>>(eventos);
            return Ok(new ApiResponse<IEnumerable<EventoDetalleResponseDto>>(dto));
        }

        [HttpGet("entre-fechas")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventoDetalleResponseDto>>>> GetByFechaInicioYFin([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var eventos = await _eventoService.FindByFechaInicioAndFechaFinAsync(fechaInicio, fechaFin);
            var dto = _mapper.Map<IEnumerable<EventoDetalleResponseDto>>(eventos);
            return Ok(new ApiResponse<IEnumerable<EventoDetalleResponseDto>>(dto));
        }

    }
}
