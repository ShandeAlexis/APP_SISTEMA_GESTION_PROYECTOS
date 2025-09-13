using System;
using SISTEMA.API.SISTEMAS_api.Core.Models.Entregable;

namespace SISTEMA.API.SISTEMAS_api.Core.Interfaces;

public interface ICurvaService
{
    Task<IEnumerable<CurvaMensualDTO>?> GetCurvaAsync(int curvaId);
    Task<bool> UpdateCurvaAsync(int curvaId, IEnumerable<CurvaMensualDTO> nuevosDetalles);

    Task<bool> DeleteCurvaAsync(int curvaId);

}
