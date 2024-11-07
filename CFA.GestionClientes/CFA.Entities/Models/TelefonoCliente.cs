using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFA.Entities.Models;

public class TelefonoCliente
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long IdTelefono { get; set; }

    [Required]
    public long CodigoCliente { get; set; }

    [Required]
    [StringLength(15)]
    public string NumeroTelefono { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string TipoTelefono { get; set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime FechaRegistro { get; set; }

    // Navegación
    [ForeignKey(nameof(CodigoCliente))]
    public virtual Cliente Cliente { get; set; } = null!;
}