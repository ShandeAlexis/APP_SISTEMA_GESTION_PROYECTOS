using System;
using SISTEMA.API.SISTEMAS_api.Core.Models.Proyecto;

namespace SISTEMA.API.SISTEMAS_api.Core.Interfaces;

public interface IProyectoService
{
    Task<IEnumerable<ProyectoDTO>> GetProyectos();
    Task<ProyectoDTO?> GetProyecto(int id);
    Task<ProyectoDTO> CreateProyecto(ProyectoCreateDTO proyectoCreateDTO);
    Task<ProyectoDTO?> UpdateProyecto(int id, ProyectoCreateDTO proyectoCreateDTO);
    Task<bool> DeleteProyecto(int id);
}
