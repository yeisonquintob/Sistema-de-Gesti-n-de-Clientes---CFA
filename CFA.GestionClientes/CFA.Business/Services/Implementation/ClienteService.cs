using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FluentValidation;
using CFA.Business.Services.Interfaces;
using CFA.Business.Helpers;
using CFA.Common.Exceptions;
using CFA.DataAccess.UnitOfWork;
using CFA.Entities.Models;
using CFA.Entities.DTOs.Cliente;
using CFA.Entities.DTOs.Consultas;
using FluentValidation.Results; 

namespace CFA.Business.Services.Implementation;

public class ClienteService : IClienteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICatalogoService _catalogoService;
    private readonly IValidator<ClienteCreateDto> _createValidator;
    private readonly IValidator<ClienteUpdateDto> _updateValidator;
    private readonly ILogger<ClienteService> _logger;

    public ClienteService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICatalogoService catalogoService,
        IValidator<ClienteCreateDto> createValidator,
        IValidator<ClienteUpdateDto> updateValidator,
        ILogger<ClienteService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _catalogoService = catalogoService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    public async Task<ClienteReadDto> CreateAsync(ClienteCreateDto clienteDto)
    {
        try
        {
            _logger.LogInformation("Iniciando creación de cliente");
            
            // Validar el DTO de creación
            await _createValidator.ValidateAndThrowAsync(clienteDto);

            // Validaciones de negocio
            await ValidateDocumentAsync(clienteDto.TipoDocumento, clienteDto.NumeroDocumento);
            await ValidateAgeForDocumentTypeAsync(clienteDto.TipoDocumento, clienteDto.FechaNacimiento);

            // Validar existencia de catálogos
            if (!await _catalogoService.ValidateTipoDocumentoExistsAsync(clienteDto.TipoDocumento))
                throw new ValidationException("Tipo de documento inválido");

            if (!await _catalogoService.ValidateGeneroExistsAsync(clienteDto.Genero))
                throw new ValidationException("Género inválido");

            // Mapear y guardar el cliente
            var cliente = _mapper.Map<Cliente>(clienteDto);
            await _unitOfWork.ClienteRepository.AddAsync(cliente);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Cliente creado exitosamente con código {Codigo}", cliente.Codigo);
            return _mapper.Map<ClienteReadDto>(cliente);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear cliente");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cliente");
            throw new BusinessException("Error al crear el cliente", ex);
        }
    }

    public async Task<ClienteReadDto> UpdateAsync(long codigo, ClienteUpdateDto clienteDto)
    {
        try
        {
            _logger.LogInformation("Iniciando actualización del cliente {Codigo}", codigo);
            
            // Validar el DTO de actualización
            await _updateValidator.ValidateAndThrowAsync(clienteDto);

            // Obtener el cliente existente
            var cliente = await _unitOfWork.ClienteRepository.GetByIdAsync(codigo)
                ?? throw new NotFoundException($"No se encontró el cliente con código {codigo}");

            // Validaciones de negocio
            await ValidateDocumentAsync(clienteDto.TipoDocumento, clienteDto.NumeroDocumento, codigo);
            await ValidateAgeForDocumentTypeAsync(clienteDto.TipoDocumento, clienteDto.FechaNacimiento);

            // Validar catálogos
            if (!await _catalogoService.ValidateTipoDocumentoExistsAsync(clienteDto.TipoDocumento))
                throw new ValidationException("Tipo de documento inválido");

            if (!await _catalogoService.ValidateGeneroExistsAsync(clienteDto.Genero))
                throw new ValidationException("Género inválido");

            // Actualizar el cliente
            _mapper.Map(clienteDto, cliente);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Cliente {Codigo} actualizado exitosamente", codigo);
            return _mapper.Map<ClienteReadDto>(cliente);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al actualizar cliente {Codigo}", codigo);
            throw;
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Cliente {Codigo} no encontrado", codigo);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar cliente {Codigo}", codigo);
            throw new BusinessException($"Error al actualizar el cliente {codigo}", ex);
        }
    }

    public async Task DeleteAsync(long codigo)
    {
        try
        {
            _logger.LogInformation("Iniciando eliminación del cliente {Codigo}", codigo);

            var cliente = await _unitOfWork.ClienteRepository.GetByIdAsync(codigo)
                ?? throw new NotFoundException($"No se encontró el cliente con código {codigo}");

            await _unitOfWork.ClienteRepository.DeleteAsync(cliente);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Cliente {Codigo} eliminado exitosamente", codigo);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Cliente {Codigo} no encontrado", codigo);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar cliente {Codigo}", codigo);
            throw new BusinessException($"Error al eliminar el cliente {codigo}", ex);
        }
    }

    public async Task<ClienteReadDto?> GetByIdAsync(long codigo)
    {
        try
        {
            var cliente = await _unitOfWork.ClienteRepository.GetByIdAsync(codigo);
            return cliente == null ? null : _mapper.Map<ClienteReadDto>(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cliente {Codigo}", codigo);
            throw new BusinessException($"Error al obtener el cliente {codigo}", ex);
        }
    }

    public async Task<IEnumerable<ClienteSimpleDto>> GetAllAsync()
    {
        try
        {
            var clientes = await _unitOfWork.ClienteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClienteSimpleDto>>(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los clientes");
            throw new BusinessException("Error al obtener el listado de clientes", ex);
        }
    }

    public async Task<IEnumerable<ClienteSimpleDto>> SearchByNameAsync(string nombre)
    {
        try
        {
            var normalizedNombre = nombre.Trim().ToUpper();
            var clientes = await _unitOfWork.ClienteRepository.FindAsync(c =>
                c.Nombres.Contains(normalizedNombre) ||
                c.Apellido1.Contains(normalizedNombre) ||
                (c.Apellido2 != null && c.Apellido2.Contains(normalizedNombre)));

            return _mapper.Map<IEnumerable<ClienteSimpleDto>>(
                clientes.OrderBy(c => c.Nombres).ThenBy(c => c.Apellido1)
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar clientes por nombre {Nombre}", nombre);
            throw new BusinessException("Error al buscar clientes por nombre", ex);
        }
    }

    public async Task<IEnumerable<ClienteSimpleDto>> SearchByDocumentAsync(string numeroDocumento)
    {
        try
        {
            var clientes = await _unitOfWork.ClienteRepository.FindAsync(
                c => c.NumeroDocumento.Contains(numeroDocumento));

            return _mapper.Map<IEnumerable<ClienteSimpleDto>>(
                clientes.OrderByDescending(c => c.NumeroDocumento)
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar clientes por documento {Documento}", numeroDocumento);
            throw new BusinessException("Error al buscar clientes por documento", ex);
        }
    }

    public async Task<IEnumerable<ClientePorFechaDto>> SearchByBirthDateRangeAsync(
        DateTime fechaInicial,
        DateTime fechaFinal)
    {
        try
        {
            if (fechaInicial > fechaFinal)
                throw new ValidationException("La fecha inicial no puede ser mayor que la fecha final");

            var clientes = await _unitOfWork.ClienteRepository.FindAsync(
                c => c.FechaNacimiento >= fechaInicial && c.FechaNacimiento <= fechaFinal);

            return _mapper.Map<IEnumerable<ClientePorFechaDto>>(
                clientes.OrderBy(c => c.FechaNacimiento)
            );
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar clientes por rango de fechas");
            throw new BusinessException("Error al buscar clientes por rango de fechas", ex);
        }
    }

    public async Task<IEnumerable<ClienteMultiplesTelefonosDto>> GetClientsWithMultiplePhonesAsync()
    {
        try
        {
            var clientes = await _unitOfWork.ClienteRepository.GetAllWithPhonesAsync();
            var clientesConMultiplesTelefonos = clientes
                .Where(c => c.Telefonos.Count > 1)
                .ToList();

            return _mapper.Map<IEnumerable<ClienteMultiplesTelefonosDto>>(clientesConMultiplesTelefonos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener clientes con múltiples teléfonos");
            throw new BusinessException("Error al obtener clientes con múltiples teléfonos", ex);
        }
    }

    public async Task<IEnumerable<ClienteMultiplesDireccionesDto>> GetClientsWithMultipleAddressesAsync()
    {
        try
        {
            var clientes = await _unitOfWork.ClienteRepository.GetAllWithAddressesAsync();
            var clientesConMultiplesDirecciones = clientes
                .Where(c => c.Direcciones.Count > 1)
                .ToList();

            return _mapper.Map<IEnumerable<ClienteMultiplesDireccionesDto>>(clientesConMultiplesDirecciones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener clientes con múltiples direcciones");
            throw new BusinessException("Error al obtener clientes con múltiples direcciones", ex);
        }
    }

    public async Task<bool> ValidateDocumentAsync(
    string tipoDocumento,
    string numeroDocumento,
    long? codigoCliente = null)
{
    try
    {
        // Validar que existe el tipo de documento
        if (!await _catalogoService.ValidateTipoDocumentoExistsAsync(tipoDocumento))
            throw new ValidationException("Tipo de documento inválido");

        // Verificar si existe el documento
        var documentoExiste = await _unitOfWork.ClienteRepository.AnyAsync(c =>
            c.TipoDocumento == tipoDocumento &&
            c.NumeroDocumento == numeroDocumento &&
            (!codigoCliente.HasValue || c.Codigo != codigoCliente.Value));

        if (documentoExiste)
            throw new ValidationException("Ya existe un cliente con el mismo tipo y número de documento");

        return true;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al validar documento {TipoDocumento} {NumeroDocumento}", 
            tipoDocumento, numeroDocumento);
        throw;
    }
}

public async Task<bool> ValidateAgeForDocumentTypeAsync(
    string tipoDocumento, 
    DateTime fechaNacimiento)
{
    try
    {
        // Validar que existe el tipo de documento de forma asíncrona
        if (!await _catalogoService.ValidateTipoDocumentoExistsAsync(tipoDocumento))
            throw new ValidationException("Tipo de documento inválido");

        var edad = await Task.Run(() => ClienteHelper.CalcularEdad(fechaNacimiento));
        
        bool esValido = await Task.Run(() => ClienteHelper.ValidarTipoDocumentoPorEdad(tipoDocumento, edad));
        
        if (!esValido)
        {
            var mensaje = tipoDocumento switch
            {
                "RC" => "Para Registro Civil la edad debe ser menor a 7 años",
                "TI" => "Para Tarjeta de Identidad la edad debe estar entre 8 y 17 años",
                "CC" => "Para Cédula de Ciudadanía la edad debe ser mayor a 18 años",
                _ => "Tipo de documento inválido"
            };
            throw new ValidationException(mensaje);
        }

        return true;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al validar edad para tipo de documento");
        throw;
    }
}
}