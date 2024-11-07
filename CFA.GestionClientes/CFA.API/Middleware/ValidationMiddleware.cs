using System.Text.Json;
using CFA.API.Models.Response;
using FluentValidation;

namespace CFA.API.Middleware;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationMiddleware> _logger;

    public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
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
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validación");
            await HandleValidationExceptionAsync(context, ex);
        }
    }

    private static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        var response = new ApiErrorResponse
        {
            Success = false,
            Message = "Error de validación",
            Errors = exception.Errors.Select(e => e.ErrorMessage).ToArray()
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}