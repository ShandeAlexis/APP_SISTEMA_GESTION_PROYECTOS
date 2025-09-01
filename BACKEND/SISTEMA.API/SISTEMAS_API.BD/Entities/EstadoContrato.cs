using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SISTEMA.API.SISTEMAS_API.BD.Entities;

[Table("ESTADO_CONTRATO")]
public class EstadoContrato
{
    [Key]
    [MaxLength(20)]
    public string ECONchCodigo { get; set; } = string.Empty;

    public string? ECONchDescripcion { get; set; }

    public ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
}