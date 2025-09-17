using System;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

public class ReporteDTO
{
    public string ProyectoCodigo { get; set; } = string.Empty;
    public DateTime FechaCorte { get; set; }
    public List<DetalleReporteDTO> Detalles { get; set; } = new();
}
