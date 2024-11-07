using CFA.Entities.DTOs.Base;
using CFA.Entities.DTOs.Direccion;
using CFA.Entities.DTOs.Telefono;
using System.ComponentModel.DataAnnotations;

namespace CFA.Entities.DTOs.Cliente;

public class ClienteCreateDto : ClienteBaseDto
{
    [Required(ErrorMessage = "Debe proporcionar al menos una dirección")]
    public List<DireccionCreateDto> Direcciones { get; set; } = new();

    [Required(ErrorMessage = "Debe proporcionar al menos un teléfono")]
    public List<TelefonoCreateDto> Telefonos { get; set; } = new();
}