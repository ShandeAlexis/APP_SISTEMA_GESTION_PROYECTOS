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

    [HttpGet("{id}/calcularCurva/{tipo}")]
    public async Task<ActionResult<IEnumerable<CurvaMensualDTO>>> CalcularCurvaMensual(int id, string tipo)
    {
        try
        {
            var curva = await entregableService.CalcularCurvaMensualAsync(id, tipo);
            return Ok(curva);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al calcular curva mensual", detalle = ex.Message });
        }
    }

    [HttpGet("contrato/{contratoId}")]
    public async Task<ActionResult<IEnumerable<EntregableDTO>>> GetEntregablesByContrato(int contratoId)
    {
        try
        {
            var entregables = await entregableService.GetEntregablesByContratoId(contratoId);
            if (entregables == null || !entregables.Any())
                return NotFound(new { mensaje = $"No se encontraron entregables para el contrato {contratoId}" });

            return Ok(entregables);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = Mensajes.Entregable.ErrorObtener, detalle = ex.Message });
        }
    }


}



