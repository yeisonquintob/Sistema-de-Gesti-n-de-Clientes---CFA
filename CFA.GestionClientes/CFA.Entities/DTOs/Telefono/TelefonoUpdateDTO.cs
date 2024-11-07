namespace CFA.Entities.DTOs.Telefono;

public class TelefonoUpdateDto : TelefonoCreateDto
{
    public long IdTelefono { get; set; }
    public long CodigoCliente { get; set; }
}