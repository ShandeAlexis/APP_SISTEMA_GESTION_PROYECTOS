using System;

namespace SISTEMA.API.SISTEMAS_api.Core.Models.Usuario;

public class UsuarioCreateDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
