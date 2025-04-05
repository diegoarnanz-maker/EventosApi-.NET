using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using EventosApi.Models;
using EventosApi.Services.Auth;
using EventosApi.Dtos;

namespace EventosApi.Auth
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthService _authService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthService authService
        ) : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

                string username = credentials[0];
                string password = credentials[1];

                // 👇 Adaptado: usamos el DTO de login
                var loginDto = new LoginRequestDto
                {
                    Username = username,
                    Password = password
                };

                // Llamamos a LoginAsync
                var usuario = await _authService.LoginAsync(loginDto);

                if (usuario == null)
                    return AuthenticateResult.Fail("Invalid username or password");

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, usuario.Username),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                };

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
