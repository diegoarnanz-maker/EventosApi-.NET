using AutoMapper;
using EventosApi.Dtos;
using EventosApi.Models;
using EventosApi.Services;
using EventosApi.Utils;
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
            var usuarioResponse = _mapper.Map<UsuarioResponseDto>(usuario);

            // Devolver la respuesta con el usuario autenticado
            return Ok(new ApiResponse<UsuarioResponseDto>(usuarioResponse));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Register([FromBody] RegisterRequestDto dto)
        {
            // Verificar si el usuario ya existe
            bool exists = await _usuarioService.ExistsByUsernameAsync(dto.Username);
            if (exists)
            {
                return Conflict(new ApiResponse<string>("El usuario ya existe."));
            }

            // Mapear DTO → Entidad
            Usuario usuario = _mapper.Map<Usuario>(dto);

            // Encriptar la contraseña y otros datos
            usuario.Password = _usuarioService.HashPassword(usuario, dto.Password);
            usuario.Enabled = true;
            usuario.FechaRegistro = DateTime.UtcNow;

            // Guardar en BBDD
            Usuario nuevoUsuario = await _usuarioService.CreateAsync(usuario);

            // Mapear a DTO de respuesta
            UsuarioResponseDto responseDto = _mapper.Map<UsuarioResponseDto>(nuevoUsuario);
            return Created("", new ApiResponse<UsuarioResponseDto>(responseDto));
        }


    }

}
