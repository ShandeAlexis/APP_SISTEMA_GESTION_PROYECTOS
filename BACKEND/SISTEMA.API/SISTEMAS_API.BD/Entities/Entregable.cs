using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;
[Table("ENTREGABLE")]
public class Entregable
{
    [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ENTRinID { get; set; }

    [Required]
    [MaxLength(50)]
    public string ENTRchNombre { get; set; } = string.Empty;
}
