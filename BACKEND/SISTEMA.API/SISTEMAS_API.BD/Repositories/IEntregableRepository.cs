
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public interface IEntregableRepository
{
    Task<IEnumerable<Entregable>> GetAllEntregables();
    Task<Entregable?> GetEntregableById(int id);
    Task<IEnumerable<Entregable>> GetEntregablesByContratoId(int contratoId);
    Task AddEntregable(Entregable entregable);
    Task UpdateEntregable(Entregable entregable);
    Task<bool> DeleteEntregable(int id);


    Task AddCurva(Curva curva);
    Task AddDetallesCurva(IEnumerable<DetalleCurva> detalles);


    // Lectura/edición de curvas
    Task<Curva?> GetCurvaById(int curvaId);
    Task<IEnumerable<DetalleCurva>> GetDetallesByCurva(int curvaId);
    Task UpdateDetallesCurva(IEnumerable<DetalleCurva> detalles);

    // Opcional (recomendado): atómico
    Task AddCurvaConDetallesAsync(Curva curva, IEnumerable<DetalleCurva> detalles);

}
