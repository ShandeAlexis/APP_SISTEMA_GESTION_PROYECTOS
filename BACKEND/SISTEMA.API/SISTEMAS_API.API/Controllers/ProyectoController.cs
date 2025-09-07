using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SISTEMA.API.SISTEMAS_api.Core.Constantes;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models.Proyecto;

namespace SISTEMA.API.SISTEMAS_API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectoController : ControllerBase
    {
        private readonly IProyectoService proyectoService;

        public ProyectoController(IProyectoService IproyectoService)
        {
            proyectoService = IproyectoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProyectoDTO>>> GetProyectos()
        {
            try
            {
                var proyectos = await proyectoService.GetProyectos();
                return Ok(proyectos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Proyecto.ErrorObtener, detalle = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProyectoDTO>> GetProyecto(int id)
        {
            try
            {
                var proyecto = await proyectoService.GetProyecto(id);
                if (proyecto == null)
                {
                    return NotFound(new { mensaje = Mensajes.Proyecto.NoEncontrado });
                }
                return Ok(proyecto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Proyecto.ErrorObtener, detalle = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProyectoDTO>> CreateProyecto([FromBody] ProyectoCreateDTO proyectoCreateDTO)
        {
            try
            {
                if (proyectoCreateDTO == null)
                {
                    return BadRequest(new { mensaje = Mensajes.Proyecto.ErrorCrear });
                }

                var nuevoProyecto = await proyectoService.CreateProyecto(proyectoCreateDTO);
                return CreatedAtAction(nameof(GetProyecto), new { id = nuevoProyecto.Id }, nuevoProyecto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Proyecto.ErrorCrear, detalle = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProyectoDTO>> UpdateProyecto(int id, [FromBody] ProyectoCreateDTO proyectoCreateDTO)
        {
            try
            {
                if (proyectoCreateDTO == null)
                {
                    return BadRequest(new { mensaje = Mensajes.Proyecto.ErrorActualizar });
                }

                var proyectoActualizado = await proyectoService.UpdateProyecto(id, proyectoCreateDTO);
                if (proyectoActualizado == null)
                {
                    return NotFound(new { mensaje = Mensajes.Proyecto.NoEncontrado });
                }

                return Ok(proyectoActualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Proyecto.ErrorActualizar, detalle = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProyecto(int id)
        {
            try
            {
                var eliminado = await proyectoService.DeleteProyecto(id);
                if (!eliminado)
                {
                    return NotFound(new { mensaje = Mensajes.Proyecto.NoEncontrado });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Proyecto.ErrorEliminar, detalle = ex.Message });
            }
        }


    }
}
