using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;


[Table("EDT")]
public class EDT
{
    [Key]
    [MaxLength(10)]
    public string EDTchCodigo { get; set; } = string.Empty;

    public string? EDTchDescripcion { get; set; }

    public ICollection<Entregable> Entregables { get; set; } = new List<Entregable>();
}