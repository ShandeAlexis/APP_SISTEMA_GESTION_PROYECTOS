using System;

namespace SISTEMA.API.SISTEMAS_api.Core.Models.Incidencia;

public class IncidenciaDTO
{
    public int Id { get; set; } 
   public string CodigoProyecto { get; set; } = string.Empty;
    public string Nota { get; set; } = string.Empty;
    public string Nivel { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string? Responsable { get; set; }
    public string? Categoria { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaResolucion { get; set; }
}
