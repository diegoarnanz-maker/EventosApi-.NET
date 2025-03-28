using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using EventosApi.Services;
using EventosApi.Models;

namespace EventosApi.Auth
{
    // 📍 Este Handler reemplaza al AuthenticationManager + UserDetailsService de Spring
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserValidationService _usuarioValidationService;

        // 🔧 Constructor con dependencias inyectadas
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserValidationService userValidationService // ⬅️ Equivalente a inyectar un UserDetailsService
        ) : base(options, logger, encoder, clock)
        {
            _usuarioValidationService = userValidationService;
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
                AuthenticationHeaderValue authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                byte[] credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
                string[] credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

                string username = credentials[0];
                string password = credentials[1];

                // ✅ 3. Valida el usuario contra la base de datos
                // ⬅️ Esto es como el método `loadUserByUsername` + `BCrypt.matches(...)` en Spring
                Usuario? usuario = await _usuarioValidationService.ValidateUserAsync(username, password);

                if (usuario == null)
                    return AuthenticateResult.Fail("Invalid username or password");

                // ✅ 4. Crea los claims (datos de autenticación del usuario)
                Claim[] claims = new[]
                {
                    new Claim(ClaimTypes.Name, usuario.Username),
                    // Si tuvieras roles: new Claim(ClaimTypes.Role, "ROLE_ADMIN")
                };

                // 📦 5. Crea una identidad y construye el ticket de autenticación
                ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }
    }
}
