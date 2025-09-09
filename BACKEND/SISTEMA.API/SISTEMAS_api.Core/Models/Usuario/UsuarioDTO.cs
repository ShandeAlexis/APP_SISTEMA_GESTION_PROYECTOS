using System;

namespace SISTEMA.API.SISTEMAS_api.Core.Models.Usuario;

public class UsuarioDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
