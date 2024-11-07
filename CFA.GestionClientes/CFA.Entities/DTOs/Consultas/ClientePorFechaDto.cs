namespace CFA.Entities.DTOs.Consultas;

public class ClientePorFechaDto
{
    public DateTime FechaNacimiento { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public int Edad { get; set; }
}