namespace CFA.Entities.DTOs.Direccion;

public class DireccionReadDto : DireccionCreateDto
{
    public long IdDireccion { get; set; }
    public DateTime FechaRegistro { get; set; }
}