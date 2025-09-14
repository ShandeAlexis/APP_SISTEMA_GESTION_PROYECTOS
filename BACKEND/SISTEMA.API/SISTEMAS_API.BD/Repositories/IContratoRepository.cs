
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public interface IContratoRepository
{
    Task<IEnumerable<Contrato>> GetAllContratos();

    Task<Contrato?> GetContratoById(int id);

     Task<IEnumerable<Contrato>> GetContratosByProyectoId(int proyectoId); 
    Task AddContrato(Contrato contrato);
    Task UpdateContrato(Contrato contrato);
    Task<bool> DeleteContrato(int id);
}
