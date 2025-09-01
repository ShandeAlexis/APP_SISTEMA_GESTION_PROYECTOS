using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

 [Table("CURVA")]
    public class Curva
    {
        [Key]
        public int CURVinID { get; set; }

        [Required]
        [MaxLength(20)]
        public string CURVchOrigen { get; set; } = string.Empty;

        [Required]
        public int CURVinIDOrigen { get; set; }

        [Required]
        public DateTime CURVdaFechaInicial { get; set; }

        [Required]
        public DateTime CURVdaFechaFin { get; set; }

        [Required]
        [MaxLength(50)]
        public string TCURVchCodigo { get; set; } = string.Empty;

        [ForeignKey(nameof(TCURVchCodigo))]
        public TipoCurva TipoCurva { get; set; } = null!;

        public ICollection<DetalleCurva> Detalles { get; set; } = new List<DetalleCurva>();
    }
