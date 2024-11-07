using CFA.Entities.DTOs.Base;
using CFA.Entities.DTOs.Direccion;
using CFA.Entities.DTOs.Telefono;

namespace CFA.Entities.DTOs.Cliente;

public class ClienteReadDto : ClienteBaseDto
{
    public long Codigo { get; set; }
    public string TipoDocumentoDescripcion { get; set; } = null!;
    public string GeneroDescripcion { get; set; } = null!;
    public DateTime FechaRegistro { get; set; }
    public List<DireccionReadDto> Direcciones { get; set; } = new();
    public List<TelefonoReadDto> Telefonos { get; set; } = new();
}