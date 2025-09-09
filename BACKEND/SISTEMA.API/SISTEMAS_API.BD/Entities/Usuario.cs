using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

[Table("USUARIO")]
public class Usuario
{
    [Key]
    public int USUAinID { get; set; }
    public string USUAchNombre { get; set; } = string.Empty;
    public string USUAchEmail { get; set; } = string.Empty;
    public string USUAchPassword { get; set; } = string.Empty;
}
