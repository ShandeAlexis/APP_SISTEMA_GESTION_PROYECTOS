using System;
using SISTEMA.API.SISTEMAS_api.Core.Models.Usuario;

namespace SISTEMA.API.SISTEMAS_api.Core.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDTO>> GetUsuarios();
    Task<UsuarioDTO?> GetUsuario(int id);
    Task<UsuarioDTO> CreateUsuario(UsuarioCreateDTO usuarioCreateDTO);
    Task<UsuarioDTO?> UpdateUsuario(int id, UsuarioCreateDTO usuarioCreateDTO);
    Task<bool> DeleteUsuario(int id);
    Task<UsuarioDTO?> Login(string email, string password);
}
