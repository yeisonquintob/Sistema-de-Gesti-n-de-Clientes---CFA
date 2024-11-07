using System.ComponentModel.DataAnnotations;

namespace CFA.Entities.DTOs.Base;

public class ClienteBaseDto
{
    [Required(ErrorMessage = "El tipo de documento es obligatorio")]
    [StringLength(2)]
    public string TipoDocumento { get; set; } = null!;

    [Required(ErrorMessage = "El número de documento es obligatorio")]
    [StringLength(11)]
    public string NumeroDocumento { get; set; } = null!;

    [Required(ErrorMessage = "Los nombres son obligatorios")]
    [StringLength(30)]
    public string Nombres { get; set; } = null!;

    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    [StringLength(30)]
    public string Apellido1 { get; set; } = null!;

    [StringLength(30)]
    public string? Apellido2 { get; set; }

    [Required(ErrorMessage = "El género es obligatorio")]
    [StringLength(1)]
    public string Genero { get; set; } = null!;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateTime FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [StringLength(100)]
    public string Email { get; set; } = null!;
}