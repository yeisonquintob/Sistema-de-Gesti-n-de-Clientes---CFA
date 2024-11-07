using Microsoft.AspNetCore.Mvc;
using CFA.API.Controllers.Base;
using CFA.API.Models.Response;
using CFA.Business.Services.Interfaces;
using CFA.Entities.DTOs.Direccion;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFA.API.Controllers.v1;

[ApiController]
[Route("api/v1/clientes/{codigoCliente}/direcciones")]
public class DireccionController : BaseApiController
{
    private readonly IDireccionService _direccionService;
    private readonly ILogger<DireccionController> _logger;

    public DireccionController(
        IDireccionService direccionService,
        ILogger<DireccionController> logger)
    {
        _direccionService = direccionService ?? throw new ArgumentNullException(nameof(direccionService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene todas las direcciones de un cliente
    /// </summary>
    /// <param name="codigoCliente">Código del cliente</param>
    /// <returns>Lista de direcciones del cliente</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<DireccionReadDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCliente(long codigoCliente)
    {
        try
        {
            _logger.LogInformation("Obteniendo direcciones del cliente {CodigoCliente}", codigoCliente);
            var direcciones = await _direccionService.GetByClienteAsync(codigoCliente);
            return HandleResponse(direcciones, "Direcciones obtenidas exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener direcciones del cliente {CodigoCliente}", codigoCliente);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Obtiene una dirección específica
    /// </summary>
    /// <param name="codigoCliente">Código del cliente</param>
    /// <param name="idDireccion">ID de la dirección</param>
    [HttpGet("{idDireccion}")]
    [ProducesResponseType(typeof(BaseResponse<DireccionReadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long codigoCliente, long idDireccion)
    {
        try
        {
            _logger.LogInformation("Obteniendo dirección {IdDireccion} del cliente {CodigoCliente}", 
                idDireccion, codigoCliente);
            var direccion = await _direccionService.GetByIdAsync(idDireccion);
            return HandleResponse(direccion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener dirección {IdDireccion}", idDireccion);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Crea una nueva dirección para un cliente
    /// </summary>
    /// <param name="codigoCliente">Código del cliente</param>
    /// <param name="direccionDto">Datos de la dirección</param>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<DireccionReadDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(long codigoCliente, [FromBody] DireccionCreateDto direccionDto)
    {
        try
        {
            _logger.LogInformation("Creando dirección para cliente {CodigoCliente}", codigoCliente);
            
            // Validación de formato de dirección
            if (!await _direccionService.ValidateDireccionFormatoAsync(direccionDto.Direccion))
            {
                return BadRequest(new ApiErrorResponse 
                { 
                    Success = false,
                    Message = "Formato de dirección inválido",
                    Errors = new[] { "La dirección no cumple con el formato requerido" }
                });
            }

            var direccion = await _direccionService.CreateAsync(direccionDto);

            return CreatedAtAction(
                nameof(GetById), 
                new { codigoCliente, idDireccion = direccion.IdDireccion },
                new BaseResponse<DireccionReadDto>
                {
                    Success = true,
                    Message = "Dirección creada exitosamente",
                    Data = direccion
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear dirección para cliente {CodigoCliente}", codigoCliente);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Actualiza una dirección existente
    /// </summary>
    /// <param name="codigoCliente">Código del cliente</param>
    /// <param name="idDireccion">ID de la dirección</param>
    /// <param name="direccionDto">Datos actualizados de la dirección</param>
    [HttpPut("{idDireccion}")]
    [ProducesResponseType(typeof(BaseResponse<DireccionReadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long codigoCliente, long idDireccion, [FromBody] DireccionUpdateDto direccionDto)
    {
        try
        {
            if (idDireccion != direccionDto.IdDireccion)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Success = false,
                    Message = "El ID de la dirección en la ruta no coincide con el ID en los datos",
                    Errors = new[] { "ID de dirección inconsistente" }
                });
            }

            if (codigoCliente != direccionDto.CodigoCliente)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Success = false,
                    Message = "El código del cliente en la ruta no coincide con el código en los datos",
                    Errors = new[] { "Código de cliente inconsistente" }
                });
            }

            _logger.LogInformation("Actualizando dirección {IdDireccion} del cliente {CodigoCliente}", 
                idDireccion, codigoCliente);
                
            // Validación de formato de dirección
            if (!await _direccionService.ValidateDireccionFormatoAsync(direccionDto.Direccion))
            {
                return BadRequest(new ApiErrorResponse 
                { 
                    Success = false,
                    Message = "Formato de dirección inválido",
                    Errors = new[] { "La dirección no cumple con el formato requerido" }
                });
            }

            var direccion = await _direccionService.UpdateAsync(idDireccion, direccionDto);
            return HandleResponse(direccion, "Dirección actualizada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar dirección {IdDireccion}", idDireccion);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Elimina una dirección
    /// </summary>
    /// <param name="codigoCliente">Código del cliente</param>
    /// <param name="idDireccion">ID de la dirección</param>
    [HttpDelete("{idDireccion}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long codigoCliente, long idDireccion)
    {
        try
        {
            _logger.LogInformation("Eliminando dirección {IdDireccion} del cliente {CodigoCliente}", 
                idDireccion, codigoCliente);
            
            // Validar que no sea la única dirección antes de eliminar
            await _direccionService.ValidateClientHasMinimumAddressAsync(codigoCliente);
            
            await _direccionService.DeleteAsync(idDireccion);
            return HandleResponse(true, "Dirección eliminada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar dirección {IdDireccion}", idDireccion);
            return HandleError(ex);
        }
    }
}