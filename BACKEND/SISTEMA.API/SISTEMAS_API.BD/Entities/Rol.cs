using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

[Table("ROL")]
public class Rol
{
    [Key]
    public int ROLinID { get; set; }
    public string ROLchNombre { get; set; } = string.Empty; 
}