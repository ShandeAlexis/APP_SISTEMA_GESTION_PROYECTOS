using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities
{
    [Table("ENTREGABLE")]
    public class Entregable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ENTRinID { get; set; }

        [Required]
        [MaxLength(150)]
        public string ENTRchCodigo { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(3,2)")]
        public decimal ENTRdePctContrato { get; set; }

        [Required]
        public DateTime ENTRdaFechaInicialPLAN { get; set; }

        [Required]
        public int ENTRinDuracionPlanDias { get; set; }

        [Required]
        public DateTime ENTRdaFechaInicialREAL { get; set; }

        [Required]
        public int ENTRinDuracionRealDias { get; set; }

        // === Relaciones ===
        [Required]
        public int CONTinID { get; set; }
        
        [ForeignKey(nameof(CONTinID))]
        public Contrato Contrato { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string TENTchCodigo { get; set; } = string.Empty;

        [ForeignKey(nameof(TENTchCodigo))]
        public TipoEntregable TipoEntregable { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string TPROchCodigo { get; set; } = string.Empty;

        [ForeignKey(nameof(TPROchCodigo))]
        public TipoProrrateo TipoProrrateo { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string EDTchCodigo { get; set; } = string.Empty;

        [ForeignKey(nameof(EDTchCodigo))]
        public EDT EDT { get; set; } = null!;
    }
}