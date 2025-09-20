using System;
using SISTEMA.API.SISTEMAS_api.Core.Models.Incidencia;

namespace SISTEMA.API.SISTEMAS_api.Core.Interfaces;

public interface IIncidenciaService
{
    Task<IEnumerable<IncidenciaDTO>> GetIncidencias();
    Task<IncidenciaDTO?> GetIncidencia(int id);
    Task<IncidenciaDTO> CreateIncidencia(IncidenciaCreateDTO incidenciaCreateDTO);
    Task<IncidenciaDTO?> UpdateIncidencia(int id, IncidenciaCreateDTO incidenciaCreateDTO);
    Task<bool> DeleteIncidencia(int id);
}
