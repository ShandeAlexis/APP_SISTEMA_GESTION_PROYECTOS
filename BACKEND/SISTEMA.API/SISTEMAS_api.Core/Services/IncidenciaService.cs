using System;
using SISTEMA.API.SISTEMAS_api.Core.Models.Incidencia;
using SISTEMA.API.SISTEMAS_API.BD.Entities;
using SISTEMA.API.SISTEMAS_API.BD.Repositories;

namespace SISTEMA.API.SISTEMAS_api.Core.Interfaces;

public class IncidenciaService : IIncidenciaService
{
    private readonly IIncidenciaRepository incidenciaRepository;

    public IncidenciaService(IIncidenciaRepository repo)
    {
        incidenciaRepository = repo;
    }

    public async Task<IEnumerable<IncidenciaDTO>> GetIncidencias()
    {
        var incidencias = await incidenciaRepository.GetAllIncidencias();
        return incidencias.Select(i => new IncidenciaDTO
        {
            Id = i.INCIinID,
            CodigoProyecto = i.Proyecto.PROYchCodigo,
            Nota = i.INCIchNota,
            Nivel = i.INCIchNivel,
            Estado = i.INCIchEstado,
            Responsable = i.INCIchResponsable,
            Categoria = i.INCIchCategoria,
            FechaRegistro = i.INCIdaFechaRegistro,
            FechaResolucion = i.INCIdaFechaResolucion
        });
    }

    public async Task<IncidenciaDTO?> GetIncidencia(int id)
    {
        var i = await incidenciaRepository.GetIncidenciaById(id);
        if (i == null) return null;

        return new IncidenciaDTO
        {   
            Id = i.INCIinID,
            CodigoProyecto = i.Proyecto.PROYchCodigo,
            Nota = i.INCIchNota,
            Nivel = i.INCIchNivel,
            Estado = i.INCIchEstado,
            Responsable = i.INCIchResponsable,
            Categoria = i.INCIchCategoria,
            FechaRegistro = i.INCIdaFechaRegistro,
            FechaResolucion = i.INCIdaFechaResolucion
        };
    }


    public async Task<IncidenciaDTO> CreateIncidencia(IncidenciaCreateDTO dto)
    {
        var i = new Incidencia
        {
            PROYinID = dto.ProyectoId,
            INCIchNota = dto.Nota,
            INCIchNivel = dto.Nivel,
            INCIchEstado = dto.Estado,
            INCIchResponsable = dto.Responsable,
            INCIchCategoria = dto.Categoria,
            INCIdaFechaRegistro = DateTime.Now,
            INCIdaFechaResolucion = dto.FechaResolucion
        };

        await incidenciaRepository.AddIncidencia(i);

        var incidenciaCreada = await incidenciaRepository.GetIncidenciaById(i.INCIinID);

        return new IncidenciaDTO
        {
            Id = incidenciaCreada!.INCIinID,
            CodigoProyecto = incidenciaCreada!.Proyecto.PROYchCodigo,
            Nota = incidenciaCreada.INCIchNota,
            Nivel = incidenciaCreada.INCIchNivel,
            Estado = incidenciaCreada.INCIchEstado,
            Responsable = incidenciaCreada.INCIchResponsable,
            Categoria = incidenciaCreada.INCIchCategoria,
            FechaRegistro = incidenciaCreada.INCIdaFechaRegistro,
            FechaResolucion = incidenciaCreada.INCIdaFechaResolucion
        };
    }


    public async Task<IncidenciaDTO?> UpdateIncidencia(int id, IncidenciaCreateDTO dto)
    {
        var i = await incidenciaRepository.GetIncidenciaById(id);
        if (i == null) return null;

        i.INCIchNota = dto.Nota;
        i.INCIchNivel = dto.Nivel;
        i.INCIchEstado = dto.Estado;
        i.INCIchResponsable = dto.Responsable;
        i.INCIchCategoria = dto.Categoria;
        i.INCIdaFechaResolucion = dto.FechaResolucion;

        await incidenciaRepository.UpdateIncidencia(i);

        return new IncidenciaDTO
        {
            Id = i.INCIinID,
            CodigoProyecto = i.Proyecto.PROYchCodigo,
            Nota = i.INCIchNota,
            Nivel = i.INCIchNivel,
            Estado = i.INCIchEstado,
            Responsable = i.INCIchResponsable,
            Categoria = i.INCIchCategoria,
            FechaRegistro = i.INCIdaFechaRegistro,
            FechaResolucion = i.INCIdaFechaResolucion
        };
    }


    public async Task<bool> DeleteIncidencia(int id)
    {
        return await incidenciaRepository.DeleteIncidencia(id);
    }
}
