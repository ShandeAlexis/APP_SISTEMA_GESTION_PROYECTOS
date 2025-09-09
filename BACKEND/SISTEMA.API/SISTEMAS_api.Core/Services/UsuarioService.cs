using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SISTEMA.API.SISTEMAS_api.Core.Constantes;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models.Usuario;
using SISTEMA.API.SISTEMAS_API.BD.Entities;
using SISTEMA.API.SISTEMAS_API.BD.Repositories;

namespace SISTEMA.API.SISTEMAS_api.Core.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository usuarioRepository;
    private readonly IConfiguration configuration; // 👈 para leer las claves del appsettings

    public UsuarioService(IUsuarioRepository usuarioRepo, IConfiguration config)
    {
        usuarioRepository = usuarioRepo;
        configuration = config;
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

    public async Task<AuthResponseDTO?> Login(string email, string password)
    {
        var usuario = await usuarioRepository.GetUsuarioByEmail(email);
        if (usuario == null) return null;

        var hash = HashPassword(password);
        if (usuario.USUAchPassword != hash) return null;

        // Generar token
        var key = configuration["Jwt:Key"] ?? "clave_super_secreta_123";
        var issuer = configuration["Jwt:Issuer"] ?? "SISTEMA.API";
        var token = GenerateJwtToken(usuario, key, issuer);

        var usuarioDTO = new UsuarioDTO
        {
            Id = usuario.USUAinID,
            Nombre = usuario.USUAchNombre,
            Email = usuario.USUAchEmail
        };

        return new AuthResponseDTO
        {
            Usuario = usuarioDTO,
            Token = token
        };
    }


    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private string GenerateJwtToken(Usuario usuario, string key, string issuer)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.USUAchEmail),
            new Claim("nombre", usuario.USUAchNombre),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
