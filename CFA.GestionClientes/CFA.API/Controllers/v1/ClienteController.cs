// CFA.API/Controllers/v1/ClienteController.cs

using Microsoft.AspNetCore.Mvc;
using CFA.API.Controllers.Base;
using CFA.API.Models.Response;
using CFA.Business.Services.Interfaces;
using CFA.Entities.DTOs.Cliente;
using CFA.Entities.DTOs.Consultas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CFA.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CFA.API.Controllers.v1;

[ApiController]
[Route("api/v1/clientes")]
public class ClienteController : BaseApiController
{
    private readonly IClienteService _clienteService;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(
        IClienteService clienteService,
        ILogger<ClienteController> logger)
    {
        _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene un listado de todos los clientes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<ClienteSimpleDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.LogInformation("Obteniendo listado de clientes");
            var clientes = await _clienteService.GetAllAsync();
            return HandleResponse(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener clientes");
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Obtiene un cliente por su código
    /// </summary>
    [HttpGet("{codigo}")]
    [ProducesResponseType(typeof(BaseResponse<ClienteReadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long codigo)
    {
        try
        {
            _logger.LogInformation("Obteniendo cliente con código {Codigo}", codigo);
            var cliente = await _clienteService.GetByIdAsync(codigo);
            return HandleResponse(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cliente {Codigo}", codigo);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Crea un nuevo cliente
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<ClienteReadDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] ClienteCreateDto clienteDto)
    {
        try
        {
            // Validaciones de negocio
            await ValidateClienteAsync(clienteDto.TipoDocumento, clienteDto.NumeroDocumento, clienteDto.FechaNacimiento);
            
            _logger.LogInformation("Creando nuevo cliente");
            var cliente = await _clienteService.CreateAsync(clienteDto);
            
            return CreatedAtAction(
                nameof(GetById), 
                new { codigo = cliente.Codigo }, 
                new BaseResponse<ClienteReadDto> 
                { 
                    Success = true, 
                    Message = "Cliente creado exitosamente",
                    Data = cliente 
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cliente");
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Actualiza un cliente existente
    /// </summary>
    [HttpPut("{codigo}")]
    [ProducesResponseType(typeof(BaseResponse<ClienteReadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long codigo, [FromBody] ClienteUpdateDto clienteDto)
    {
        try
        {
            // Validaciones de negocio
            await ValidateClienteAsync(clienteDto.TipoDocumento, clienteDto.NumeroDocumento, clienteDto.FechaNacimiento, codigo);
            
            _logger.LogInformation("Actualizando cliente {Codigo}", codigo);
            var cliente = await _clienteService.UpdateAsync(codigo, clienteDto);
            return HandleResponse(cliente, "Cliente actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar cliente {Codigo}", codigo);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Elimina un cliente
    /// </summary>
    [HttpDelete("{codigo}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long codigo)
    {
        try
        {
            _logger.LogInformation("Eliminando cliente {Codigo}", codigo);
            await _clienteService.DeleteAsync(codigo);
            return HandleResponse(true, "Cliente eliminado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar cliente {Codigo}", codigo);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Busca clientes por nombre
    /// </summary>
    [HttpGet("buscar/nombre/{texto}")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<ClienteSimpleDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchByName(string texto)
    {
        try
        {
            _logger.LogInformation("Buscando clientes por nombre {Texto}", texto);
            var clientes = await _clienteService.SearchByNameAsync(texto);
            return HandleResponse(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar clientes por nombre {Texto}", texto);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Busca clientes por número de documento
    /// </summary>
    [HttpGet("buscar/documento/{numero}")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<ClienteSimpleDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchByDocument(string numero)
    {
        try
        {
            _logger.LogInformation("Buscando clientes por documento {Numero}", numero);
            var clientes = await _clienteService.SearchByDocumentAsync(numero);
            return HandleResponse(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar clientes por documento {Numero}", numero);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Busca clientes por rango de fecha de nacimiento
    /// </summary>
    [HttpGet("buscar/fecha-nacimiento")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<ClientePorFechaDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchByBirthDate([FromQuery] DateTime fechaInicial, [FromQuery] DateTime fechaFinal)
    {
        try
        {
            _logger.LogInformation("Buscando clientes por rango de fecha {FechaInicial} - {FechaFinal}", 
                fechaInicial, fechaFinal);
            
            var clientes = await _clienteService.SearchByBirthDateRangeAsync(fechaInicial, fechaFinal);
            return HandleResponse(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar clientes por rango de fecha");
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Obtiene clientes con múltiples teléfonos
    /// </summary>
    [HttpGet("multiples-telefonos")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<ClienteMultiplesTelefonosDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClientsWithMultiplePhones()
    {
        try
        {
            _logger.LogInformation("Obteniendo clientes con múltiples teléfonos");
            var clientes = await _clienteService.GetClientsWithMultiplePhonesAsync();
            return HandleResponse(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener clientes con múltiples teléfonos");
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Obtiene clientes con múltiples direcciones
    /// </summary>
    [HttpGet("multiples-direcciones")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<ClienteMultiplesDireccionesDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClientsWithMultipleAddresses()
    {
        try
        {
            _logger.LogInformation("Obteniendo clientes con múltiples direcciones");
            var clientes = await _clienteService.GetClientsWithMultipleAddressesAsync();
            return HandleResponse(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener clientes con múltiples direcciones");
            return HandleError(ex);
        }
    }

    private async Task ValidateClienteAsync(string tipoDocumento, string numeroDocumento, DateTime fechaNacimiento, long? codigoCliente = null)
    {
        // Validar tipo de documento según edad
        if (!await _clienteService.ValidateAgeForDocumentTypeAsync(tipoDocumento, fechaNacimiento))
            throw new ValidationException("La edad no corresponde con el tipo de documento");
        
        // Validar documento único
        if (!await _clienteService.ValidateDocumentAsync(tipoDocumento, numeroDocumento, codigoCliente))
            throw new ValidationException("El número de documento ya existe para otro cliente");
    }
}