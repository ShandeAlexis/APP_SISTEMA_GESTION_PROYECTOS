using System;
using SISTEMA.API.SISTEMAS_api.Core.Models;
using SISTEMA.API.SISTEMAS_api.Core.Models.Entregable;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_api.Core.Interfaces;

public interface IEntregableService
{
    // Task<PagedResult<CategoriaDTO>> GetCategoriasPaged(PagedRequest request);
    Task<IEnumerable<EntregableDTO>> GetEntregables();
    Task<EntregableDTO?> GetEntregable(int id);
    Task<EntregableDTO> CreateEntregable(EntregableCreateDTO entregableCreateDTO);
    Task<EntregableDTO?> UpdateEntregable(int id, EntregableCreateDTO createDTO);
    Task<bool> DeleteEntregable(int id);


    Task<IEnumerable<CurvaMensualDTO>> CalcularCurvaMensualAsync(int entregableId, string tipoCurvaCodigo);

}
