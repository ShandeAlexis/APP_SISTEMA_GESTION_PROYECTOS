using System;
using SISTEMA.API.SISTEMAS_api.Core.Constantes;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_api.Core.Models.Proyecto;
using SISTEMA.API.SISTEMAS_API.BD.Entities;
using SISTEMA.API.SISTEMAS_API.BD.Repositories;

namespace SISTEMA.API.SISTEMAS_api.Core.Services;

public class ProyectoService : IProyectoService
{
    private readonly IProyectoRepository proyectoRepository;

    public ProyectoService(IProyectoRepository proyectoRepo)
    {
        proyectoRepository = proyectoRepo;
    }

    public async Task<IEnumerable<ProyectoDTO>> GetProyectos()
    {
        var proyectos = await proyectoRepository.GetAllProyectos();
        return proyectos.Select(p => new ProyectoDTO
        {
            Id = p.PROYinID,
            Codigo = p.PROYchCodigo,
            Descripcion = p.PROYchDescripcion,
            FechaInicio = p.PROYdaFechaInicial,
            FechaFin = p.PROYdaFechaFinal,
            Capex = p.PROYdeCapex,
            Estimado = p.PROYdeEstimado,
            CodigoEstado = p.EPROchCodigo,
            CodigoEmpresa = p.EMPRchCodigo
        });
    }
    public async Task<ProyectoDTO?> GetProyecto(int id)
    {
        var proyecto = await proyectoRepository.GetProyectoById(id);
        if (proyecto == null) return null;

        return new ProyectoDTO
        {
            Id = proyecto.PROYinID,
            Codigo = proyecto.PROYchCodigo,
            Descripcion = proyecto.PROYchDescripcion,
            FechaInicio = proyecto.PROYdaFechaInicial,
            FechaFin = proyecto.PROYdaFechaFinal,
            Capex = proyecto.PROYdeCapex,
            Estimado = proyecto.PROYdeEstimado,
            CodigoEstado = proyecto.EPROchCodigo,
            CodigoEmpresa = proyecto.EMPRchCodigo
        };
    }

    public async Task<ProyectoDTO> CreateProyecto(ProyectoCreateDTO proyectoCreateDTO)
    {
        var proyecto = new Proyecto
        {
            PROYchCodigo = proyectoCreateDTO.Codigo,
            PROYchDescripcion = proyectoCreateDTO.Descripcion,
            PROYdaFechaInicial = proyectoCreateDTO.FechaInicio,
            PROYdaFechaFinal = proyectoCreateDTO.FechaFin,
            PROYdeCapex = proyectoCreateDTO.Capex,
            PROYdeEstimado = proyectoCreateDTO.Estimado,
            EPROchCodigo = proyectoCreateDTO.CodigoEstado,
            EMPRchCodigo = proyectoCreateDTO.CodigoEmpresa
        };

        await proyectoRepository.AddProyecto(proyecto);

        var proyectoGuardado = await proyectoRepository.GetProyectoById(proyecto.PROYinID);
        if (proyectoGuardado == null) throw new Exception(Mensajes.Proyecto.ErrorObtener);

        return new ProyectoDTO
        {
            Id = proyectoGuardado.PROYinID,
            Codigo = proyectoGuardado.PROYchCodigo,
            Descripcion = proyectoGuardado.PROYchDescripcion,
            FechaInicio = proyectoGuardado.PROYdaFechaInicial,
            FechaFin = proyectoGuardado.PROYdaFechaFinal,
            Capex = proyectoGuardado.PROYdeCapex,
            Estimado = proyectoGuardado.PROYdeEstimado,
            CodigoEstado = proyectoGuardado.EPROchCodigo,
            CodigoEmpresa = proyectoGuardado.EMPRchCodigo
        };
    }

    public async Task<ProyectoDTO?> UpdateProyecto(int id, ProyectoCreateDTO proyectoCreateDTO)
    {
        var existingProyecto = await proyectoRepository.GetProyectoById(id);
        if (existingProyecto == null)
        {
            throw new Exception(Mensajes.Proyecto.NoEncontrado);
        }
        existingProyecto.PROYchCodigo = proyectoCreateDTO.Codigo;
        existingProyecto.PROYchDescripcion = proyectoCreateDTO.Descripcion;
        existingProyecto.PROYdaFechaInicial = proyectoCreateDTO.FechaInicio;
        existingProyecto.PROYdaFechaFinal = proyectoCreateDTO.FechaFin;
        existingProyecto.PROYdeCapex = proyectoCreateDTO.Capex;
        existingProyecto.PROYdeEstimado = proyectoCreateDTO.Estimado;
        existingProyecto.EPROchCodigo = proyectoCreateDTO.CodigoEstado;

        await proyectoRepository.UpdateProyecto(existingProyecto);

        return new ProyectoDTO
        {
            Id = existingProyecto.PROYinID,
            Codigo = existingProyecto.PROYchCodigo,
            Descripcion = existingProyecto.PROYchDescripcion,
            FechaInicio = existingProyecto.PROYdaFechaInicial,
            FechaFin = existingProyecto.PROYdaFechaFinal,
            Capex = existingProyecto.PROYdeCapex,
            Estimado = existingProyecto.PROYdeEstimado,
            CodigoEstado = existingProyecto.EPROchCodigo,
            CodigoEmpresa = existingProyecto.EMPRchCodigo
        };
    }
    
    public async Task<bool> DeleteProyecto(int id)
    {
        return await proyectoRepository.DeleteProyecto(id);
    }

}
