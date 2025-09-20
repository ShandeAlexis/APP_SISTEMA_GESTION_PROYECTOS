using System;
using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public class IncidenciaRepository: IIncidenciaRepository
{
 private readonly SISTEMAS_API_DBContext _context;

    public IncidenciaRepository(SISTEMAS_API_DBContext dbcontext)
    {
        _context = dbcontext;
    }

    public async Task<IEnumerable<Incidencia>> GetAllIncidencias()
    {
        return await _context.Incidencias
            .Include(i => i.Proyecto)
            .ToListAsync();
    }

    public async Task<Incidencia?> GetIncidenciaById(int id)
    {
        return await _context.Incidencias
            .Include(i => i.Proyecto)
            .FirstOrDefaultAsync(i => i.INCIinID == id);
    }

    public async Task AddIncidencia(Incidencia incidencia)
    {
        await _context.Incidencias.AddAsync(incidencia);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateIncidencia(Incidencia incidencia)
    {
        _context.Incidencias.Update(incidencia);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteIncidencia(int id)
    {
        var incidencia = await _context.Incidencias.FindAsync(id);
        if (incidencia == null) return false;

        _context.Incidencias.Remove(incidencia);
        await _context.SaveChangesAsync();
        return true;
    }
}
