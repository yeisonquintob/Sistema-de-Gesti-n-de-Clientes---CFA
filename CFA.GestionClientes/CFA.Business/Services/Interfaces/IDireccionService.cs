
using CFA.Entities.DTOs.Direccion;

namespace CFA.Business.Services.Interfaces;

public interface IDireccionService
{
    Task<DireccionReadDto> CreateAsync(DireccionCreateDto direccionDto);
    Task<DireccionReadDto> UpdateAsync(long idDireccion, DireccionUpdateDto direccionDto);
    Task DeleteAsync(long idDireccion);
    Task<DireccionReadDto?> GetByIdAsync(long idDireccion);
    Task<IEnumerable<DireccionReadDto>> GetByClienteAsync(long codigoCliente);
    Task ValidateClientHasMinimumAddressAsync(long codigoCliente);
    Task<bool> ValidateDireccionFormatoAsync(string direccion);
}
