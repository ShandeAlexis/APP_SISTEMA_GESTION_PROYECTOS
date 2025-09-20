using System;
using SISTEMA.API.SISTEMAS_api.Core.Models.Contrato;
using SISTEMA.API.SISTEMAS_api.Core.Models.Curva;

namespace SISTEMA.API.SISTEMAS_api.Core.Interfaces;

public interface IContratoService
{
    Task<IEnumerable<ContratoDTO>> GetContratos();

    Task<ContratoDTO?> GetContrato(int id);
    Task<IEnumerable<ContratoDTO>> GetContratosByProyectoId(int proyectoId);

    Task<ContratoDTO> CreateContrato(ContratoCreateDTO contratoCreateDTO);
    Task<ContratoDTO?> UpdateContrato(int id, ContratoCreateDTO createDTO);
    Task<bool> DeleteContrato(int id);

    Task<IEnumerable<CurvaDTO>> CalculateCurvasForContratoAsync(int idContrato, string tipoCurva);
}
