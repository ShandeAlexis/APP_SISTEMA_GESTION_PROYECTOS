using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

[Table("EMPRESA")]
public class Empresa
{
    [Key]
    [MaxLength(50)]
    public string EMPRchCodigo { get; set; } = string.Empty;

    public string? EMPRchDescripcion { get; set; }

    public ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
}