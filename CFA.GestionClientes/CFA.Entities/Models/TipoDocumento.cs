using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFA.Entities.Models;

public class TipoDocumento
{
    [Key]
    [StringLength(2)]
    public string IdTipoDocumento { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Descripcion { get; set; } = null!;

    // Navegación
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}