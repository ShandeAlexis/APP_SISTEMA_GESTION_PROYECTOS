using System;
using System.Security.Cryptography;
using System.Text;
using SISTEMA.API.SISTEMAS_api.Core.Constantes;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models.Usuario;
using SISTEMA.API.SISTEMAS_API.BD.Entities;
using SISTEMA.API.SISTEMAS_API.BD.Repositories;

namespace SISTEMA.API.SISTEMAS_api.Core.Services;

public class UsuarioService: IUsuarioService
{
 private readonly IUsuarioRepository usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepo)
    {
        usuarioRepository = usuarioRepo;
    }

    public async Task<IEnumerable<UsuarioDTO>> GetUsuarios()
    {
        var usuarios = await usuarioRepository.GetAllUsuarios();
        return usuarios.Select(u => new UsuarioDTO
        {
            Id = u.USUAinID,
            Nombre = u.USUAchNombre,
            Email = u.USUAchEmail
        });
    }

    public async Task<UsuarioDTO?> GetUsuario(int id)
    {
        var u = await usuarioRepository.GetUsuarioById(id);
        if (u == null) return null;

        return new UsuarioDTO
        {
            Id = u.USUAinID,
            Nombre = u.USUAchNombre,
            Email = u.USUAchEmail
        };
    }

    public async Task<UsuarioDTO> CreateUsuario(UsuarioCreateDTO usuarioCreateDTO)
    {
        var nuevoUsuario = new Usuario
        {
            USUAchNombre = usuarioCreateDTO.Nombre,
            USUAchEmail = usuarioCreateDTO.Email,
            USUAchPassword = HashPassword(usuarioCreateDTO.Password)
        };

        await usuarioRepository.AddUsuario(nuevoUsuario);

        var usuarioGuardado = await usuarioRepository.GetUsuarioById(nuevoUsuario.USUAinID);
        if (usuarioGuardado == null)
            throw new Exception(Mensajes.Usuario.ErrorObtener);

        return new UsuarioDTO
        {
            Id = usuarioGuardado.USUAinID,
            Nombre = usuarioGuardado.USUAchNombre,
            Email = usuarioGuardado.USUAchEmail
        };
    }

    public async Task<UsuarioDTO?> UpdateUsuario(int id, UsuarioCreateDTO usuarioCreateDTO)
    {
        var existingUsuario = await usuarioRepository.GetUsuarioById(id);
        if (existingUsuario == null)
            throw new Exception(Mensajes.Usuario.ErrorObtener);

        existingUsuario.USUAchNombre = usuarioCreateDTO.Nombre;
        existingUsuario.USUAchEmail = usuarioCreateDTO.Email;

        if (!string.IsNullOrWhiteSpace(usuarioCreateDTO.Password))
            existingUsuario.USUAchPassword = HashPassword(usuarioCreateDTO.Password);

        await usuarioRepository.UpdateUsuario(existingUsuario);

        return new UsuarioDTO
        {
            Id = existingUsuario.USUAinID,
            Nombre = existingUsuario.USUAchNombre,
            Email = existingUsuario.USUAchEmail
        };
    }

    public async Task<bool> DeleteUsuario(int id)
    {
        return await usuarioRepository.DeleteUsuario(id);
    }

    public async Task<UsuarioDTO?> Login(string email, string password)
    {
        var usuario = await usuarioRepository.GetUsuarioByEmail(email);
        if (usuario == null) return null;

        var hash = HashPassword(password);
        if (usuario.USUAchPassword != hash) return null;

        return new UsuarioDTO
        {
            Id = usuario.USUAinID,
            Nombre = usuario.USUAchNombre,
            Email = usuario.USUAchEmail
        };
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
