using System;
using Microsoft.AspNetCore.Mvc;
using SISTEMA.API.SISTEMAS_api.Core.Constantes;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models;
using SISTEMA.API.SISTEMAS_api.Core.Models.Entregable;

namespace SISTEMA.API.SISTEMAS_API.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EntregableController : ControllerBase
{
    private readonly IEntregableService entregableService;
    public EntregableController(IEntregableService IEntregableService)
    {
        entregableService = IEntregableService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntregableDTO>>> GetEntregables()
    {
        try
        {
            var entregables = await entregableService.GetEntregables();
            return Ok(entregables);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = Mensajes.Entregable.ErrorObtener, detalle = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EntregableDTO>> GetEntregable(int id)
    {
        try
        {
            var entregable = await entregableService.GetEntregable(id);
            if (entregable == null)
                return NotFound(Mensajes.Entregable.NoEncontrado);

            return Ok(entregable);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = Mensajes.Entregable.ErrorObtener, detalle = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<EntregableDTO>> PostEntregable([FromBody] EntregableCreateDTO createDTO)
    {
        try
        {
            var nueva = await entregableService.CreateEntregable(createDTO);
            return CreatedAtAction(nameof(GetEntregable), new { id = nueva.Id }, nueva);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = Mensajes.Entregable.ErrorCrear, detalle = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEntregable(int id, [FromBody] EntregableCreateDTO createDTO)
    {
        try
        {
            var actualizado = await entregableService.UpdateEntregable(id, createDTO);
            if (actualizado == null)
                return NotFound(Mensajes.Entregable.NoEncontrado);
            return Ok(actualizado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = Mensajes.Entregable.ErrorActualizar, detalle = ex.Message });
        }
    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteEntregable(int id)
    {
        try
        {
            var eliminado = await entregableService.DeleteEntregable(id);
            if (!eliminado)
                return NotFound(new { mensaje = Mensajes.Entregable.NoEncontrado });
            return Ok(new { mensaje = Mensajes.Entregable.Eliminado });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = Mensajes.Entregable.ErrorEliminar, detalle = ex.Message });
        }
    }
}





// private readonly SISTEMAS_API_DBContext _context;

// public EntregableController(SISTEMAS_API_DBContext context)
// {
//     _context = context;
// }

// // GET: api/Entregables
// [HttpGet]
// public async Task<ActionResult<IEnumerable<EntregableDTO>>> GetEntregables()
// {
//     var entregables = await _context.Entregables
//         .Include(e => e.Contrato)
//         .ThenInclude(c => c.Proyecto)
//         .Select(e => new EntregableDTO
//         {
//             Id = e.ENTRinID,
//             Codigo = e.ENTRchCodigo,
//             PctContrato = e.ENTRdePctContrato,
//             FechaInicialPlan = e.ENTRdaFechaInicialPLAN,
//             DuracionPlanDias = e.ENTRinDuracionPlanDias,
//             FechaInicialReal = e.ENTRdaFechaInicialREAL,
//             DuracionRealDias = e.ENTRinDuracionRealDias,
//             ContratoCodigo = e.Contrato.CONTchCodigo,
//             ProyectoCodigo = e.Contrato.Proyecto.PROYchCodigo,
//             TipoEntregableCodigo = e.TENTchCodigo,
//             TipoProrrateoCodigo = e.TPROchCodigo,
//             EDTchCodigo = e.EDTchCodigo
//         })
//         .AsNoTracking()
//         .ToListAsync();

//     return Ok(entregables);
// }

// // GET: api/Entregables/5
// [HttpGet("{id}")]
// public async Task<ActionResult<EntregableDTO>> GetEntregable(int id)
// {
//     var entregable = await _context.Entregables
//         .Include(e => e.Contrato)
//         .ThenInclude(c => c.Proyecto)
//         .Where(e => e.ENTRinID == id)
//         .Select(e => new EntregableDTO
//         {
//             Id = e.ENTRinID,
//             Codigo = e.ENTRchCodigo,
//             PctContrato = e.ENTRdePctContrato,
//             FechaInicialPlan = e.ENTRdaFechaInicialPLAN,
//             DuracionPlanDias = e.ENTRinDuracionPlanDias,
//             FechaInicialReal = e.ENTRdaFechaInicialREAL,
//             DuracionRealDias = e.ENTRinDuracionRealDias,
//             ContratoCodigo = e.Contrato.CONTchCodigo,
//             ProyectoCodigo = e.Contrato.Proyecto.PROYchCodigo,
//             TipoEntregableCodigo = e.TENTchCodigo,
//             TipoProrrateoCodigo = e.TPROchCodigo,
//             EDTchCodigo = e.EDTchCodigo
//         })
//         .FirstOrDefaultAsync();

//     if (entregable == null)
//         return NotFound(Mensajes.Entregable.NoEncontrado);

//     return Ok(entregable);
// }

// // POST: api/Entregables
// [HttpPost]
// public async Task<ActionResult<EntregableDTO>> PostEntregable([FromBody] EntregableCreateDTO dto)
// {
//     if (!ModelState.IsValid)
//         return BadRequest(ModelState);

//     var entregable = new Entregable
//     {
//         ENTRchCodigo = dto.Codigo,
//         ENTRdePctContrato = dto.PctContrato,
//         ENTRdaFechaInicialPLAN = dto.FechaInicialPlan,
//         ENTRinDuracionPlanDias = dto.DuracionPlanDias,
//         ENTRdaFechaInicialREAL = dto.FechaInicialReal,
//         ENTRinDuracionRealDias = dto.DuracionRealDias,
//         CONTinID = dto.ContratoId,
//         TENTchCodigo = dto.TipoEntregableCodigo,
//         TPROchCodigo = dto.TipoProrrateoCodigo,
//         EDTchCodigo = dto.EDTchCodigo
//     };

//     _context.Entregables.Add(entregable);
//     await _context.SaveChangesAsync();

//     return CreatedAtAction(nameof(GetEntregable), new { id = entregable.ENTRinID }, dto);
// }

// // PUT: api/Entregables/5
// [HttpPut("{id}")]
// public async Task<IActionResult> PutEntregable(int id, [FromBody] EntregableCreateDTO dto)
// {
//     var entregable = await _context.Entregables.FindAsync(id);
//     if (entregable == null)
//         return NotFound(Mensajes.Entregable.NoEncontrado);

//     entregable.ENTRchCodigo = dto.Codigo;
//     entregable.ENTRdePctContrato = dto.PctContrato;
//     entregable.ENTRdaFechaInicialPLAN = dto.FechaInicialPlan;
//     entregable.ENTRinDuracionPlanDias = dto.DuracionPlanDias;
//     entregable.ENTRdaFechaInicialREAL = dto.FechaInicialReal;
//     entregable.ENTRinDuracionRealDias = dto.DuracionRealDias;
//     entregable.CONTinID = dto.ContratoId;
//     entregable.TENTchCodigo = dto.TipoEntregableCodigo;
//     entregable.TPROchCodigo = dto.TipoProrrateoCodigo;
//     entregable.EDTchCodigo = dto.EDTchCodigo;

//     await _context.SaveChangesAsync();
//     return NoContent();
// }

// // DELETE: api/Entregables/5
// [HttpDelete("{id}")]
// public async Task<IActionResult> DeleteEntregable(int id)
// {
//     var entregable = await _context.Entregables.FindAsync(id);
//     if (entregable == null)
//         return NotFound(Mensajes.Entregable.NoEncontrado);

//     _context.Entregables.Remove(entregable);
//     await _context.SaveChangesAsync();

//     return Ok(Mensajes.Entregable.Eliminado);
// }