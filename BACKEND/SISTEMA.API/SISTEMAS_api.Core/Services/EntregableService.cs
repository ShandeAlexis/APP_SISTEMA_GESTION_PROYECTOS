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
    public async Task<IEnumerable<EntregableDTO>> GetEntregablesByContratoId(int contratoId)
    {
        var entregables = await entregableRepository.GetEntregablesByContratoId(contratoId);

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
            TipoEntregableCodigo = e.TENTchCodigo,
            TipoProrrateoCodigo = e.TPROchCodigo,
            EDTchCodigo = e.EDTchCodigo
        });
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

    public async Task<EntregableDTO?> UpdateEntregable(int id, EntregableCreateDTO createDTO)
    {
        var entregable = await entregableRepository.GetEntregableById(id);
        if (entregable == null)
        {
            throw new Exception(Mensajes.Entregable.ErrorObtener);
        }
        entregable.ENTRchCodigo = createDTO.Codigo;
        entregable.ENTRdePctContrato = createDTO.PctContrato;
        entregable.ENTRdaFechaInicialPLAN = createDTO.FechaInicialPlan;
        entregable.ENTRinDuracionPlanDias = createDTO.DuracionPlanDias;
        entregable.ENTRdaFechaInicialREAL = createDTO.FechaInicialReal;
        entregable.ENTRinDuracionRealDias = createDTO.DuracionRealDias;
        entregable.TENTchCodigo = createDTO.TipoEntregableCodigo;
        entregable.TPROchCodigo = createDTO.TipoProrrateoCodigo;
        entregable.EDTchCodigo = createDTO.EDTchCodigo;

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



    public async Task<IEnumerable<CurvaMensualDTO>> CalcularCurvaMensualAsync(int entregableId, string tipoCurvaCodigo)
    {
        // ✅ Validar tipos de curva permitidos
        if (tipoCurvaCodigo != "CRV_PLAN_VIG" && tipoCurvaCodigo != "CRV_REAL_VIG")
            throw new Exception("Solo se permiten los tipos de curva: CRV_PLAN_VIG o CRV_REAL_VIG.");

        var entregable = await entregableRepository.GetEntregableById(entregableId);
        if (entregable == null)
            throw new Exception(Mensajes.Entregable.NoEncontrado);

        // Definir fechas de la curva según tipo
        var fechaInicio = tipoCurvaCodigo == "CRV_PLAN_VIG"
            ? entregable.ENTRdaFechaInicialPLAN
            : entregable.ENTRdaFechaInicialREAL;

        var fechaFin = tipoCurvaCodigo == "CRV_PLAN_VIG"
            ? entregable.ENTRdaFechaInicialPLAN.AddDays(entregable.ENTRinDuracionPlanDias - 1)
            : entregable.ENTRdaFechaInicialREAL.AddDays(entregable.ENTRinDuracionRealDias - 1);

        // Crear curva
        var curva = new Curva
        {
            CURVchOrigen = "ENTREGABLE",
            CURVinIDOrigen = entregableId,
            CURVdaFechaInicial = fechaInicio,
            CURVdaFechaFin = fechaFin,
            TCURVchCodigo = tipoCurvaCodigo
        };

        // Guardar la curva (para obtener CURVinID)
        await entregableRepository.AddCurva(curva);

        // Armar los meses que cubre la curva
        var meses = new List<(int anio, int mes)>();
        var fechaTmp = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);

        while (fechaTmp <= fechaFin)
        {
            meses.Add((fechaTmp.Year, fechaTmp.Month));
            fechaTmp = fechaTmp.AddMonths(1);
        }

        var detalles = new List<DetalleCurva>();
        decimal valorMensual = 0;
        decimal acumulado = 0;
        int pos = 1;

        if (tipoCurvaCodigo == "CRV_PLAN_VIG")
        {
            // 🔹 Reparto en 100%
            valorMensual = 100m / meses.Count;

            foreach (var (anio, mes) in meses)
            {
                acumulado += valorMensual;

                detalles.Add(new DetalleCurva
                {
                    CURVinID = curva.CURVinID,
                    DCURdaFecha = new DateTime(anio, mes, 1),
                    DCURreValor = valorMensual,
                    DCURreValorAcum = acumulado,
                    DCURinPos = pos++
                });
            }
        }
        else if (tipoCurvaCodigo == "CRV_REAL_VIG")
        {
            // 🔹 Solo se crean meses con valores en 0
            foreach (var (anio, mes) in meses)
            {
                detalles.Add(new DetalleCurva
                {
                    CURVinID = curva.CURVinID,
                    DCURdaFecha = new DateTime(anio, mes, 1),
                    DCURreValor = 0,
                    DCURreValorAcum = 0,
                    DCURinPos = pos++
                });
            }
        }

        // Guardar detalles
        await entregableRepository.AddDetallesCurva(detalles);

        // Devolver DTOs
        return detalles.Select(d => new CurvaMensualDTO
        {
            Fecha = d.DCURdaFecha,
            Valor = d.DCURreValor,
            ValorAcumulado = d.DCURreValorAcum
        });
    }



}
