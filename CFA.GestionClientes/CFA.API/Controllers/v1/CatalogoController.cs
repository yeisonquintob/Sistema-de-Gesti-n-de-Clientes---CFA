using Microsoft.AspNetCore.Mvc;
using CFA.API.Controllers.Base;
using CFA.API.Models.Response;
using CFA.Business.Services.Interfaces;
using CFA.Entities.DTOs.Catalogos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFA.API.Controllers.v1;

[ApiController]
[Route("api/v1/catalogos")]
public class CatalogoController : BaseApiController
{
    private readonly ICatalogoService _catalogoService;
    private readonly ILogger<CatalogoController> _logger;

    public CatalogoController(
        ICatalogoService catalogoService,
        ILogger<CatalogoController> logger)
    {
        _catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene todos los tipos de documento
    /// </summary>
    /// <returns>Lista de tipos de documento</returns>
    [HttpGet("tipos-documento")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<TipoDocumentoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTiposDocumento()
    {
        try
        {
            _logger.LogInformation("Obteniendo listado de tipos de documento");
            var tiposDocumento = await _catalogoService.GetTiposDocumentoAsync();
            return HandleResponse(tiposDocumento, "Tipos de documento obtenidos exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tipos de documento");
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Obtiene todos los géneros
    /// </summary>
    /// <returns>Lista de géneros</returns>
    [HttpGet("generos")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<GeneroDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGeneros()
    {
        try
        {
            _logger.LogInformation("Obteniendo listado de géneros");
            var generos = await _catalogoService.GetGenerosAsync();
            return HandleResponse(generos, "Géneros obtenidos exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener géneros");
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Obtiene un tipo de documento por su ID
    /// </summary>
    /// <param name="id">Identificador del tipo de documento</param>
    /// <returns>Tipo de documento</returns>
    [HttpGet("tipos-documento/{id}")]
    [ProducesResponseType(typeof(BaseResponse<TipoDocumentoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTipoDocumentoById(string id)
    {
        try
        {
            _logger.LogInformation("Obteniendo tipo de documento con ID {Id}", id);
            var tipoDocumento = await _catalogoService.GetTipoDocumentoByIdAsync(id);
            return HandleResponse(tipoDocumento, "Tipo de documento obtenido exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tipo de documento {Id}", id);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Obtiene un género por su ID
    /// </summary>
    /// <param name="id">Identificador del género</param>
    /// <returns>Género</returns>
    [HttpGet("generos/{id}")]
    [ProducesResponseType(typeof(BaseResponse<GeneroDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGeneroById(string id)
    {
        try
        {
            _logger.LogInformation("Obteniendo género con ID {Id}", id);
            var genero = await _catalogoService.GetGeneroByIdAsync(id);
            return HandleResponse(genero, "Género obtenido exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener género {Id}", id);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Valida si existe un tipo de documento
    /// </summary>
    /// <param name="id">Identificador del tipo de documento</param>
    /// <returns>True si existe, False si no existe</returns>
    [HttpGet("tipos-documento/{id}/validar")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ValidateTipoDocumento(string id)
    {
        try
        {
            _logger.LogInformation("Validando existencia de tipo de documento {Id}", id);
            var exists = await _catalogoService.ValidateTipoDocumentoExistsAsync(id);
            return HandleResponse(exists, "Validación realizada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar tipo de documento {Id}", id);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Valida si existe un género
    /// </summary>
    /// <param name="id">Identificador del género</param>
    /// <returns>True si existe, False si no existe</returns>
    [HttpGet("generos/{id}/validar")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ValidateGenero(string id)
    {
        try
        {
            _logger.LogInformation("Validando existencia de género {Id}", id);
            var exists = await _catalogoService.ValidateGeneroExistsAsync(id);
            return HandleResponse(exists, "Validación realizada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar género {Id}", id);
            return HandleError(ex);
        }
    }
}