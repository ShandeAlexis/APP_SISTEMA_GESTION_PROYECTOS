using System;

namespace SISTEMA.API.SISTEMAS_api.Core.Models.Incidencia;

public class IncidenciaCreateDTO
{
    public int ProyectoId { get; set; }
    public string Nota { get; set; } = string.Empty;
    public string Nivel { get; set; } = "BAJO";
    public string Estado { get; set; } = "ABIERTA";
    public string? Responsable { get; set; }
    public string? Categoria { get; set; }
    public DateTime? FechaResolucion { get; set; }
}
