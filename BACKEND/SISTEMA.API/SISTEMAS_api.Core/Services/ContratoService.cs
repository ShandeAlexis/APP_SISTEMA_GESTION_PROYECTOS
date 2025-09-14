
using SISTEMA.API.SISTEMAS_api.Core.Constantes;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models.Contrato;
using SISTEMA.API.SISTEMAS_API.BD.Entities;
using SISTEMA.API.SISTEMAS_API.BD.Repositories;

namespace SISTEMA.API.SISTEMAS_api.Core.Services;

public class ContratoService : IContratoService
{
    private readonly IContratoRepository contratoRepository;

    public ContratoService(IContratoRepository contratoRepo)
    {
        contratoRepository = contratoRepo;
    }

    public async Task<IEnumerable<ContratoDTO>> GetContratos()
    {
        var contratos = await contratoRepository.GetAllContratos();
        return contratos.Select(c => new ContratoDTO
        {
            Id = c.CONTinID,
            Codigo = c.CONTchCodigo,
            ProyectoCodigo = c.Proyecto.PROYchCodigo,
            Capex = c.CONTdeCapex,
            Costo = c.CONTdeCosto,
            Estimado = c.CONTdeEstimado,
            Contratista = c.CONTchContratista,
            Alcance = c.CONTchAlcance,
            EstadoCodigo = c.ECONchCodigo,
            Objetivo = c.CONTchObjetivo,
            FechaInicial = c.CONTdaFechaInicial,
            FechaFinal = c.CONTdaFechaFinal,
            CodigoContrato = c.CONTchCodigoContrato
        });
    }

    public async Task<ContratoDTO?> GetContrato(int id)
    {
        var c = await contratoRepository.GetContratoById(id);
        if (c == null) return null;

        return new ContratoDTO
        {
            Id = c.CONTinID,
            Codigo = c.CONTchCodigo,
            ProyectoCodigo = c.Proyecto.PROYchCodigo,
            Capex = c.CONTdeCapex,
            Costo = c.CONTdeCosto,
            Estimado = c.CONTdeEstimado,
            Contratista = c.CONTchContratista,
            Alcance = c.CONTchAlcance,
            EstadoCodigo = c.ECONchCodigo,
            Objetivo = c.CONTchObjetivo,
            FechaInicial = c.CONTdaFechaInicial,
            FechaFinal = c.CONTdaFechaFinal,
            CodigoContrato = c.CONTchCodigoContrato
        };
    }

    public async Task<IEnumerable<ContratoDTO>> GetContratosByProyectoId(int proyectoId)
    {
        var contratos = await contratoRepository.GetContratosByProyectoId(proyectoId);

        return contratos.Select(c => new ContratoDTO
        {
            Id = c.CONTinID,
            Codigo = c.CONTchCodigo,
            ProyectoCodigo = c.Proyecto.PROYchCodigo,
            Capex = c.CONTdeCapex,
            Costo = c.CONTdeCosto,
            Estimado = c.CONTdeEstimado,
            Contratista = c.CONTchContratista,
            Alcance = c.CONTchAlcance,
            EstadoCodigo = c.ECONchCodigo,
            Objetivo = c.CONTchObjetivo,
            FechaInicial = c.CONTdaFechaInicial,
            FechaFinal = c.CONTdaFechaFinal,
            CodigoContrato = c.CONTchCodigoContrato
        });
    }


    public async Task<ContratoDTO> CreateContrato(ContratoCreateDTO contratoCreateDTO)
    {
        var nuevoContrato = new Contrato
        {
            CONTchCodigo = contratoCreateDTO.Codigo,
            PROYinID = contratoCreateDTO.ProyectoId,
            CONTdeCapex = contratoCreateDTO.Capex,
            CONTdeCosto = contratoCreateDTO.Costo,
            CONTdeEstimado = contratoCreateDTO.Estimado,
            CONTchContratista = contratoCreateDTO.Contratista,
            CONTchAlcance = contratoCreateDTO.Alcance,
            ECONchCodigo = contratoCreateDTO.EstadoCodigo,
            CONTchObjetivo = contratoCreateDTO.Objetivo,
            CONTdaFechaInicial = contratoCreateDTO.FechaInicial,
            CONTdaFechaFinal = contratoCreateDTO.FechaFinal,
            CONTchCodigoContrato = contratoCreateDTO.CodigoContrato
        };

        await contratoRepository.AddContrato(nuevoContrato);

        var contratoGuardado = await contratoRepository.GetContratoById(nuevoContrato.CONTinID);
        if (contratoGuardado == null)
        {
            throw new Exception(Mensajes.Contrato.ErrorObtener);
        }
        return new ContratoDTO
        {
            Id = contratoGuardado.CONTinID,
            Codigo = contratoGuardado.CONTchCodigo,
            ProyectoCodigo = contratoGuardado.Proyecto.PROYchCodigo,
            Capex = contratoGuardado.CONTdeCapex,
            Costo = contratoGuardado.CONTdeCosto,
            Estimado = contratoGuardado.CONTdeEstimado,
            Contratista = contratoGuardado.CONTchContratista,
            Alcance = contratoGuardado.CONTchAlcance,
            EstadoCodigo = contratoGuardado.ECONchCodigo,
            Objetivo = contratoGuardado.CONTchObjetivo,
            FechaInicial = contratoGuardado.CONTdaFechaInicial,
            FechaFinal = contratoGuardado.CONTdaFechaFinal,
            CodigoContrato = contratoGuardado.CONTchCodigoContrato
        };
    }

    public async Task<ContratoDTO?> UpdateContrato(int id, ContratoCreateDTO createDTO)
    {
        var existingContrato = await contratoRepository.GetContratoById(id);
        if (existingContrato == null)
        {
            throw new Exception(Mensajes.Contrato.ErrorObtener);
        }

        existingContrato.CONTchCodigo = createDTO.Codigo;
        existingContrato.CONTchAlcance = createDTO.Alcance;
        existingContrato.CONTchObjetivo = createDTO.Objetivo;
        existingContrato.CONTchContratista = createDTO.Contratista;
        existingContrato.CONTdeCapex = createDTO.Capex;
        existingContrato.CONTdeCosto = createDTO.Costo;
        existingContrato.CONTdeEstimado = createDTO.Estimado;
        existingContrato.ECONchCodigo = createDTO.EstadoCodigo;
        existingContrato.CONTdaFechaInicial = createDTO.FechaInicial;
        existingContrato.CONTdaFechaFinal = createDTO.FechaFinal;
        existingContrato.CONTchCodigoContrato = createDTO.CodigoContrato;

        await contratoRepository.UpdateContrato(existingContrato);

        return new ContratoDTO
        {
            Id = existingContrato.CONTinID,
            Codigo = existingContrato.CONTchCodigo,
            ProyectoCodigo = existingContrato.Proyecto.PROYchCodigo,
            Alcance = existingContrato.CONTchAlcance,
            Objetivo = existingContrato.CONTchObjetivo,
            Contratista = existingContrato.CONTchContratista,
            Capex = existingContrato.CONTdeCapex,
            Costo = existingContrato.CONTdeCosto,
            Estimado = existingContrato.CONTdeEstimado,
            EstadoCodigo = existingContrato.ECONchCodigo,
            FechaInicial = existingContrato.CONTdaFechaInicial,
            FechaFinal = existingContrato.CONTdaFechaFinal,
            CodigoContrato = existingContrato.CONTchCodigoContrato
        };
    }

    public async Task<bool> DeleteContrato(int id)
    {
        return await contratoRepository.DeleteContrato(id);
    }
}
