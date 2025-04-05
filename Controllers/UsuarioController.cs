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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async
        Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetAll()
        {
            IEnumerable<Usuario> usuarios = await _usuarioService.GetAllAsync();

            IEnumerable<UsuarioResponseDto> usuarioDtos = _mapper.Map<IEnumerable<UsuarioResponseDto>>(usuarios);

            return Ok(new ApiResponse<IEnumerable<UsuarioResponseDto>>(usuarioDtos));
        }

    }
}