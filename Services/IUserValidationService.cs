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
        private readonly IMapper _mapper;

        public AuthController(IUserValidationService userValidationService, IMapper mapper)
        {
            _userValidationService = userValidationService;
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
    }

}
