
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public interface IEntregableRepository
{
    Task<IEnumerable<Entregable>> GetAllEntregables();
    Task<Entregable?> GetEntregableById(int id);
    Task AddEntregable(Entregable entregable);
    Task UpdateEntregable(Entregable entregable);
    Task<bool> DeleteEntregable(int id);
    //  Task<bool> ExistsByName(string nombre, int? excludingId = null);
}
