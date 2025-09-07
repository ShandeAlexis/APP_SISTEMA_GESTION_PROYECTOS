
using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public class EntregableRepository : IEntregableRepository
{
    private readonly SISTEMAS_API_DBContext _context;

    public EntregableRepository(SISTEMAS_API_DBContext dbcontext)
    {
        _context = dbcontext;
    }

    public async Task<IEnumerable<Entregable>> GetAllEntregables()
    {
        return await _context.Entregables
                    .Include(e => e.Contrato)
                    .ThenInclude(c => c.Proyecto)
                    .ToListAsync();
    }

    public async Task<Entregable?> GetEntregableById(int id)
    {
        return await _context.Entregables
            .Include(e => e.Contrato)
            .ThenInclude(c => c.Proyecto)
            .FirstOrDefaultAsync(e => e.ENTRinID == id);
    }

    public async Task AddEntregable(Entregable entregable)
    {
        _context.Entregables.Add(entregable);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEntregable(Entregable entregable)
    {
        _context.Entregables.Update(entregable);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteEntregable(int id)
    {
        var entregable = await _context.Entregables.FindAsync(id);
        if (entregable == null)
        {
            return false;
        }

        _context.Entregables.Remove(entregable);
        await _context.SaveChangesAsync();
        return true;
    }

}
