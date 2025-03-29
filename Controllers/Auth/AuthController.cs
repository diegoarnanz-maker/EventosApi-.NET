using AutoMapper;
using EventosApi.Dtos;
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
                return Unauthorized(new ApiResponse<string>("Invalid username or password"));
            }

            // Mapear el usuario a un DTO de respuesta
            UsuarioResponseDto usuarioResponse = _mapper.Map<UsuarioResponseDto>(usuario);

            // Devolver la respuesta con el usuario autenticado
            return Ok(new ApiResponse<UsuarioResponseDto>(usuarioResponse));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Register([FromBody] RegisterRequestDto dto)
        {
            try
            {
                // Toda la lógica queda encapsulada en el servicio
                Usuario nuevoUsuario = await _usuarioService.RegisterAsync(dto);

                UsuarioResponseDto responseDto = _mapper.Map<UsuarioResponseDto>(nuevoUsuario);
                return Created("", new ApiResponse<UsuarioResponseDto>(responseDto));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiResponse<string>(ex.Message));
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> GetMe()
        {
            // Obtener el nombre de usuario desde el contexto de seguridad
            string? username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new ApiResponse<string>("No se pudo obtener el usuario autenticado."));
            }

            // Buscar el usuario en la base de datos
            Usuario? usuario = await _usuarioService.FindByUsernameAsync(username);

            if (usuario == null)
            {
                return NotFound(new ApiResponse<string>("Usuario no encontrado."));
            }

            // Mapear a DTO
            UsuarioResponseDto usuarioResponse = _mapper.Map<UsuarioResponseDto>(usuario);
            ApiResponse<UsuarioResponseDto> response = new ApiResponse<UsuarioResponseDto>(usuarioResponse);

            return Ok(response);
        }


    }

}
