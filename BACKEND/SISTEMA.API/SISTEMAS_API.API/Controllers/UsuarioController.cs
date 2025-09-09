using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SISTEMA.API.SISTEMAS_api.Core.Constantes;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models.Usuario;

namespace SISTEMA.API.SISTEMAS_API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        // ✅ Requiere token
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            try
            {
                var usuarios = await usuarioService.GetUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Usuario.ErrorObtener, detalle = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            try
            {
                var usuario = await usuarioService.GetUsuario(id);
                if (usuario == null)
                    return NotFound(Mensajes.Usuario.NoEncontrado);

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Usuario.ErrorObtener, detalle = ex.Message });
            }
        }

        [HttpPost]
        [Authorize] // ✅ solo usuarios autenticados pueden crear otros usuarios
        public async Task<ActionResult<UsuarioDTO>> PostUsuario([FromBody] UsuarioCreateDTO createDTO)
        {
            try
            {
                var nuevo = await usuarioService.CreateUsuario(createDTO);
                return CreatedAtAction(nameof(GetUsuario), new { id = nuevo.Id }, nuevo);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Usuario.ErrorCrear, detalle = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioCreateDTO updateDTO)
        {
            try
            {
                var actualizado = await usuarioService.UpdateUsuario(id, updateDTO);
                if (actualizado == null)
                    return NotFound(Mensajes.Usuario.NoEncontrado);

                return Ok(actualizado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Usuario.ErrorActualizar, detalle = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var eliminado = await usuarioService.DeleteUsuario(id);
                if (!eliminado)
                    return NotFound(Mensajes.Usuario.NoEncontrado);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Usuario.ErrorEliminar, detalle = ex.Message });
            }
        }

        // ✅ Login no requiere token
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            try
            {
                var authResponse = await usuarioService.Login(loginDTO.Email, loginDTO.Password);
                if (authResponse == null)
                    return Unauthorized(Mensajes.Usuario.CredencialesInvalidas);

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = Mensajes.Usuario.ErrorLogin, detalle = ex.Message });
            }
        }
    }

    // DTO solo para login
    public class LoginRequestDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
