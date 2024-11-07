
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FluentValidation;
using CFA.Business.Services.Interfaces;
using CFA.Common.Exceptions;
using CFA.DataAccess.UnitOfWork;
using CFA.Entities.Models;
using CFA.Entities.DTOs.Telefono;
using FluentValidation.Results;

namespace CFA.Business.Services.Implementation;

public class TelefonoService : ITelefonoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<TelefonoCreateDto> _createValidator;
    private readonly IValidator<TelefonoUpdateDto> _updateValidator;
    private readonly ILogger<TelefonoService> _logger;

    public TelefonoService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<TelefonoCreateDto> createValidator,
        IValidator<TelefonoUpdateDto> updateValidator,
        ILogger<TelefonoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    public async Task<TelefonoReadDto> CreateAsync(long codigoCliente, TelefonoCreateDto telefonoDto)
    {
        try
        {
            _logger.LogInformation("Iniciando creación de teléfono para cliente {CodigoCliente}", codigoCliente);

            // Validar el DTO de creación
            await _createValidator.ValidateAndThrowAsync(telefonoDto);

            // Validar que el cliente existe
            var cliente = await _unitOfWork.ClienteRepository.GetByIdAsync(codigoCliente)
                ?? throw new NotFoundException($"No se encontró el cliente con código {codigoCliente}");

            // Validar el número de teléfono
            await ValidateNumeroTelefonoAsync(telefonoDto.NumeroTelefono);

            // Crear el teléfono
            var telefono = _mapper.Map<TelefonoCliente>(telefonoDto);
            telefono.CodigoCliente = codigoCliente;

            await _unitOfWork.TelefonoRepository.AddAsync(telefono);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Teléfono creado exitosamente con ID {IdTelefono} para cliente {CodigoCliente}", 
                telefono.IdTelefono, codigoCliente);

            return _mapper.Map<TelefonoReadDto>(telefono);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear teléfono para cliente {CodigoCliente}", codigoCliente);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear teléfono para cliente {CodigoCliente}", codigoCliente);
            throw new BusinessException($"Error al crear el teléfono para el cliente {codigoCliente}", ex);
        }
    }

    public async Task<TelefonoReadDto> UpdateAsync(long idTelefono, long codigoCliente, TelefonoUpdateDto telefonoDto)
    {
        try
        {
            _logger.LogInformation("Iniciando actualización de teléfono {IdTelefono} para cliente {CodigoCliente}", 
                idTelefono, codigoCliente);

            // Validar el DTO de actualización
            await _updateValidator.ValidateAndThrowAsync(telefonoDto);

            // Validar que existe el teléfono y pertenece al cliente
            var telefono = await _unitOfWork.TelefonoRepository.GetByIdAsync(idTelefono);
            
            if (telefono == null || telefono.CodigoCliente != codigoCliente)
                throw new NotFoundException($"No se encontró el teléfono {idTelefono} para el cliente {codigoCliente}");

            // Validar el número de teléfono
            await ValidateNumeroTelefonoAsync(telefonoDto.NumeroTelefono);

            // Actualizar el teléfono
            _mapper.Map(telefonoDto, telefono);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Teléfono {IdTelefono} actualizado exitosamente", idTelefono);
            return _mapper.Map<TelefonoReadDto>(telefono);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al actualizar teléfono {IdTelefono}", idTelefono);
            throw;
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Teléfono {IdTelefono} no encontrado", idTelefono);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar teléfono {IdTelefono}", idTelefono);
            throw new BusinessException($"Error al actualizar el teléfono {idTelefono}", ex);
        }
    }

    public async Task DeleteAsync(long idTelefono)
    {
        try
        {
            _logger.LogInformation("Iniciando eliminación de teléfono {IdTelefono}", idTelefono);

            var telefono = await _unitOfWork.TelefonoRepository.GetByIdAsync(idTelefono)
                ?? throw new NotFoundException($"No se encontró el teléfono con ID {idTelefono}");

            // Verificar que no sea el único teléfono del cliente
            var cantidadTelefonos = await _unitOfWork.TelefonoRepository
                .CountAsync(t => t.CodigoCliente == telefono.CodigoCliente);

            if (cantidadTelefonos <= 1)
                throw new ValidationException("No se puede eliminar el único teléfono del cliente");

            await _unitOfWork.TelefonoRepository.DeleteAsync(telefono);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Teléfono {IdTelefono} eliminado exitosamente", idTelefono);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al eliminar teléfono {IdTelefono}", idTelefono);
            throw;
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Teléfono {IdTelefono} no encontrado", idTelefono);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar teléfono {IdTelefono}", idTelefono);
            throw new BusinessException($"Error al eliminar el teléfono {idTelefono}", ex);
        }
    }

    public async Task<TelefonoReadDto?> GetByIdAsync(long idTelefono)
    {
        try
        {
            var telefono = await _unitOfWork.TelefonoRepository.GetByIdAsync(idTelefono);
            return telefono == null ? null : _mapper.Map<TelefonoReadDto>(telefono);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener teléfono {IdTelefono}", idTelefono);
            throw new BusinessException($"Error al obtener el teléfono {idTelefono}", ex);
        }
    }

    public async Task<IEnumerable<TelefonoReadDto>> GetAllByClienteAsync(long codigoCliente)
    {
        try
        {
            var telefonos = await _unitOfWork.TelefonoRepository
                .FindAsync(t => t.CodigoCliente == codigoCliente);

            return _mapper.Map<IEnumerable<TelefonoReadDto>>(telefonos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener teléfonos del cliente {CodigoCliente}", codigoCliente);
            throw new BusinessException($"Error al obtener los teléfonos del cliente {codigoCliente}", ex);
        }
    }

    public async Task<bool> ValidateNumeroTelefonoAsync(string numeroTelefono)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(numeroTelefono))
                throw new ValidationException("El número de teléfono no puede estar vacío");

            if (numeroTelefono.Length < 7 || numeroTelefono.Length > 15)
                throw new ValidationException("El número de teléfono debe tener entre 7 y 15 dígitos");

            // Validar que solo contenga números
            if (!Regex.IsMatch(numeroTelefono, @"^\d+$"))
                throw new ValidationException("El número de teléfono solo debe contener dígitos");

            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar número de teléfono");
            throw;
        }
    }
}