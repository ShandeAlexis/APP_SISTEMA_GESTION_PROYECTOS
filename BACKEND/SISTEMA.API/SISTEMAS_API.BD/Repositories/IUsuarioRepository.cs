using System;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllUsuarios();
    Task<Usuario?> GetUsuarioById(int id);
    Task<Usuario?> GetUsuarioByEmail(string email);
    Task<Usuario> AddUsuario(Usuario usuario);
    Task UpdateUsuario(Usuario usuario);
    Task<bool> DeleteUsuario(int id);
}
