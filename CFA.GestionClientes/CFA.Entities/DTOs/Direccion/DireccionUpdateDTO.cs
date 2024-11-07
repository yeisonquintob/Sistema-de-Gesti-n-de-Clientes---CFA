namespace CFA.Entities.DTOs.Direccion;

public class DireccionUpdateDto : DireccionCreateDto
{
    public long IdDireccion { get; set; }
    public long CodigoCliente { get; set; }
}