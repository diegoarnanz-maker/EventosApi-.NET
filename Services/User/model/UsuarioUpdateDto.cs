using EventosApi.Models;

namespace EventosApi.Services.User.model
{
    public class UsuarioUpdateDto
    {
        public string? Email { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? Direccion { get; set; }

        public void UpdateEntity(Usuario usuario)
        {
            if (!string.IsNullOrWhiteSpace(Email)) usuario.Email = Email;
            if (!string.IsNullOrWhiteSpace(Nombre)) usuario.Nombre = Nombre;
            if (!string.IsNullOrWhiteSpace(Apellidos)) usuario.Apellidos = Apellidos;
            if (!string.IsNullOrWhiteSpace(Direccion)) usuario.Direccion = Direccion;
        }
    }
}
