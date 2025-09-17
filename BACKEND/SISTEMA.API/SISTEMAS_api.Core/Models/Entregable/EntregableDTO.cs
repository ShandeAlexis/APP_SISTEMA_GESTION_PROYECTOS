using System;

namespace SISTEMA.API.SISTEMAS_api.Core.Models;

public class EntregableDTO
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public decimal PctContrato { get; set; }
    public DateTime ? FechaInicialPlan { get; set; }
    public int? DuracionPlanDias { get; set; }
    public DateTime ? FechaInicialReal { get; set; }
    public int? DuracionRealDias { get; set; }

    public string ContratoCodigo { get; set; } = string.Empty;
    public string ProyectoCodigo { get; set; } = string.Empty;

    public string TipoEntregableCodigo { get; set; } = string.Empty;
    public string TipoProrrateoCodigo { get; set; } = string.Empty;
    public string EDTchCodigo { get; set; } = string.Empty;
}
