using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

[Table("DETALLE_CURVA")]
public class DetalleCurva
{
    [Key]
    public int DCURinID { get; set; }

    [Required]
    public int CURVinID { get; set; }

    [ForeignKey(nameof(CURVinID))]
    public Curva Curva { get; set; } = null!;

    [Required]
    public DateTime DCURdaFecha { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal DCURreValor { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal DCURreValorAcum { get; set; }

    [Required]
    public int DCURinPos { get; set; }
}