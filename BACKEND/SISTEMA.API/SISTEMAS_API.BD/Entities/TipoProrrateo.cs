using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

[Table("TIPO_PRORRATEO")]
public class TipoProrrateo
{
    [Key]
    [MaxLength(10)]
    public string TPROchCodigo { get; set; } = string.Empty;

    public string? TPROchDescripcion { get; set; }

    public ICollection<Entregable> Entregables { get; set; } = new List<Entregable>();
}