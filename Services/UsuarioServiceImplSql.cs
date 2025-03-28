using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Configurations;
using EventosApi.Models;
using EventosApi.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            Usuario? usuario = await _usuarioRepository.FindByUsernameAsync(username);
            return usuario != null;
        }

        // Encripta la contraseña
        public string HashPassword(Usuario usuario, string rawPassword)
        {
            return _passwordHasher.HashPassword(usuario, rawPassword);
        }

        protected override DbSet<Usuario> GetDbSet()
        {
            return _context.Usuarios;
        }
    }
}