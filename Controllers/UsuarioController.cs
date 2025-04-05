using EventosApi.Dtos;
using EventosApi.Services;
using EventosApi.Services.User.model;
using EventosApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UsuarioResponseDto>>>> GetAll()
        {
            var usuarios = await _usuarioService.GetAllDtosAsync();
            return Ok(SuccessResponse(usuarios));
        }

        [HttpGet("/buscar/{username}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<UsuarioDetalleResponseDto>>> GetById(string username)
        {
            var dto = await _usuarioService.GetDetalleByIdAsync(username);
            return Ok(SuccessResponse(dto));
        }

        [HttpGet("/buscar/role/{role}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UsuarioResponseDto>>>> GetByRole(string role)
        {
            var usuarios = await _usuarioService.FindByRolAsync(role);
            return Ok(SuccessResponse(usuarios));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Create([FromBody] UsuarioCreateDto dto)
        {
            var usuario = await _usuarioService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { username = usuario.Username }, SuccessResponse(usuario));
        }

        [HttpPut("/actualizar/{username}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Update(string username, [FromBody] UsuarioUpdateDto dto)
        {
            var usuario = await _usuarioService.UpdateAsync(username, dto);
            return Ok(SuccessResponse(usuario));
        }

        [HttpPut("cambiar-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> CambiarPassword([FromBody] UsuarioChangePasswordDto dto)
        {
            var actualizado = await _usuarioService.ChangePasswordAsync(Username, dto);
            return Ok(SuccessResponse(actualizado));
        }

        [HttpDelete("/eliminar-permanente/{username}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Delete(string username)
        {
            var usuario = await _usuarioService.DeleteAsync(username);
            return Ok(SuccessResponse(usuario));
        }

        [HttpPatch("desactivar/{username}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Desactivar(string username)
        {
            var desactivado = await _usuarioService.DesactivarUsuarioAsync(username);
            return Ok(SuccessResponse(desactivado));
        }

        [HttpPatch("activar/{username}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Activar(string username)
        {
            var activado = await _usuarioService.ActivarUsuarioAsync(username);
            return Ok(SuccessResponse(activado));
        }

    }
}