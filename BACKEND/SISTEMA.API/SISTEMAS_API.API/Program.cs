using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Services;
using SISTEMA.API.SISTEMAS_API.BD;
using SISTEMA.API.SISTEMAS_API.BD.Repositories;
using SISTEMA.API.SISTEMAS_api.Core.Config;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

var builder = WebApplication.CreateBuilder(args);

// ========================
// Configuración CORS
// ========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// ========================
// Configuración DbContext
// ========================
builder.Services.AddDbContext<SISTEMAS_API_DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========================
// Configuración JwtSettings
// ========================
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")); 

// ========================
// Registrar repositorios
// ========================
builder.Services.AddScoped<IEntregableRepository, EntregableRepository>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();
builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICurvaRepository, CurvaRepository>();
builder.Services.AddScoped<IIncidenciaRepository, IncidenciaRepository>();
// ========================
// Registrar servicios Core
// ========================
builder.Services.AddScoped<IEntregableService, EntregableService>();
builder.Services.AddScoped<IContratoService, ContratoService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICurvaService, CurvaService>();
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<IIncidenciaService, IncidenciaService>();

// ========================
// Configuración JWT
// ========================
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>(); 
if (jwtSettings is null)
{
    throw new InvalidOperationException("La sección 'Jwt' no está configurada en appsettings.json");
}
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key)),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role 
        };
    });

builder.Services.AddAuthorization();

// ========================
// Swagger/OpenAPI
// ========================
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");

// Middleware de autenticación/autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
