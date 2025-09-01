using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

[Table("TIPO_ENTREGABLE")]
public class TipoEntregable
{
    [Key]
    [MaxLength(10)]
    public string TENTchCodigo { get; set; } = string.Empty;

    public string? TENTchDescripcion { get; set; }

    public ICollection<Entregable> Entregables { get; set; } = new List<Entregable>();
}