using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

[Table("COMENTARIO_SEMANAL")]
public class ComentarioSemanal
{
    [Key]
    public int COMEinID { get; set; }

    [Required]
    public int PROYinID { get; set; }
    [ForeignKey(nameof(PROYinID))]
    public Proyecto Proyecto { get; set; } = null!;

    [Required]
    public int COMEinAño { get; set; }

    [Required]
    public int COMEinSemana { get; set; }

    public string? COMEchDescripcion { get; set; }
}