using System.ComponentModel.DataAnnotations;
namespace CFA.Entities.DTOs.Telefono;

public class TelefonoCreateDto
{
    [Required(ErrorMessage = "El número de teléfono es obligatorio")]
    [StringLength(15)]
    public string NumeroTelefono { get; set; } = null!;

    [Required(ErrorMessage = "El tipo de teléfono es obligatorio")]
    [StringLength(20)]
    public string TipoTelefono { get; set; } = null!;
}