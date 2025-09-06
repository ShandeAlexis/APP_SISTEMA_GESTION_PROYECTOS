using System;
using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD;

public class SISTEMAS_API_DBContext : DbContext
{
    public SISTEMAS_API_DBContext(DbContextOptions<SISTEMAS_API_DBContext> options) : base(options){}
    public DbSet<Entregable> Entregables { get; set; } = null!;
}
