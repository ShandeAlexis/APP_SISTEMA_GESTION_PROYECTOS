using System;

namespace SISTEMA.API.SISTEMAS_api.Core.Models.Usuario;

public class UsuarioDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}

public class AuthResponseDTO
{
    public UsuarioDTO Usuario { get; set; } = new UsuarioDTO();
    public string Token { get; set; } = string.Empty;
}