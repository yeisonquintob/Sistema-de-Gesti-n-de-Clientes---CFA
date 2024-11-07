
using CFA.Entities.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace CFA.Entities.DTOs.Cliente;

public class ClienteUpdateDto : ClienteBaseDto
{
    [Required(ErrorMessage = "El código del cliente es obligatorio")]
    public long Codigo { get; set; }
}