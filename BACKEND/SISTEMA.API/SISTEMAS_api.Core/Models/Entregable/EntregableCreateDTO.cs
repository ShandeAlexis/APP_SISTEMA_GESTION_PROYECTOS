using System;
using System.ComponentModel.DataAnnotations;

namespace SISTEMA.API.SISTEMAS_api.Core.Models.Entregable;

public class EntregableCreateDTO
{
    [Required, MaxLength(150)]
    public string Codigo { get; set; } = string.Empty;

    [Required]
    public decimal PctContrato { get; set; }

    [Required]
    public DateTime FechaInicialPlan { get; set; }

    [Required]
    public int DuracionPlanDias { get; set; }

    [Required]
    public DateTime FechaInicialReal { get; set; }

    [Required]
    public int DuracionRealDias { get; set; }

    [Required]
    public int ContratoId { get; set; }

    [Required, MaxLength(10)]
    public string TipoEntregableCodigo { get; set; } = string.Empty;

    [Required, MaxLength(10)]
    public string TipoProrrateoCodigo { get; set; } = string.Empty;

    [Required, MaxLength(10)]
    public string EDTchCodigo { get; set; } = string.Empty;
}
