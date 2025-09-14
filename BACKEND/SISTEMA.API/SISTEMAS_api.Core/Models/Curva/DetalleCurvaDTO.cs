using System;

namespace SISTEMA.API.SISTEMAS_api.Core.Models.Curva;

public class CurvaDTO
{
    public int Id { get; set; }
    public string Origen { get; set; } = string.Empty;
    public int OrigenId { get; set; }
    public DateTime FechaInicial { get; set; }
    public DateTime FechaFinal { get; set; }
    public string TipoCurvaCodigo { get; set; } = string.Empty;

    public List<DetalleCurvaUpdateDTO> Detalles { get; set; } = new List<DetalleCurvaUpdateDTO>();
}
public class DetalleCurvaUpdateDTO
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Valor { get; set; }
    public decimal ValorAcumulado { get; set; }
    public int Posicion { get; set; }
}

public class CurvaUpdateDTO
{
    public int CurvaId { get; set; }
    public List<DetalleCurvaUpdateDTO> Detalles { get; set; } = new();
}



