namespace CFA.Entities.DTOs.Telefono;

public class TelefonoReadDto : TelefonoCreateDto
{
    public long IdTelefono { get; set; }
    public DateTime FechaRegistro { get; set; }
}