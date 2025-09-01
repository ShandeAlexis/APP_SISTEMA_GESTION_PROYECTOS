using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

[Table("ESTADO_PRO")]
public class EstadoProyecto
{
    [Key]
    [MaxLength(10)]
    public string EPROchCodigo { get; set; } = string.Empty;

    public string? EPROchDescripcion { get; set; }

    public ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
}
