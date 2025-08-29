using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_API.BD;

var builder = WebApplication.CreateBuilder(args);
// Configurar DbContext con SQL Server
builder.Services.AddDbContext<SISTEMAS_API_DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


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


