using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFA.Entities.Models;
public class Cliente
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Codigo { get; set; }

    [Required]
    [StringLength(2)]
    public string TipoDocumento { get; set; } = null!;

    [Required]
    [StringLength(11)]
    public string NumeroDocumento { get; set; } = null!;

    [Required]
    [StringLength(30)]
    public string Nombres { get; set; } = null!;

    [Required]
    [StringLength(30)]
    public string Apellido1 { get; set; } = null!;

    [StringLength(30)]
    public string? Apellido2 { get; set; }

    [Required]
    [StringLength(1)]
    public string Genero { get; set; } = null!;

    [Required]
    public DateTime FechaNacimiento { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime FechaRegistro { get; set; }

    // Navegación
    [ForeignKey(nameof(TipoDocumento))]
    public virtual TipoDocumento TipoDocumentoNavigation { get; set; } = null!;

    [ForeignKey(nameof(Genero))]
    public virtual Genero GeneroNavigation { get; set; } = null!;

    public virtual ICollection<DireccionCliente> Direcciones { get; set; } = new List<DireccionCliente>();
    public virtual ICollection<TelefonoCliente> Telefonos { get; set; } = new List<TelefonoCliente>();
}