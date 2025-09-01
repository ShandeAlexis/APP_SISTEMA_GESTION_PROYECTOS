using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

[Table("CONTRATO")]
public class Contrato
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CONTinID { get; set; }

    [Required]
    [MaxLength(50)]
    public string CONTchCodigo { get; set; } = string.Empty;

    [Required]
    public int PROYinID { get; set; }
    [ForeignKey(nameof(PROYinID))]
    public Proyecto Proyecto { get; set; } = null!;

    [Column(TypeName = "decimal(15,2)")]
    public decimal? CONTdeCapex { get; set; }

    [Column(TypeName = "decimal(15,2)")]
    public decimal? CONTdeCosto { get; set; }

    [Column(TypeName = "decimal(15,2)")]
    public decimal? CONTdeEstimado { get; set; }

    [MaxLength(150)]
    public string? CONTchContratista { get; set; }

    public string? CONTchAlcance { get; set; }

    [Required]
    [MaxLength(20)]
    public string ECONchCodigo { get; set; } = string.Empty;

    [ForeignKey(nameof(ECONchCodigo))]
    public EstadoContrato Estado { get; set; } = null!;

    public string? CONTchObjetivo { get; set; }

    [Required]
    public DateTime CONTdaFechaInicial { get; set; }

    [Required]
    public DateTime CONTdaFechaFinal { get; set; }

    [MaxLength(50)]
    public string? CONTchCodigoContrato { get; set; }

    public ICollection<Entregable> Entregables { get; set; } = new List<Entregable>();
}