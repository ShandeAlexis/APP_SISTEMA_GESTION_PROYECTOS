using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SISTEMA.API.SISTEMAS_api.Core.Constantes;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models.Incidencia;

namespace SISTEMA.API.SISTEMAS_API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidenciaController : ControllerBase
    {
        private readonly IIncidenciaService incidenciaService;

        public IncidenciaController(IIncidenciaService service)
        {
            incidenciaService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncidenciaDTO>>> GetIncidencias()
        {
            try
            {
                var incidencias = await incidenciaService.GetIncidencias();
                return Ok(incidencias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Incidencia.ErrorObtener, detalle = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IncidenciaDTO>> GetIncidencia(int id)
        {
            try
            {
                var incidencia = await incidenciaService.GetIncidencia(id);
                if (incidencia == null) return NotFound(new { mensaje = Mensajes.Incidencia.NoEncontrado });
                return Ok(incidencia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Incidencia.ErrorObtener, detalle = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult<IncidenciaDTO>> CreateIncidencia([FromBody] IncidenciaCreateDTO dto)
        {
            try
            {
                var nueva = await incidenciaService.CreateIncidencia(dto);

                return CreatedAtAction(nameof(GetIncidencia), new { id = nueva.Id }, nueva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Incidencia.ErrorCrear, detalle = ex.Message });
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<IncidenciaDTO>> UpdateIncidencia(int id, [FromBody] IncidenciaCreateDTO dto)
        {
            try
            {
                var actualizada = await incidenciaService.UpdateIncidencia(id, dto);
                if (actualizada == null) return NotFound(new { mensaje = Mensajes.Incidencia.NoEncontrado });
                return Ok(actualizada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Incidencia.ErrorActualizar, detalle = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncidencia(int id)
        {
            try
            {
                var eliminado = await incidenciaService.DeleteIncidencia(id);
                if (!eliminado) return NotFound(new { mensaje = Mensajes.Incidencia.NoEncontrado });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Incidencia.ErrorEliminar, detalle = ex.Message });
            }
        }
    }
}
