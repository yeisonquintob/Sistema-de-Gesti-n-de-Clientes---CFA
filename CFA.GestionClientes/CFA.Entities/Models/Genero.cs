using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFA.Entities.Models;
public class Genero
{
    [Key]
    [StringLength(1)]
    public string IdGenero { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string Descripcion { get; set; } = null!;

    // Navegación
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}