using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Services;
using SISTEMA.API.SISTEMAS_API.BD;
using SISTEMA.API.SISTEMAS_API.BD.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000") // puerto donde corre React
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});


// Configurar DbContext con SQL Server
builder.Services.AddDbContext<SISTEMAS_API_DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositorios
builder.Services.AddScoped<IEntregableRepository, EntregableRepository>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();
builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Registrar servicios de la capa Core
builder.Services.AddScoped<IEntregableService, EntregableService>();
builder.Services.AddScoped<IContratoService, ContratoService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// OpenAPI / Swagger
builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");

// app.UseHttpsRedirection();
app.MapControllers();

app.Run();
