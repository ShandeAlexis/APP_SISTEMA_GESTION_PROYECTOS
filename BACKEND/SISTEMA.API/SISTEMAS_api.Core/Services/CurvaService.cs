using System;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models.Curva;
using SISTEMA.API.SISTEMAS_api.Core.Models.Entregable;
using SISTEMA.API.SISTEMAS_API.BD.Entities;
using SISTEMA.API.SISTEMAS_API.BD.Repositories;

namespace SISTEMA.API.SISTEMAS_api.Core.Services;

public class CurvaService : ICurvaService
{
    private readonly ICurvaRepository curvaRepository;

    public CurvaService(ICurvaRepository repo)
    {
        curvaRepository = repo;
    }

    public async Task<IEnumerable<CurvaMensualDTO>?> GetCurvaAsync(int curvaId)
    {
        var curva = await curvaRepository.GetCurvaById(curvaId);

        if (curva == null)
            return null;

        if (curva.Detalles == null || !curva.Detalles.Any())
            return Enumerable.Empty<CurvaMensualDTO>();

        return curva.Detalles
            .OrderBy(d => d.DCURinPos)
            .Select(detalle => new CurvaMensualDTO
            {
                Fecha = detalle.DCURdaFecha,
                Valor = detalle.DCURreValor,
                ValorAcumulado = detalle.DCURreValorAcum
            })
            .ToList();
    }

    public async Task<bool> UpdateCurvaAsync(int curvaId, IEnumerable<CurvaMensualDTO> nuevosDetalles)
    {
        var curva = await curvaRepository.GetCurvaById(curvaId);
        if (curva == null)
            return false;

        curva.Detalles.Clear();

        int pos = 1;
        foreach (var dto in nuevosDetalles)
        {
            curva.Detalles.Add(new DetalleCurva
            {
                CURVinID = curva.CURVinID,
                DCURdaFecha = dto.Fecha,
                DCURreValor = dto.Valor,
                DCURreValorAcum = dto.ValorAcumulado,
                DCURinPos = pos++
            });
        }

        await curvaRepository.UpdateCurva(curva);
        return true;
    }

    public async Task<bool> DeleteCurvaAsync(int curvaId)
    {
        var curva = await curvaRepository.GetCurvaById(curvaId);
        if (curva == null)
            return false; // no existe

        await curvaRepository.DeleteCurva(curva);
        return true;
    }
    public async Task<IEnumerable<CurvaDTO>> GetCurvasByEntregableId(int entregableId)
    {
        var curvas = await curvaRepository.GetCurvasByEntregableId(entregableId);

        return curvas.Select(c => new CurvaDTO
        {
            Id = c.CURVinID,
            Origen = c.CURVchOrigen,
            OrigenId = c.CURVinIDOrigen,
            FechaInicial = c.CURVdaFechaInicial,
            FechaFinal = c.CURVdaFechaFin,
            TipoCurvaCodigo = c.TCURVchCodigo,
            Detalles = c.Detalles.Select(d => new DetalleCurvaUpdateDTO
            {
                Id = d.DCURinID,
                Fecha = d.DCURdaFecha,
                Valor = d.DCURreValor,
                ValorAcumulado = d.DCURreValorAcum,
                Posicion = d.DCURinPos
            }).ToList()
        });
    }


}


// public async Task<IEnumerable<CurvaMensualDTO>> GenerarCurvaAsync(int entregableId, string tipoCurvaCodigo)
// {
//     if (tipoCurvaCodigo != "CRV_PLAN_VIG" && tipoCurvaCodigo != "CRV_REAL_VIG")
//         throw new Exception("Tipo de curva no válido.");

//     var entregable = await entregableRepository.GetEntregableById(entregableId);
//     if (entregable == null)
//         throw new Exception("Entregable no encontrado.");

//     var fechaInicio = tipoCurvaCodigo == "CRV_PLAN_VIG"
//         ? entregable.ENTRdaFechaInicialPLAN
//         : entregable.ENTRdaFechaInicialREAL;

//     var fechaFin = tipoCurvaCodigo == "CRV_PLAN_VIG"
//         ? entregable.ENTRdaFechaInicialPLAN.AddDays(entregable.ENTRinDuracionPlanDias - 1)
//         : entregable.ENTRdaFechaInicialREAL.AddDays(entregable.ENTRinDuracionRealDias - 1);

//     var curva = new Curva
//     {
//         CURVchOrigen = "ENTREGABLE",
//         CURVinIDOrigen = entregableId,
//         CURVdaFechaInicial = fechaInicio,
//         CURVdaFechaFin = fechaFin,
//         TCURVchCodigo = tipoCurvaCodigo
//     };

//     await entregableRepository.AddCurva(curva);

//     var meses = new List<(int anio, int mes)>();
//     var fechaTmp = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);
//     while (fechaTmp <= fechaFin)
//     {
//         meses.Add((fechaTmp.Year, fechaTmp.Month));
//         fechaTmp = fechaTmp.AddMonths(1);
//     }

//     decimal valorMensual = tipoCurvaCodigo == "CRV_PLAN_VIG" ? 100m / meses.Count : 0m;
//     decimal acumulado = 0;
//     int pos = 1;
//     var detalles = new List<DetalleCurva>();

//     foreach (var (anio, mes) in meses)
//     {
//         acumulado += valorMensual;
//         detalles.Add(new DetalleCurva
//         {
//             CURVinID = curva.CURVinID,
//             DCURdaFecha = new DateTime(anio, mes, 1),
//             DCURreValor = valorMensual,
//             DCURreValorAcum = acumulado,
//             DCURinPos = pos++
//         });
//     }

//     await entregableRepository.AddDetallesCurva(detalles);

//     return detalles.Select(d => new CurvaMensualDTO
//     {
//         Fecha = d.DCURdaFecha,
//         Valor = d.DCURreValor,
//         ValorAcumulado = d.DCURreValorAcum
//     }).ToList();

// }