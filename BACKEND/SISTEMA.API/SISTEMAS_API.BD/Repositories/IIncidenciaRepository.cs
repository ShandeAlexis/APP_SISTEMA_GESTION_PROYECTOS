using System;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public interface IIncidenciaRepository
{
    Task<IEnumerable<Incidencia>> GetAllIncidencias();
    Task<Incidencia?> GetIncidenciaById(int id);
    Task AddIncidencia(Incidencia incidencia);
    Task UpdateIncidencia(Incidencia incidencia);
    Task<bool> DeleteIncidencia(int id);
}
