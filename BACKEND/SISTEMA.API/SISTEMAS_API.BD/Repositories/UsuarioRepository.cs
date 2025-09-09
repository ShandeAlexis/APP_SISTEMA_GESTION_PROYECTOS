using System;
using Microsoft.EntityFrameworkCore;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
 private readonly SISTEMAS_API_DBContext _context;

    public UsuarioRepository(SISTEMAS_API_DBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetAllUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario?> GetUsuarioById(int id)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.USUAinID == id);
    }

    public async Task<Usuario?> GetUsuarioByEmail(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.USUAchEmail == email);
    }

    public async Task<Usuario> AddUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task UpdateUsuario(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.USUAinID == id);
        if (usuario == null) return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }
}
