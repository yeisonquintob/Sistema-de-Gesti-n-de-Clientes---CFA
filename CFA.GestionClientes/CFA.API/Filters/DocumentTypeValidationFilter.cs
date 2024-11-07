using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CFA.API.Models.Response;
using CFA.Business.Services.Interfaces;
using System.Text.RegularExpressions;

namespace CFA.API.Filters;

public class DocumentTypeValidationFilter : IAsyncActionFilter
{
    private readonly ICatalogoService _catalogoService;
    private readonly ILogger<DocumentTypeValidationFilter> _logger;

    public DocumentTypeValidationFilter(
        ICatalogoService catalogoService,
        ILogger<DocumentTypeValidationFilter> logger)
    {
        _catalogoService = catalogoService;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var tipoDocumento = context.ActionArguments
            .FirstOrDefault(x => x.Key.ToLower().Contains("tipodocumento"))
            .Value?.ToString();

        if (!string.IsNullOrEmpty(tipoDocumento))
        {
            var isValid = await ValidateDocumentTypeAsync(tipoDocumento);
            if (!isValid)
            {
                context.Result = new BadRequestObjectResult(new ApiErrorResponse
                {
                    Success = false,
                    Message = "Tipo de documento inválido",
                    Errors = new[] { "El tipo de documento especificado no es válido" }
                });
                return;
            }
        }

        await next();
    }

    private async Task<bool> ValidateDocumentTypeAsync(string tipoDocumento)
    {
        try
        {
            return await _catalogoService.ValidateTipoDocumentoExistsAsync(tipoDocumento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar tipo de documento {TipoDocumento}", tipoDocumento);
            return false;
        }
    }
}