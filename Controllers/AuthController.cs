using EventosApi.Dtos;
using EventosApi.Exceptions;
using EventosApi.Services.Auth;
using EventosApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventosApi.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Login([FromBody] LoginRequestDto loginDto)
        {
            var usuarioResponse = await _authService.LoginAsync(loginDto);

            if (usuarioResponse == null)
                throw new UnauthorizedException("Nombre de usuario o contraseña inválidos.");

            return Ok(SuccessResponse(usuarioResponse));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Register([FromBody] RegisterRequestDto dto)
        {
            var usuarioCreado = await _authService.RegisterAsync(dto);
            return Created("", SuccessResponse(usuarioCreado));
        }

        [HttpGet("me")]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> GetMe()
        {
            var usuario = await _authService.GetUsuarioAutenticadoAsync(Username);

            if (usuario == null)
                throw new UnauthorizedException("No se pudo obtener el usuario autenticado.");

            return Ok(SuccessResponse(usuario));
        }
    }
}
