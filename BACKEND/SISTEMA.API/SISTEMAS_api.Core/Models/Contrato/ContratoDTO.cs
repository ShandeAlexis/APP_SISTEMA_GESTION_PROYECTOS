
namespace SISTEMA.API.SISTEMAS_api.Core.Models.Contrato;

public class ContratoDTO
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string ProyectoCodigo { get; set; } = string.Empty;
    public decimal? Capex { get; set; }
    public decimal? Costo { get; set; }
    public decimal? Estimado { get; set; }
    public string? Contratista { get; set; }
    public string? Alcance { get; set; }
    public string EstadoCodigo { get; set; } = string.Empty;
    public string? Objetivo { get; set; }
    public DateTime FechaInicial { get; set; }
    public DateTime FechaFinal { get; set; }
    public string? CodigoContrato { get; set; }
}
