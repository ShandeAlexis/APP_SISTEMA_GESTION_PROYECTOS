using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

[Table("PROYECTO")]
public class Proyecto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PROYinID { get; set; }

    [Required, StringLength(50)]
    public string PROYchCodigo { get; set; } = string.Empty;


    public string? PROYchDescripcion { get; set; }

    [Required]
    public DateTime PROYdaFechaInicial { get; set; }

    [Required]
    public DateTime PROYdaFechaFinal { get; set; }

    [Column(TypeName = "decimal(15,2)")]
    public decimal? PROYdeCapex { get; set; }

    [Column(TypeName = "decimal(15,2)")]
    public decimal? PROYdeEstimado { get; set; }

    // === Relaciones ===
    [Required]
    [MaxLength(50)]
    public string EMPRchCodigo { get; set; } = string.Empty;

    [ForeignKey(nameof(EMPRchCodigo))]
    public Empresa Empresa { get; set; } = null!;

    [Required]
    [MaxLength(10)]
    public string EPROchCodigo { get; set; } = string.Empty;

    [ForeignKey(nameof(EPROchCodigo))]
    public EstadoProyecto Estado { get; set; } = null!;

    public ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
    public ICollection<ComentarioSemanal> Comentarios { get; set; } = new List<ComentarioSemanal>();
}