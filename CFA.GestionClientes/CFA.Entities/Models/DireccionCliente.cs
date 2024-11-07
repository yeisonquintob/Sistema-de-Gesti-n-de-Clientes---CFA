using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFA.Entities.Models;

public class DireccionCliente
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long IdDireccion { get; set; }

    [Required]
    public long CodigoCliente { get; set; }

    [Required]
    [StringLength(100)]
    public string Direccion { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string TipoDireccion { get; set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime FechaRegistro { get; set; }

    // Navegación
    [ForeignKey(nameof(CodigoCliente))]
    public virtual Cliente Cliente { get; set; } = null!;
}