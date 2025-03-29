using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Models;

namespace EventosApi.Services
{
    public interface IUserValidationService
    {
        // Metodo para validar usuario.
        public Task<Usuario?> ValidateUserAsync(string username, string password);
    }
}