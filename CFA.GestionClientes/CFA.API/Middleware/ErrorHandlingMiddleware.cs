using System.Net;
using System.Text.Json;
using CFA.API.Models.Response;
using CFA.Common.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace CFA.API.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no manejado en la aplicación");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new ApiErrorResponse
        {
            Success = false,
            Message = "Se produjo un error al procesar la solicitud"
        };

        var statusCode = exception switch
        {
            ValidationException validationEx => 
                HandleValidationException(validationEx, response),
            CFA.Common.Exceptions.ValidationException validationEx =>
                HandleCustomValidationException(validationEx, response),
            NotFoundException notFoundEx => 
                HandleNotFoundException(notFoundEx, response),
            BusinessException businessEx => 
                HandleBusinessException(businessEx, response),
            UnauthorizedAccessException _ => 
                HandleUnauthorizedException(response),
            _ => HandleUnknownException(response)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int HandleValidationException(ValidationException exception, ApiErrorResponse response)
    {
        response.Message = "Error de validación";
        response.Errors = exception.Errors.Select(e => e.ErrorMessage).ToArray();
        return (int)HttpStatusCode.BadRequest;
    }

    private static int HandleCustomValidationException(CFA.Common.Exceptions.ValidationException exception, ApiErrorResponse response)
    {
        response.Message = "Error de validación";
        response.Errors = new[] { exception.Message };
        return (int)HttpStatusCode.BadRequest;
    }

    private static int HandleNotFoundException(NotFoundException exception, ApiErrorResponse response)
    {
        response.Message = exception.Message;
        response.Errors = new[] { "El recurso solicitado no fue encontrado" };
        return (int)HttpStatusCode.NotFound;
    }

    private static int HandleBusinessException(BusinessException exception, ApiErrorResponse response)
    {
        response.Message = exception.Message;
        response.Errors = new[] { exception.InnerException?.Message ?? "Error en la lógica de negocio" };
        return (int)HttpStatusCode.BadRequest;
    }

    private static int HandleUnauthorizedException(ApiErrorResponse response)
    {
        response.Message = "Acceso no autorizado";
        response.Errors = new[] { "No tiene permisos para realizar esta operación" };
        return (int)HttpStatusCode.Unauthorized;
    }

    private static int HandleUnknownException(ApiErrorResponse response)
    {
        response.Message = "Error interno del servidor";
        response.Errors = new[] { "Se produjo un error inesperado" };
        return (int)HttpStatusCode.InternalServerError;
    }
}