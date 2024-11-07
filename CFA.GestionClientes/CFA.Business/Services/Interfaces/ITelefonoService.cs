
using CFA.Entities.DTOs.Telefono;

namespace CFA.Business.Services.Interfaces;

public interface ITelefonoService
{
    Task<TelefonoReadDto> CreateAsync(TelefonoCreateDto telefonoDto);
    Task<TelefonoReadDto> UpdateAsync(long idTelefono, TelefonoUpdateDto telefonoDto);
    Task DeleteAsync(long idTelefono);
    Task<TelefonoReadDto?> GetByIdAsync(long idTelefono);
    Task<IEnumerable<TelefonoReadDto>> GetByClienteAsync(long codigoCliente);
    Task ValidateClientHasMinimumPhoneAsync(long codigoCliente);
    Task<bool> ValidatePhoneFormatAsync(string telefono);
}