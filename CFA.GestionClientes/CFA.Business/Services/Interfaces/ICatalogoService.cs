using System.Collections.Generic;
using System.Threading.Tasks;
using CFA.Entities.DTOs.Catalogos;

namespace CFA.Business.Services.Interfaces;

public interface ICatalogoService
{
    // Métodos para TipoDocumento
    Task<IEnumerable<TipoDocumentoDto>> GetAllTiposDocumentoAsync();
    Task<TipoDocumentoDto?> GetTipoDocumentoByIdAsync(string id);
    Task<bool> ValidateTipoDocumentoExistsAsync(string id);

    // Métodos para Género
    Task<IEnumerable<GeneroDto>> GetAllGenerosAsync();
    Task<GeneroDto?> GetGeneroByIdAsync(string id);
    Task<bool> ValidateGeneroExistsAsync(string id);
}