using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

 [Table("TIPO_CURVA")]
    public class TipoCurva
    {
        [Key]
        [MaxLength(50)]
        public string TCURVchCodigo { get; set; } = string.Empty;

        public string? TCURVchDescripcion { get; set; }

        public ICollection<Curva> Curvas { get; set; } = new List<Curva>();
    }
