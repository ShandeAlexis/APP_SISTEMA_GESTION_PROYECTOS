using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public class ProyectoRepository : IProyectoRepository
{
    private readonly SISTEMAS_API_DBContext _context;

    public ProyectoRepository(SISTEMAS_API_DBContext dbcontext)
    {
        _context = dbcontext;
    }
    public async Task<IEnumerable<Proyecto>> GetAllProyectos()
    {
        return await _context.Proyectos
                    .Include(p => p.Empresa)
                    .ToListAsync();
    }

    public async Task<Proyecto?> GetProyectoById(int id)
    {
        return await _context.Proyectos
                    .Include(p => p.Empresa)
                    .FirstOrDefaultAsync(p => p.PROYinID == id);
    }
    public async Task AddProyecto(Proyecto proyecto)
    {
        await _context.Proyectos.AddAsync(proyecto);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProyecto(Proyecto proyecto)
    {
        _context.Proyectos.Update(proyecto);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteProyecto(int id)
    {
        var proyecto = await _context.Proyectos.FindAsync(id);
        if (proyecto == null) return false;

        _context.Proyectos.Remove(proyecto);
        await _context.SaveChangesAsync();
        return true;
    }


}
