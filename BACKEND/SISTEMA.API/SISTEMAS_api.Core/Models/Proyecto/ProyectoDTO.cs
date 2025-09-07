namespace SISTEMA.API.SISTEMAS_api.Core.Models.Proyecto;

public class ProyectoDTO
{
    public int Id { get; set; }
    public string Codigo { get; set; } = null!;
    public string? Descripcion { get; set; } = null!;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal? Capex { get; set; }
    public decimal? Estimado { get; set; }
    public string CodigoEstado { get; set; } = null!;
    public string CodigoEmpresa { get; set; } = null!;
}
