using System;
using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public class CurvaRepository : ICurvaRepository
{
    private readonly SISTEMAS_API_DBContext _context;
    public CurvaRepository(SISTEMAS_API_DBContext context)
    {
        _context = context;
    }
    public async Task<Curva?> GetCurvaById(int id)
    {
        return await _context.Curvas
        .Include(c => c.Detalles)
        .FirstOrDefaultAsync(c => c.CURVinID == id);
    }
    public async Task UpdateCurva(Curva curva)
    {
        _context.Curvas.Update(curva);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCurva(Curva curva)
    {
        _context.Curvas.Remove(curva);
        await _context.SaveChangesAsync();
    }

}
