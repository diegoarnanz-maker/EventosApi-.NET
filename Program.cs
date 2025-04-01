using System.Text.Json.Serialization;
using EventosApi.Auth;
using EventosApi.Configurations;
using EventosApi.Models;
using EventosApi.Repositories;
using EventosApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("basic", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "basic",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Basic Authentication"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(Program));

// Configuración de PasswordHasher
builder.Services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

// Servicio de autenticación básica
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });

// Servicios de autorización por roles
builder.Services.AddAuthorization();

// Registro de servicios específicos
// @Repository
builder.Services.AddScoped<ITipoRepository, TipoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();

// @Service
builder.Services.AddScoped<ITipoService, TipoServiceImplSql>();
builder.Services.AddScoped<IUserValidationService, UserDetailsServiceImplSql>();
builder.Services.AddScoped<IUsuarioService, UsuarioServiceImplSql>();
builder.Services.AddScoped<IEventoService, EventoServiceImplSql>();
builder.Services.AddScoped<IReservaService, ReservaServiceImplSql>();

var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<EventosApi.Middlewares.ErrorHandlingMiddleware>();

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Configurar rutas de controladores
app.MapControllers();

app.Run();
