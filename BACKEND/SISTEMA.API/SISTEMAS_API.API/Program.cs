using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Services;
using SISTEMA.API.SISTEMAS_API.BD;
using SISTEMA.API.SISTEMAS_API.BD.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext con SQL Server
builder.Services.AddDbContext<SISTEMAS_API_DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositorios
builder.Services.AddScoped<IEntregableRepository, EntregableRepository>();

// Registrar servicios de la capa Core
builder.Services.AddScoped<IEntregableService, EntregableService>();

// OpenAPI / Swagger
builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
app.MapControllers();

app.Run();
