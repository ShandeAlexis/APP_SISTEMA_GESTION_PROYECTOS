using Microsoft.AspNetCore.Mvc;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models.Curva;
using SISTEMA.API.SISTEMAS_api.Core.Models.Entregable;

namespace SISTEMA.API.SISTEMAS_API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurvaController : ControllerBase
    {
        private readonly ICurvaService curvaService;
        public CurvaController(ICurvaService curvaService)
        {
            this.curvaService = curvaService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CurvaMensualDTO>>> GetCurvas(int id)
        {
            try
            {
                var curvas = await curvaService.GetCurvaAsync(id);

                if (curvas == null)
                    return NotFound("La curva no existe."); // curva inexistente

                if (!curvas.Any())
                    return Ok(new List<CurvaMensualDTO>());

                return Ok(curvas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error en el servidor", detalle = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCurva(int id, [FromBody] IEnumerable<CurvaMensualDTO> nuevosDetalles)
        {
            try
            {
                var actualizado = await curvaService.UpdateCurvaAsync(id, nuevosDetalles);
                if (!actualizado)
                    return NotFound(new { mensaje = "Curva no encontrada." });

                return Ok(new { mensaje = "Curva actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar la curva.", detalle = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurva(int id)
        {
            try
            {
                var eliminado = await curvaService.DeleteCurvaAsync(id);
                if (!eliminado)
                    return NotFound(new { mensaje = "Curva no encontrada." });

                return Ok(new { mensaje = "Curva eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar la curva.", detalle = ex.Message });
            }
        }


        [HttpGet("entregable/{entregableId}")]
        public async Task<ActionResult<IEnumerable<CurvaDTO>>> GetCurvasByEntregable(int entregableId)
        {
            try
            {
                var curvas = await curvaService.GetCurvasByEntregableId(entregableId);

                if (!curvas.Any())
                    return NotFound(new { mensaje = "No se encontraron curvas para este entregable." });

                return Ok(curvas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener curvas", detalle = ex.Message });
            }
        }



    }
}
