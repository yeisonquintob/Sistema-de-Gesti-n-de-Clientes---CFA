
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FluentValidation;
using CFA.Business.Services.Interfaces;
using CFA.Common.Exceptions;
using CFA.DataAccess.UnitOfWork;
using CFA.Entities.Models;
using CFA.Entities.DTOs.Direccion;
using FluentValidation.Results;

namespace CFA.Business.Services.Implementation;

public class DireccionService : IDireccionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<DireccionCreateDto> _createValidator;
    private readonly IValidator<DireccionUpdateDto> _updateValidator;
    private readonly ILogger<DireccionService> _logger;

    public DireccionService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<DireccionCreateDto> createValidator,
        IValidator<DireccionUpdateDto> updateValidator,
        ILogger<DireccionService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    public async Task<DireccionReadDto> CreateAsync(long codigoCliente, DireccionCreateDto direccionDto)
    {
        try
        {
            _logger.LogInformation("Iniciando creación de dirección para cliente {CodigoCliente}", codigoCliente);

            // Validar el DTO de creación
            await _createValidator.ValidateAndThrowAsync(direccionDto);

            // Validar que el cliente existe
            var cliente = await _unitOfWork.ClienteRepository.GetByIdAsync(codigoCliente)
                ?? throw new NotFoundException($"No se encontró el cliente con código {codigoCliente}");

            // Validar la dirección
            await ValidateDireccionAsync(direccionDto.Direccion);

            // Crear la dirección
            var direccion = _mapper.Map<DireccionCliente>(direccionDto);
            direccion.CodigoCliente = codigoCliente;

            await _unitOfWork.DireccionRepository.AddAsync(direccion);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Dirección creada exitosamente con ID {IdDireccion} para cliente {CodigoCliente}", 
                direccion.IdDireccion, codigoCliente);

            return _mapper.Map<DireccionReadDto>(direccion);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear dirección para cliente {CodigoCliente}", codigoCliente);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear dirección para cliente {CodigoCliente}", codigoCliente);
            throw new BusinessException($"Error al crear la dirección para el cliente {codigoCliente}", ex);
        }
    }

    public async Task<DireccionReadDto> UpdateAsync(long idDireccion, long codigoCliente, DireccionUpdateDto direccionDto)
    {
        try
        {
            _logger.LogInformation("Iniciando actualización de dirección {IdDireccion} para cliente {CodigoCliente}", 
                idDireccion, codigoCliente);

            // Validar el DTO de actualización
            await _updateValidator.ValidateAndThrowAsync(direccionDto);

            // Validar que existe la dirección y pertenece al cliente
            var direccion = await _unitOfWork.DireccionRepository.GetByIdAsync(idDireccion);
            
            if (direccion == null || direccion.CodigoCliente != codigoCliente)
                throw new NotFoundException($"No se encontró la dirección {idDireccion} para el cliente {codigoCliente}");

            // Validar la dirección
            await ValidateDireccionAsync(direccionDto.Direccion);

            // Actualizar la dirección
            _mapper.Map(direccionDto, direccion);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Dirección {IdDireccion} actualizada exitosamente", idDireccion);
            return _mapper.Map<DireccionReadDto>(direccion);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al actualizar dirección {IdDireccion}", idDireccion);
            throw;
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Dirección {IdDireccion} no encontrada", idDireccion);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar dirección {IdDireccion}", idDireccion);
            throw new BusinessException($"Error al actualizar la dirección {idDireccion}", ex);
        }
    }

    public async Task DeleteAsync(long idDireccion)
    {
        try
        {
            _logger.LogInformation("Iniciando eliminación de dirección {IdDireccion}", idDireccion);

            var direccion = await _unitOfWork.DireccionRepository.GetByIdAsync(idDireccion)
                ?? throw new NotFoundException($"No se encontró la dirección con ID {idDireccion}");

            // Verificar que no sea la única dirección del cliente
            var cantidadDirecciones = await _unitOfWork.DireccionRepository
                .CountAsync(d => d.CodigoCliente == direccion.CodigoCliente);

            if (cantidadDirecciones <= 1)
                throw new ValidationException("No se puede eliminar la única dirección del cliente");

            await _unitOfWork.DireccionRepository.DeleteAsync(direccion);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Dirección {IdDireccion} eliminada exitosamente", idDireccion);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al eliminar dirección {IdDireccion}", idDireccion);
            throw;
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Dirección {IdDireccion} no encontrada", idDireccion);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar dirección {IdDireccion}", idDireccion);
            throw new BusinessException($"Error al eliminar la dirección {idDireccion}", ex);
        }
    }

    public async Task<DireccionReadDto?> GetByIdAsync(long idDireccion)
    {
        try
        {
            var direccion = await _unitOfWork.DireccionRepository.GetByIdAsync(idDireccion);
            return direccion == null ? null : _mapper.Map<DireccionReadDto>(direccion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener dirección {IdDireccion}", idDireccion);
            throw new BusinessException($"Error al obtener la dirección {idDireccion}", ex);
        }
    }

    public async Task<IEnumerable<DireccionReadDto>> GetAllByClienteAsync(long codigoCliente)
    {
        try
        {
            var direcciones = await _unitOfWork.DireccionRepository
                .FindAsync(d => d.CodigoCliente == codigoCliente);

            return _mapper.Map<IEnumerable<DireccionReadDto>>(direcciones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener direcciones del cliente {CodigoCliente}", codigoCliente);
            throw new BusinessException($"Error al obtener las direcciones del cliente {codigoCliente}", ex);
        }
    }

    public async Task<bool> ValidateDireccionAsync(string direccion)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(direccion))
                throw new ValidationException("La dirección no puede estar vacía");

            if (direccion.Length > 100)
                throw new ValidationException("La dirección no puede exceder los 100 caracteres");

            // Aquí podrían agregarse más validaciones específicas de formato de dirección
            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar dirección");
            throw;
        }
    }
}