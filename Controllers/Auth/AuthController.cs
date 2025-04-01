using AutoMapper;
using EventosApi.Dtos;
using EventosApi.Exceptions;
using EventosApi.Models;
using EventosApi.Services;
using EventosApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventosApi.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserValidationService _userValidationService;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public AuthController(IUserValidationService userValidationService, IUsuarioService usuarioService, IMapper mapper)
        {
            _userValidationService = userValidationService;
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Login([FromBody] LoginRequestDto loginDto)
        {
            // Validar el usuario usando el servicio IUserValidationService
            Usuario? usuario = await _userValidationService.ValidateUserAsync(loginDto.Username, loginDto.Password);

            if (usuario == null)
            {
                // Lanzamos directamente una excepción personalizada
                throw new UnauthorizedException("Nombre de usuario o contraseña inválidos.");
            }

            UsuarioResponseDto usuarioResponse = _mapper.Map<UsuarioResponseDto>(usuario);
            return Ok(new ApiResponse<UsuarioResponseDto>(usuarioResponse));
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Register([FromBody] RegisterRequestDto dto)
        {
            Usuario nuevoUsuario = await _usuarioService.RegisterAsync(dto);

            UsuarioResponseDto responseDto = _mapper.Map<UsuarioResponseDto>(nuevoUsuario);
            return Created("", new ApiResponse<UsuarioResponseDto>(responseDto));
        }

        // GET: api/auth/me
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> GetMe()
        {
            string? username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedException("No se pudo obtener el usuario autenticado.");

            Usuario? usuario = await _usuarioService.FindByUsernameAsync(username);

            UsuarioResponseDto usuarioResponse = _mapper.Map<UsuarioResponseDto>(usuario);
            return Ok(new ApiResponse<UsuarioResponseDto>(usuarioResponse));
        }
    }
}
