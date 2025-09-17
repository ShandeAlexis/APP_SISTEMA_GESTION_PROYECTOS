using System;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_api.Core.Interfaces;

public interface IReporteService
{
     Task<ReporteDTO> GenerarReporteAsync(int proyectoId, DateTime fechaCorte);


}
