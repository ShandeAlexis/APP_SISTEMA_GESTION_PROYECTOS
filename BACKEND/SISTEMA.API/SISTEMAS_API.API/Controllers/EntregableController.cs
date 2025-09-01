using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_api.Core.Constantes;
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
        try
        {
            return await _context.Entregables.ToListAsync();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Mensajes.General.ErrorInterno);
        }
    }

    // GET: api/Entregables/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Entregable>> GetEntregable(int id)
    {
        try
        {
            var entregable = await _context.Entregables.FindAsync(id);

            if (entregable == null)
                return NotFound(Mensajes.Entregable.NoEncontrado);

            return entregable;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Mensajes.General.ErrorInterno);
        }
    }

    // POST: api/Entregables
    [HttpPost]
    public async Task<ActionResult<Entregable>> PostEntregable(Entregable entregable)
    {
        try
        {
            _context.Entregables.Add(entregable);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEntregable), new { id = entregable.ENTRinID }, entregable);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Mensajes.General.ErrorInterno);
        }
    }

    // PUT: api/Entregables/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEntregable(int id, Entregable entregable)
    {
        if (id != entregable.ENTRinID)
            return BadRequest(Mensajes.Entregable.IdInvalido);

        var entregableExistente = await _context.Entregables.FindAsync(id);
        if (entregableExistente == null)
            return NotFound(Mensajes.Entregable.NoEncontrado);

        _context.Entry(entregableExistente).CurrentValues.SetValues(entregable);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Entregables.AnyAsync(e => e.ENTRinID == id))
                return NotFound(Mensajes.Entregable.NoEncontrado);
            else
                throw;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Mensajes.Entregable.ErrorActualizar);
        }

        return NoContent();
    }

    // DELETE: api/Entregables/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEntregable(int id)
    {
        try
        {
            var entregable = await _context.Entregables.FindAsync(id);
            if (entregable == null)
                return NotFound(Mensajes.Entregable.NoEncontrado);

            _context.Entregables.Remove(entregable);
            await _context.SaveChangesAsync();

            return Ok(Mensajes.Entregable.Eliminado);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Mensajes.General.ErrorInterno);
        }
    }
}
