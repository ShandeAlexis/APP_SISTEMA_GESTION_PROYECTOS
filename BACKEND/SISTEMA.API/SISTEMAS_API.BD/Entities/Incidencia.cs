using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;
[Table("INCIDENCIAS")]
public class Incidencia
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int INCIinID { get; set; }

    [Required]
    public int PROYinID { get; set; }

    [ForeignKey(nameof(PROYinID))]
    public Proyecto Proyecto { get; set; } = null!;

    [Required, StringLength(255)]
    public string INCIchNota { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string INCIchNivel { get; set; } = "BAJO"; // BAJO, MEDIO, ALTO, CRITICO

    [Required, StringLength(20)]
    public string INCIchEstado { get; set; } = "ABIERTA"; // ABIERTA, EN_PROCESO, CERRADA

    [StringLength(100)]
    public string? INCIchResponsable { get; set; }

    [StringLength(50)]
    public string? INCIchCategoria { get; set; }

    [Required]
    public DateTime INCIdaFechaRegistro { get; set; } = DateTime.Now;

    public DateTime? INCIdaFechaResolucion { get; set; }
}