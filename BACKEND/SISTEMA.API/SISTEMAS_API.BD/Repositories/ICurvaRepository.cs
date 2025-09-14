using System;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_API.BD.Repositories;

public interface ICurvaRepository
{
    Task<Curva?> GetCurvaById(int id);
    Task UpdateCurva(Curva curva);
    Task DeleteCurva(Curva curva);

    Task<IEnumerable<Curva>> GetCurvasByEntregableId(int entregableId);


}
