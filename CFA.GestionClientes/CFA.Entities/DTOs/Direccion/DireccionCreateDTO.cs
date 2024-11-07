using System.ComponentModel.DataAnnotations;
namespace CFA.Entities.DTOs.Direccion;

public class DireccionCreateDto
{
    [Required(ErrorMessage = "La dirección es obligatoria")]
    [StringLength(100)]
    public string Direccion { get; set; } = null!;

    [Required(ErrorMessage = "El tipo de dirección es obligatorio")]
    [StringLength(20)]
    public string TipoDireccion { get; set; } = null!;
}