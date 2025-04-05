using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Dtos;
using EventosApi.Exceptions;
using EventosApi.Models;
using EventosApi.Repositories;
using EventosApi.Services.Base;
using EventosApi.Services.User.model;
using Microsoft.AspNetCore.Identity;

namespace EventosApi.Services
{
    public class UsuarioServiceImplSql : IGenericDtoService<Usuario, UsuarioCreateDto, UsuarioResponseDto, string>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioServiceImplSql(IUsuarioRepository usuarioRepository, IPasswordHasher<Usuario> passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto)
        {
            if (await ExistsByUsernameAsync(dto.Username))
                throw new BadRequestException("Ya existe un usuario con ese username.");

            Usuario user = (Usuario)dto;
            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            var creado = await _usuarioRepository.CreateAsync(user);
            return (UsuarioResponseDto)creado;
        }

        public async Task<UsuarioResponseDto> UpdateAsync(string username, UsuarioCreateDto dto)
        {
            var usuario = await _usuarioRepository.GetByUsernameAsync(username)
                          ?? throw new NotFoundException($"Usuario '{username}' no encontrado.");

            usuario.Email = dto.Email;
            usuario.Nombre = dto.Nombre;
            usuario.Apellidos = dto.Apellidos;
            usuario.Direccion = dto.Direccion;
            usuario.Rol = dto.Rol;

            var actualizado = await _usuarioRepository.UpdateAsync(usuario);
            return (UsuarioResponseDto)actualizado;
        }

        public async Task<UsuarioResponseDto> UpdateAsync(string username, UsuarioUpdateDto dto)
        {
            var usuario = await _usuarioRepository.GetByUsernameAsync(username)
                          ?? throw new NotFoundException("Usuario no encontrado.");

            if (dto.Email != null) usuario.Email = dto.Email;
            if (dto.Nombre != null) usuario.Nombre = dto.Nombre;
            if (dto.Apellidos != null) usuario.Apellidos = dto.Apellidos;
            if (dto.Direccion != null) usuario.Direccion = dto.Direccion;

            var actualizado = await _usuarioRepository.UpdateAsync(usuario);
            return (UsuarioResponseDto)actualizado;
        }

        public async Task<UsuarioResponseDto> ChangePasswordAsync(string username, UsuarioChangePasswordDto dto)
        {
            var usuario = await _usuarioRepository.GetByUsernameAsync(username)
                          ?? throw new NotFoundException("Usuario no encontrado.");

            var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, dto.CurrentPassword);
            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("La contraseña actual no es correcta.");

            usuario.Password = _passwordHasher.HashPassword(usuario, dto.NewPassword);
            var actualizado = await _usuarioRepository.UpdateAsync(usuario);
            return (UsuarioResponseDto)actualizado;
        }

        public async Task<IEnumerable<UsuarioResponseDto>> GetAllDtosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(u => (UsuarioResponseDto)u);
        }

        public async Task<UsuarioResponseDto> GetDtoByIdAsync(string username)
        {
            var usuario = await _usuarioRepository.GetByUsernameAsync(username)
                          ?? throw new NotFoundException($"Usuario '{username}' no encontrado.");
            return (UsuarioResponseDto)usuario;
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _usuarioRepository.ExistsByUsernameAsync(username);
        }

        public async Task<Usuario?> FindByUsernameAsync(string username)
        {
            return await _usuarioRepository.GetByUsernameAsync(username);
        }

        public async Task<IEnumerable<UsuarioResponseDto>> FindByNombreAsync(string nombre)
        {
            var lista = await _usuarioRepository.FindByNombreAsync(nombre);
            return lista.Select(u => (UsuarioResponseDto)u);
        }

        public async Task<IEnumerable<UsuarioResponseDto>> FindByRolAsync(string rol)
        {
            var lista = await _usuarioRepository.FindByRolAsync(rol);
            return lista.Select(u => (UsuarioResponseDto)u);
        }

        public async Task<bool> DeleteAsync(string username)
        {
            var usuario = await _usuarioRepository.GetByUsernameAsync(username)
                          ?? throw new NotFoundException($"Usuario '{username}' no encontrado.");

            return await _usuarioRepository.DeleteAsync(username);
        }

        public async Task<UsuarioResponseDto> DesactivarUsuarioAsync(string username)
        {
            var usuario = await _usuarioRepository.GetByUsernameAsync(username)
                          ?? throw new NotFoundException($"Usuario '{username}' no encontrado.");

            // Marcar como desactivado
            usuario.Enabled = false;

            var actualizado = await _usuarioRepository.UpdateAsync(usuario);
            return (UsuarioResponseDto)actualizado;
        }
        public async Task<UsuarioDetalleResponseDto> GetDetalleByIdAsync(string username)
        {
            var usuario = await _usuarioRepository.GetByUsernameAsync(username)
                          ?? throw new NotFoundException($"Usuario '{username}' no encontrado.");
            return (UsuarioDetalleResponseDto)usuario;
        }
        public async Task<UsuarioResponseDto> ActivarUsuarioAsync(string username)
        {
            var usuario = await _usuarioRepository.GetByUsernameAsync(username)
                          ?? throw new NotFoundException($"Usuario '{username}' no encontrado.");

            usuario.Enabled = true;
            var actualizado = await _usuarioRepository.UpdateAsync(usuario);

            return (UsuarioResponseDto)actualizado;
        }

    }
}
