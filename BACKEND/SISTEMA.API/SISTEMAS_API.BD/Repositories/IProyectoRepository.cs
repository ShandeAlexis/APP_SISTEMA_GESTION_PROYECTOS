using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public interface IProyectoRepository
{
    Task<IEnumerable<Proyecto>> GetAllProyectos();
    Task<Proyecto?> GetProyectoById(int id);
    Task AddProyecto(Proyecto Proyecto);
    Task UpdateProyecto(Proyecto Proyecto);
    Task<bool> DeleteProyecto(int id);
}
