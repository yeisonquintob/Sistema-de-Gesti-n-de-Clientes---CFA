// Ruta: CFA.Business/Services/Implementation/CatalogoService.cs
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
using CFA.Entities.DTOs.Catalogos;
using FluentValidation.Results;

namespace CFA.Business.Services.Implementation;

public class CatalogoService : ICatalogoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CatalogoService> _logger;

    public CatalogoService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CatalogoService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region TipoDocumento Methods

    public async Task<IEnumerable<TipoDocumentoDto>> GetAllTiposDocumentoAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo lista de tipos de documento");
            var tiposDocumento = await _unitOfWork.TipoDocumentoRepository.GetAllAsync();
            
            var result = _mapper.Map<IEnumerable<TipoDocumentoDto>>(tiposDocumento);
            _logger.LogInformation("Se obtuvieron los tipos de documento exitosamente");
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los tipos de documento");
            throw new BusinessException("Error al obtener los tipos de documento", ex);
        }
    }

    public async Task<TipoDocumentoDto?> GetTipoDocumentoByIdAsync(string id)
    {
        try
        {
            _logger.LogInformation("Obteniendo tipo de documento con ID: {Id}", id);

            if (string.IsNullOrWhiteSpace(id))
            {
                var failure = new ValidationFailure(nameof(id), "El ID del tipo de documento no puede estar vacío");
                throw new ValidationException("Error de validación", new[] { failure });
            }

            var tipoDocumento = await _unitOfWork.TipoDocumentoRepository.GetByIdAsync(id);
            
            if (tipoDocumento == null)
            {
                _logger.LogWarning("No se encontró el tipo de documento con ID: {Id}", id);
                return null;
            }

            var result = _mapper.Map<TipoDocumentoDto>(tipoDocumento);
            _logger.LogInformation("Tipo de documento obtenido exitosamente: {Id}", id);
            
            return result;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el tipo de documento: {Id}", id);
            throw new BusinessException($"Error al obtener el tipo de documento {id}", ex);
        }
    }

    public async Task<bool> ValidateTipoDocumentoExistsAsync(string id)
    {
        try
        {
            _logger.LogInformation("Validando existencia de tipo de documento: {Id}", id);

            if (string.IsNullOrWhiteSpace(id))
            {
                var failure = new ValidationFailure(nameof(id), "El ID del tipo de documento no puede estar vacío");
                throw new ValidationException("Error de validación", new[] { failure });
            }

            var exists = await _unitOfWork.TipoDocumentoRepository.AnyAsync(td => td.IdTipoDocumento == id);
            
            if (!exists)
            {
                _logger.LogWarning("No se encontró el tipo de documento: {Id}", id);
            }
            
            return exists;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar existencia del tipo de documento: {Id}", id);
            throw new BusinessException($"Error al validar el tipo de documento {id}", ex);
        }
    }

    #endregion

    #region Género Methods

    public async Task<IEnumerable<GeneroDto>> GetAllGenerosAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo lista de géneros");
            var generos = await _unitOfWork.GeneroRepository.GetAllAsync();
            
            var result = _mapper.Map<IEnumerable<GeneroDto>>(generos);
            _logger.LogInformation("Se obtuvieron los géneros exitosamente");
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los géneros");
            throw new BusinessException("Error al obtener los géneros", ex);
        }
    }

    public async Task<GeneroDto?> GetGeneroByIdAsync(string id)
    {
        try
        {
            _logger.LogInformation("Obteniendo género con ID: {Id}", id);

            if (string.IsNullOrWhiteSpace(id))
            {
                var failure = new ValidationFailure(nameof(id), "El ID del género no puede estar vacío");
                throw new ValidationException("Error de validación", new[] { failure });
            }

            var genero = await _unitOfWork.GeneroRepository.GetByIdAsync(id);
            
            if (genero == null)
            {
                _logger.LogWarning("No se encontró el género con ID: {Id}", id);
                return null;
            }

            var result = _mapper.Map<GeneroDto>(genero);
            _logger.LogInformation("Género obtenido exitosamente: {Id}", id);
            
            return result;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el género: {Id}", id);
            throw new BusinessException($"Error al obtener el género {id}", ex);
        }
    }

    public async Task<bool> ValidateGeneroExistsAsync(string id)
    {
        try
        {
            _logger.LogInformation("Validando existencia de género: {Id}", id);

            if (string.IsNullOrWhiteSpace(id))
            {
                var failure = new ValidationFailure(nameof(id), "El ID del género no puede estar vacío");
                throw new ValidationException("Error de validación", new[] { failure });
            }

            var exists = await _unitOfWork.GeneroRepository.AnyAsync(g => g.IdGenero == id);
            
            if (!exists)
            {
                _logger.LogWarning("No se encontró el género: {Id}", id);
            }
            
            return exists;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar existencia del género: {Id}", id);
            throw new BusinessException($"Error al validar el género {id}", ex);
        }
    }

    #endregion
}