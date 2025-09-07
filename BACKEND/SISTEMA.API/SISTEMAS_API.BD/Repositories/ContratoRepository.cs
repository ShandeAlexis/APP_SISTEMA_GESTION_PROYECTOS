using System;
using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public class ContratoRepository : IContratoRepository
{
    private readonly SISTEMAS_API_DBContext _context;

    public ContratoRepository(SISTEMAS_API_DBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contrato>> GetAllContratos()
    {
        return await _context.Contratos
            .Include(c => c.Proyecto)
            .ToListAsync();
    }

    public async Task<Contrato?> GetContratoById(int id)
    {
        return await _context.Contratos
            .Include(c => c.Proyecto)
            .FirstOrDefaultAsync(c => c.CONTinID == id);
    }

    public async Task AddContrato(Contrato contrato)
    {
        await _context.Contratos.AddAsync(contrato);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateContrato(Contrato contrato)
    {
        _context.Contratos.Update(contrato);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteContrato(int id)
    {
        var contrato = await _context.Contratos.FindAsync(id);
        if (contrato == null) return false;

        _context.Contratos.Remove(contrato);
        await _context.SaveChangesAsync();
        return true;
    }
}
