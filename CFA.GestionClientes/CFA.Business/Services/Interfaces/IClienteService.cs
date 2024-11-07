// Ruta: CFA.Business/Services/Interfaces/IClienteService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using CFA.Entities.DTOs.Cliente;
using CFA.Entities.DTOs.Consultas;

namespace CFA.Business.Services.Interfaces;

public interface IClienteService
{
    // Métodos CRUD básicos
    Task<ClienteReadDto> CreateAsync(ClienteCreateDto clienteDto);
    Task<ClienteReadDto> UpdateAsync(long codigo, ClienteUpdateDto clienteDto);
    Task DeleteAsync(long codigo);
    Task<ClienteReadDto?> GetByIdAsync(long codigo);
    Task<IEnumerable<ClienteSimpleDto>> GetAllAsync();
    
    // Métodos de búsqueda
    Task<IEnumerable<ClienteSimpleDto>> SearchByNameAsync(string nombre);
    Task<IEnumerable<ClienteSimpleDto>> SearchByDocumentAsync(string numeroDocumento);
    Task<IEnumerable<ClientePorFechaDto>> SearchByBirthDateRangeAsync(DateTime fechaInicial, DateTime fechaFinal);
    
    // Métodos de consulta especiales
    Task<IEnumerable<ClienteMultiplesTelefonosDto>> GetClientsWithMultiplePhonesAsync();
    Task<IEnumerable<ClienteMultiplesDireccionesDto>> GetClientsWithMultipleAddressesAsync();
    
    // Métodos de validación (estos deben ser públicos)
    Task<bool> ValidateDocumentAsync(string tipoDocumento, string numeroDocumento, long? codigoCliente = null);
    Task<bool> ValidateAgeForDocumentTypeAsync(string tipoDocumento, DateTime fechaNacimiento);
}