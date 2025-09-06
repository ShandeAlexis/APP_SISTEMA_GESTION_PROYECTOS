using System;
using SISTEMA.API.SISTEMAS_api.Core.Constantes;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models;
using SISTEMA.API.SISTEMAS_api.Core.Models.Entregable;
using SISTEMA.API.SISTEMAS_API.BD.Entities;
using SISTEMA.API.SISTEMAS_API.BD.Repositories;

namespace SISTEMA.API.SISTEMAS_api.Core.Services;

public class EntregableService : IEntregableService
{
    private readonly IEntregableRepository entregableRepository;

    public EntregableService(IEntregableRepository entregableRepo)
    {
        entregableRepository = entregableRepo;
    }

    public async Task<IEnumerable<EntregableDTO>> GetEntregables()
    {
        var entregables = await entregableRepository.GetAllEntregables();

        return entregables.Select(e => new EntregableDTO
        {
            Id = e.ENTRinID,
            Codigo = e.ENTRchCodigo,
            PctContrato = e.ENTRdePctContrato,
            FechaInicialPlan = e.ENTRdaFechaInicialPLAN,
            DuracionPlanDias = e.ENTRinDuracionPlanDias,
            FechaInicialReal = e.ENTRdaFechaInicialREAL,
            DuracionRealDias = e.ENTRinDuracionRealDias,
            ContratoCodigo = e.Contrato.CONTchCodigo,
            ProyectoCodigo = e.Contrato.Proyecto.PROYchCodigo,
            TipoEntregableCodigo = e.TENTchCodigo,
            TipoProrrateoCodigo = e.TPROchCodigo,
            EDTchCodigo = e.EDTchCodigo,
        });
    }

    public async Task<EntregableDTO?> GetEntregable(int id)
    {
        var e = await entregableRepository.GetEntregableById(id);
        if (e == null) return null;

        return new EntregableDTO
        {
            Id = e.ENTRinID,
            Codigo = e.ENTRchCodigo,
            PctContrato = e.ENTRdePctContrato,
            FechaInicialPlan = e.ENTRdaFechaInicialPLAN,
            DuracionPlanDias = e.ENTRinDuracionPlanDias,
            FechaInicialReal = e.ENTRdaFechaInicialREAL,
            DuracionRealDias = e.ENTRinDuracionRealDias,
            ContratoCodigo = e.Contrato.CONTchCodigo,
            ProyectoCodigo = e.Contrato.Proyecto.PROYchCodigo,
            TipoEntregableCodigo = e.TENTchCodigo,
            TipoProrrateoCodigo = e.TPROchCodigo,
            EDTchCodigo = e.EDTchCodigo,
        };
    }

    public async Task<EntregableDTO> CreateEntregable(EntregableCreateDTO entregableCreateDTO)
    {
        var nuevoEntregable = new Entregable
        {
            ENTRchCodigo = entregableCreateDTO.Codigo,
            ENTRdePctContrato = entregableCreateDTO.PctContrato,
            ENTRdaFechaInicialPLAN = entregableCreateDTO.FechaInicialPlan,
            ENTRinDuracionPlanDias = entregableCreateDTO.DuracionPlanDias,
            ENTRdaFechaInicialREAL = entregableCreateDTO.FechaInicialReal,
            ENTRinDuracionRealDias = entregableCreateDTO.DuracionRealDias,
            CONTinID = entregableCreateDTO.ContratoId,
            TENTchCodigo = entregableCreateDTO.TipoEntregableCodigo,
            TPROchCodigo = entregableCreateDTO.TipoProrrateoCodigo,
            EDTchCodigo = entregableCreateDTO.EDTchCodigo
        };

        await entregableRepository.AddEntregable(nuevoEntregable);
        // Traer el entregable creado
        var entregableConRelaciones = await entregableRepository.GetEntregableById(nuevoEntregable.ENTRinID);
        if (entregableConRelaciones == null)
        {
            throw new Exception(Mensajes.Entregable.ErrorObtener);
        }
        return new EntregableDTO
        {
            Id = entregableConRelaciones.ENTRinID,
            Codigo = entregableConRelaciones.ENTRchCodigo,
            PctContrato = entregableConRelaciones.ENTRdePctContrato,
            FechaInicialPlan = entregableConRelaciones.ENTRdaFechaInicialPLAN,
            DuracionPlanDias = entregableConRelaciones.ENTRinDuracionPlanDias,
            FechaInicialReal = entregableConRelaciones.ENTRdaFechaInicialREAL,
            DuracionRealDias = entregableConRelaciones.ENTRinDuracionRealDias,
            ContratoCodigo = entregableConRelaciones.Contrato.CONTchCodigo,
            ProyectoCodigo = entregableConRelaciones.Contrato.Proyecto.PROYchCodigo,
            TipoEntregableCodigo = entregableConRelaciones.TENTchCodigo,
            TipoProrrateoCodigo = entregableConRelaciones.TPROchCodigo,
            EDTchCodigo = entregableConRelaciones.EDTchCodigo,
        };
    }

    public async Task<EntregableDTO?> UpdateEntregable(int id, EntregableDTO entregableDTO)
    {
        var entregable = await entregableRepository.GetEntregableById(id);
        if (entregable == null) return null;

        entregable.ENTRchCodigo = entregableDTO.Codigo;
        entregable.ENTRdePctContrato = entregableDTO.PctContrato;
        entregable.ENTRdaFechaInicialPLAN = entregableDTO.FechaInicialPlan;
        entregable.ENTRinDuracionPlanDias = entregableDTO.DuracionPlanDias;
        entregable.ENTRdaFechaInicialREAL = entregableDTO.FechaInicialReal;
        entregable.ENTRinDuracionRealDias = entregableDTO.DuracionRealDias;
        entregable.CONTinID = entregableDTO.Id;
        entregable.TENTchCodigo = entregableDTO.TipoEntregableCodigo;
        entregable.TPROchCodigo = entregableDTO.TipoProrrateoCodigo;
        entregable.EDTchCodigo = entregableDTO.EDTchCodigo;
        await entregableRepository.UpdateEntregable(entregable);

        return new EntregableDTO
        {
            Id = entregable.ENTRinID,
            Codigo = entregable.ENTRchCodigo,
            PctContrato = entregable.ENTRdePctContrato,
            FechaInicialPlan = entregable.ENTRdaFechaInicialPLAN,
            DuracionPlanDias = entregable.ENTRinDuracionPlanDias,
            FechaInicialReal = entregable.ENTRdaFechaInicialREAL,
            DuracionRealDias = entregable.ENTRinDuracionRealDias,
            ContratoCodigo = entregable.Contrato.CONTchCodigo,
            ProyectoCodigo = entregable.Contrato.Proyecto.PROYchCodigo,
            TipoEntregableCodigo = entregable.TENTchCodigo,
            TipoProrrateoCodigo = entregable.TPROchCodigo,
            EDTchCodigo = entregable.EDTchCodigo,
        };

    }

    public async Task<bool> DeleteEntregable(int id)
    {
        return await entregableRepository.DeleteEntregable(id);
    }

}
