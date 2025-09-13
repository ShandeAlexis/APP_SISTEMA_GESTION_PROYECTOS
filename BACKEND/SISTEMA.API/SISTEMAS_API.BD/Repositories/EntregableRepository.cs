
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
    // 🔹 NUEVOS MÉTODOS
    public async Task AddCurva(Curva curva)
    {
        _context.Curvas.Add(curva);
        await _context.SaveChangesAsync();
    }

    public async Task AddDetallesCurva(IEnumerable<DetalleCurva> detalles)
    {
        _context.DetalleCurvas.AddRange(detalles);
        await _context.SaveChangesAsync();
    }

       public async Task<Curva?> GetCurvaById(int curvaId)
        {
            return await _context.Curvas
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CURVinID == curvaId);
        }

        public async Task<IEnumerable<DetalleCurva>> GetDetallesByCurva(int curvaId)
        {
            return await _context.DetalleCurvas
                .Where(d => d.CURVinID == curvaId)
                .OrderBy(d => d.DCURinPos)
                .ToListAsync();
        }

        public async Task UpdateDetallesCurva(IEnumerable<DetalleCurva> detalles)
        {
            // Si los detalles vienen "detached", marcamos como Modified
            foreach (var d in detalles)
            {
                // opcional: attach si no está siendo trackeado
                var tracked = _context.ChangeTracker.Entries<DetalleCurva>()
                               .FirstOrDefault(e => e.Entity.DCURinID == d.DCURinID);
                if (tracked == null)
                {
                    _context.DetalleCurvas.Attach(d);
                    _context.Entry(d).State = EntityState.Modified;
                }
                else
                {
                    // ya está trackeado, simplemente actualizar propiedades si lo necesitas
                    tracked.CurrentValues.SetValues(d);
                }
            }

            await _context.SaveChangesAsync();
        }

        // ------------------------
        // Opción recomendada: guardar curva + detalles en UNA transacción
        // ------------------------
        public async Task AddCurvaConDetallesAsync(Curva curva, IEnumerable<DetalleCurva> detalles)
        {
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Curvas.Add(curva);
                await _context.SaveChangesAsync(); // para obtener CURVinID si es autogenerado

                // asignar CURVinID a detalles (por si no lo hicieron)
                foreach (var d in detalles)
                {
                    d.CURVinID = curva.CURVinID;
                }

                _context.DetalleCurvas.AddRange(detalles);
                await _context.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
}
