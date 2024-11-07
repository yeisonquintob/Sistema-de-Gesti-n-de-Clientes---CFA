using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CFA.API.Models.Response;
using CFA.Common.Exceptions;

namespace CFA.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionFilter(
        ILogger<ExceptionFilter> logger,
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Error no manejado en la aplicación");

        var response = new ApiErrorResponse
        {
            Success = false,
            Message = GetExceptionMessage(context.Exception),
            Errors = GetExceptionDetails(context.Exception)
        };

        context.Result = new ObjectResult(response)
        {
            StatusCode = GetStatusCode(context.Exception)
        };

        context.ExceptionHandled = true;
    }

    private string GetExceptionMessage(Exception exception)
    {
        return exception switch
        {
            BusinessException => "Error de negocio",
            ValidationException => "Error de validación",
            NotFoundException => "Recurso no encontrado",
            UnauthorizedAccessException => "Acceso no autorizado",
            _ => _environment.IsDevelopment() 
                ? $"Error interno: {exception.Message}" 
                : "Se produjo un error interno"
        };
    }

    private string[] GetExceptionDetails(Exception exception)
    {
        if (_environment.IsDevelopment())
        {
            var details = new List<string>
            {
                exception.Message
            };

            if (exception.InnerException != null)
            {
                details.Add(exception.InnerException.Message);
            }

            return details.ToArray();
        }

        return exception switch
        {
            BusinessException ex => new[] { ex.Message },
            ValidationException ex => new[] { ex.Message },
            NotFoundException ex => new[] { ex.Message },
            UnauthorizedAccessException => new[] { "No tiene permisos para realizar esta operación" },
            _ => new[] { "Se produjo un error inesperado" }
        };
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            BusinessException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}