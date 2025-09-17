using System;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

public class DetalleReporteDTO
{
    public string? CodigoEDT { get; set; }
    public string? ContratoCodigo { get; set; }
    public decimal Capex { get; set; }
    public decimal Estimado { get; set; }

    public DateTime? FechaIniPlan { get; set; }
    public DateTime? FechaFinPlan { get; set; }
    public DateTime? FechaIniReal { get; set; }
    public DateTime? FechaFinReal { get; set; }

    public decimal FisPlan { get; set; }
    public decimal FisReal { get; set; }
    public decimal FisIndCum { get; set; }

    public decimal EcoPlan { get; set; }
    public decimal EcoReal { get; set; }
    public decimal EcoIndCum { get; set; }
}


