namespace CFA.Entities.DTOs.Cliente;

public class ClienteSimpleDto
{
    public long Codigo { get; set; }
    public string NumeroDocumento { get; set; } = null!;
    public string NombreCompleto { get; set; } = null!;
}