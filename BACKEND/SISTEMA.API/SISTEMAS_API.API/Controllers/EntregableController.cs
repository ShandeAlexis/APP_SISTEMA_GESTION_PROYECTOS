using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_API.BD;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EntregableController : ControllerBase
{
    private readonly SISTEMAS_API_DBContext _context;

    public EntregableController(SISTEMAS_API_DBContext context)
    {
        _context = context;
    }

    // GET: api/Entregables
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Entregable>>> GetEntregables()
    {
        return await _context.Entregables.ToListAsync();
    }

    // GET: api/Entregables/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Entregable>> GetEntregable(int id)
    {
        var entregable = await _context.Entregables.FindAsync(id);

        if (entregable == null)
        {
            return NotFound();
        }

        return entregable;
    }

    // POST: api/Entregables
    [HttpPost]
    public async Task<ActionResult<Entregable>> PostEntregable(Entregable entregable)
    {
        _context.Entregables.Add(entregable);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEntregable), new { id = entregable.ENTRinID }, entregable);
    }

    // PUT: api/Entregables/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEntregable(int id, Entregable entregable)
    {
        if (id != entregable.ENTRinID)
        {
            return BadRequest();
        }

        _context.Entry(entregable).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Entregables.Any(e => e.ENTRinID == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Entregables/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEntregable(int id)
    {
        var entregable = await _context.Entregables.FindAsync(id);
        if (entregable == null)
        {
            return NotFound();
        }

        _context.Entregables.Remove(entregable);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
