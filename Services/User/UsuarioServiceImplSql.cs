using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Configurations;
using EventosApi.Dtos;
using EventosApi.Exceptions;
using EventosApi.Models;
using EventosApi.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventosApi.Services
{
    public class UsuarioServiceImplSql : GenericoCRUDServiceImplSql<Usuario, int>, IUsuarioService
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioServiceImplSql(
            AppDbContext context,
            IUsuarioRepository usuarioRepository,
            IPasswordHasher<Usuario> passwordHasher,
            ILogger<UsuarioServiceImplSql> logger
        ) : base(context, logger)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        // Verifica si ya existe un usuario por su username
        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new BadRequestException("El nombre de usuario no puede estar vacío.");

            Usuario? usuario = await _usuarioRepository.FindByUsernameAsync(username);
            return usuario != null;
        }

        public async Task<Usuario?> FindByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new BadRequestException("El nombre de usuario no puede estar vacío.");

            Usuario? usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.Enabled);

            if (usuario == null)
                throw new NotFoundException("Usuario no encontrado o deshabilitado.");

            return usuario;
        }

        // Encripta la contraseña
        public string HashPassword(Usuario usuario, string rawPassword)
        {
            if (string.IsNullOrWhiteSpace(rawPassword))
                throw new BadRequestException("La contraseña no puede estar vacía.");

            return _passwordHasher.HashPassword(usuario, rawPassword);
        }

        public async Task<Usuario> RegisterAsync(RegisterRequestDto dto)
        {
            if (dto == null)
                throw new BadRequestException("Los datos de registro no pueden ser nulos.");

            // 1. Comprobar si ya existe el usuario
            Usuario? existingUser = await _usuarioRepository.FindByUsernameAsync(dto.Username);
            if (existingUser != null)
            {
                throw new BadRequestException("El nombre de usuario ya está en uso.");
            }

            // 2. Mapear DTO a entidad manualmente (sin usar AutoMapper dentro del servicio)
            Usuario nuevoUsuario = new Usuario
            {
                Username = dto.Username,
                Email = dto.Email,
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Direccion = dto.Direccion,
                Enabled = true,
                FechaRegistro = DateTime.UtcNow,
                Rol = "USER",
                Password = HashPassword(new Usuario(), dto.Password)
            };

            // 3. Guardar en BBDD
            Usuario usuarioCreado = await _usuarioRepository.CreateAsync(nuevoUsuario);
            return usuarioCreado;
        }

        // Único método requerido por la clase abstracta
        protected override DbSet<Usuario> GetDbSet()
        {
            return _context.Usuarios;
        }
    }
}
