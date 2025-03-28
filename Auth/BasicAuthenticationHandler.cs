using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using EventosApi.Services;

namespace EventosApi.Auth
{
    // 📍 Este Handler reemplaza al AuthenticationManager + UserDetailsService de Spring
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUsuarioService _usuarioService;

        // 🔧 Constructor con dependencias inyectadas
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUsuarioService usuarioService // ⬅️ Equivalente a inyectar un UserDetailsService
        ) : base(options, logger, encoder, clock)
        {
            _usuarioService = usuarioService;
        }

        // 🛡️ Este método se ejecuta en cada request protegida por [Authorize]
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // ✅ 1. Verifica que venga el header Authorization (como hace Spring en los filtros)
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                // ✅ 2. Decodifica el header Authorization: Basic base64(username:password)
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

                string username = credentials[0];
                string password = credentials[1];

                // ✅ 3. Valida el usuario contra la base de datos
                // ⬅️ Esto es como el método `loadUserByUsername` + `BCrypt.matches(...)` en Spring
                var usuario = await _usuarioService.ValidateUserAsync(username, password);

                if (usuario == null)
                    return AuthenticateResult.Fail("Invalid username or password");

                // ✅ 4. Crea los claims (datos de autenticación del usuario)
                var claims = new[] {
                    new Claim(ClaimTypes.Name, usuario.Username),
                    // Si tuvieras roles: new Claim(ClaimTypes.Role, "ADMIN")
                };

                // 📦 5. Crea una identidad y construye el ticket de autenticación
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }
    }
}
