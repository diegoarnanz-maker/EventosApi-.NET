using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Configurations;
using EventosApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventosApi.Services
{
    public class UserDetailsServiceImplSql : IUserValidationService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UserDetailsServiceImplSql(AppDbContext context, IPasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // Metodo para validar usuario
        public async Task<Usuario?> ValidateUserAsync(string username, string password)
        {
            // 1. Buscar al usuario por su nombre de usuario en la base de datos
            Usuario? usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.Enabled);

            // Si el usuario no existe o está deshabilitado, retornar null
            if (usuario == null)
            {
                return null;
            }

            // 2. Validar la contraseña usando el PasswordHasher
            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, password);

            // Si la validación falla, retornar null
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            // 3. Si las credenciales son correctas, retornar el usuario
            return usuario;
        }
    }
}