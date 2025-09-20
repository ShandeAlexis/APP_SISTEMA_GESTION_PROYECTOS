using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SISTEMA.API.SISTEMAS_api.Core.Constantes;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models.Contrato;
using SISTEMA.API.SISTEMAS_api.Core.Models.Curva;

namespace SISTEMA.API.SISTEMAS_API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoController : ControllerBase
    {
        private readonly IContratoService contratoService;

        public ContratoController(IContratoService contratoService)
        {
            this.contratoService = contratoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContratoDTO>>> GetContratos()
        {
            try
            {
                var contratos = await contratoService.GetContratos();
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Contrato.ErrorObtener, detalle = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContratoDTO>> GetContrato(int id)
        {
            try
            {
                var contrato = await contratoService.GetContrato(id);
                if (contrato == null)
                    return NotFound(Mensajes.Contrato.NoEncontrado);

                return Ok(contrato);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Contrato.ErrorObtener, detalle = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ContratoDTO>> PostContrato([FromBody] ContratoCreateDTO createDTO)
        {
            try
            {
                var nuevo = await contratoService.CreateContrato(createDTO);
                return CreatedAtAction(nameof(GetContrato), new { id = nuevo.Id }, nuevo);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Contrato.ErrorCrear, detalle = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutContrato(int id, [FromBody] ContratoCreateDTO createDTO)
        {
            try
            {
                var actualizado = await contratoService.UpdateContrato(id, createDTO);
                if (actualizado == null)
                    return NotFound(Mensajes.Contrato.NoEncontrado);

                return Ok(actualizado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Contrato.ErrorActualizar, detalle = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContrato(int id)
        {
            try
            {
                var eliminado = await contratoService.DeleteContrato(id);
                if (!eliminado)
                    return NotFound(Mensajes.Contrato.NoEncontrado);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Contrato.ErrorEliminar, detalle = ex.Message });
            }
        }

        [HttpGet("proyecto/{proyectoId}")]
        public async Task<ActionResult<IEnumerable<ContratoDTO>>> GetContratosByProyecto(int proyectoId)
        {
            try
            {
                var contratos = await contratoService.GetContratosByProyectoId(proyectoId);
                if (contratos == null || !contratos.Any())
                    return NotFound(new { mensaje = $"No se encontraron contratos para el proyecto {proyectoId}" });

                return Ok(contratos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Contrato.ErrorObtener, detalle = ex.Message });
            }
        }



        [HttpGet("{id}/curvas")]
        public async Task<ActionResult<IEnumerable<CurvaDTO>>> GetCurvasContrato(int id, [FromQuery] string tipoCurva)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tipoCurva))
                    return BadRequest(new { mensaje = "Debe especificar el tipo de curva" });

                var curvas = await contratoService.CalculateCurvasForContratoAsync(id, tipoCurva);

                if (curvas == null || !curvas.Any())
                    return NotFound(new { mensaje = $"No se encontraron curvas para el contrato {id} y tipo {tipoCurva}" });

                return Ok(curvas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al calcular curvas del contrato", detalle = ex.Message });
            }
        }


    }
}