using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Services;
using SISTEMA.API.SISTEMAS_API.BD;
using SISTEMA.API.SISTEMAS_API.BD.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ========================
// Configuración CORS
// ========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000") // puerto donde corre React
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// ========================
// Configuración DbContext
// ========================
builder.Services.AddDbContext<SISTEMAS_API_DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========================
// Registrar repositorios
// ========================
builder.Services.AddScoped<IEntregableRepository, EntregableRepository>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();
builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// ========================
// Registrar servicios Core
// ========================
builder.Services.AddScoped<IEntregableService, EntregableService>();
builder.Services.AddScoped<IContratoService, ContratoService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// ========================
// Configuración JWT
// ========================
var key = builder.Configuration["Jwt:Key"] ?? "clave_super_secreta_123";
var issuer = builder.Configuration["Jwt:Issuer"] ?? "SISTEMA.API";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
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

// JWT debe ir antes de MapControllers
app.UseAuthentication();
app.UseAuthorization();

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
