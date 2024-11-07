using Microsoft.AspNetCore.Mvc;
using CFA.API.Controllers.Base;
using CFA.API.Models.Response;
using CFA.Business.Services.Interfaces;
using CFA.Entities.DTOs.Telefono;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFA.API.Controllers.v1;

[ApiController]
[Route("api/v1/clientes/{codigoCliente}/telefonos")]
public class TelefonoController : BaseApiController
{
    private readonly ITelefonoService _telefonoService;
    private readonly ILogger<TelefonoController> _logger;

    public TelefonoController(
        ITelefonoService telefonoService,
        ILogger<TelefonoController> logger)
    {
        _telefonoService = telefonoService ?? throw new ArgumentNullException(nameof(telefonoService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene todos los teléfonos de un cliente
    /// </summary>
    /// <param name="codigoCliente">Código del cliente</param>
    /// <returns>Lista de teléfonos del cliente</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<TelefonoReadDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCliente(long codigoCliente)
    {
        try
        {
            _logger.LogInformation("Obteniendo teléfonos del cliente {CodigoCliente}", codigoCliente);
            var telefonos = await _telefonoService.GetByClienteAsync(codigoCliente);
            return HandleResponse(telefonos, "Teléfonos obtenidos exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener teléfonos del cliente {CodigoCliente}", codigoCliente);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Obtiene un teléfono específico
    /// </summary>
    /// <param name="codigoCliente">Código del cliente</param>
    /// <param name="idTelefono">ID del teléfono</param>
    [HttpGet("{idTelefono}")]
    [ProducesResponseType(typeof(BaseResponse<TelefonoReadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long codigoCliente, long idTelefono)
    {
        try
        {
            _logger.LogInformation("Obteniendo teléfono {IdTelefono} del cliente {CodigoCliente}", 
                idTelefono, codigoCliente);
            var telefono = await _telefonoService.GetByIdAsync(idTelefono);
            return HandleResponse(telefono);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener teléfono {IdTelefono}", idTelefono);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Crea un nuevo teléfono para un cliente
    /// </summary>
    /// <param name="codigoCliente">Código del cliente</param>
    /// <param name="telefonoDto">Datos del teléfono</param>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<TelefonoReadDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(long codigoCliente, [FromBody] TelefonoCreateDto telefonoDto)
    {
        try
        {
            _logger.LogInformation("Creando teléfono para cliente {CodigoCliente}", codigoCliente);
            
            // Validación de formato de teléfono
            if (!await _telefonoService.ValidatePhoneFormatAsync(telefonoDto.NumeroTelefono))
            {
                return BadRequest(new ApiErrorResponse 
                { 
                    Success = false,
                    Message = "Formato de teléfono inválido",
                    Errors = new[] { "El número de teléfono no cumple con el formato requerido" }
                });
            }

            var telefono = await _telefonoService.CreateAsync(telefonoDto);

            return CreatedAtAction(
                nameof(GetById), 
                new { codigoCliente, idTelefono = telefono.IdTelefono },
                new BaseResponse<TelefonoReadDto>
                {
                    Success = true,
                    Message = "Teléfono creado exitosamente",
                    Data = telefono
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear teléfono para cliente {CodigoCliente}", codigoCliente);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Actualiza un teléfono existente
    /// </summary>
    /// <param name="codigoCliente">Código del cliente</param>
    /// <param name="idTelefono">ID del teléfono</param>
    /// <param name="telefonoDto">Datos actualizados del teléfono</param>
    [HttpPut("{idTelefono}")]
    [ProducesResponseType(typeof(BaseResponse<TelefonoReadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long codigoCliente, long idTelefono, [FromBody] TelefonoUpdateDto telefonoDto)
    {
        try
        {
            if (idTelefono != telefonoDto.IdTelefono)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Success = false,
                    Message = "El ID del teléfono en la ruta no coincide con el ID en los datos",
                    Errors = new[] { "ID de teléfono inconsistente" }
                });
            }

            if (codigoCliente != telefonoDto.CodigoCliente)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Success = false,
                    Message = "El código del cliente en la ruta no coincide con el código en los datos",
                    Errors = new[] { "Código de cliente inconsistente" }
                });
            }

            _logger.LogInformation("Actualizando teléfono {IdTelefono} del cliente {CodigoCliente}", 
                idTelefono, codigoCliente);
                
            // Validación de formato de teléfono
            if (!await _telefonoService.ValidatePhoneFormatAsync(telefonoDto.NumeroTelefono))
            {
                return BadRequest(new ApiErrorResponse 
                { 
                    Success = false,
                    Message = "Formato de teléfono inválido",
                    Errors = new[] { "El número de teléfono no cumple con el formato requerido" }
                });
            }

            var telefono = await _telefonoService.UpdateAsync(idTelefono, telefonoDto);
            return HandleResponse(telefono, "Teléfono actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar teléfono {IdTelefono}", idTelefono);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Elimina un teléfono
    /// </summary>
    /// <param name="codigoCliente">Código del cliente</param>
    /// <param name="idTelefono">ID del teléfono</param>
    [HttpDelete("{idTelefono}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long codigoCliente, long idTelefono)
    {
        try
        {
            _logger.LogInformation("Eliminando teléfono {IdTelefono} del cliente {CodigoCliente}", 
                idTelefono, codigoCliente);
            
            // Validar que no sea el único teléfono antes de eliminar
            await _telefonoService.ValidateClientHasMinimumPhoneAsync(codigoCliente);
            
            await _telefonoService.DeleteAsync(idTelefono);
            return HandleResponse(true, "Teléfono eliminado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar teléfono {IdTelefono}", idTelefono);
            return HandleError(ex);
        }
    }
}