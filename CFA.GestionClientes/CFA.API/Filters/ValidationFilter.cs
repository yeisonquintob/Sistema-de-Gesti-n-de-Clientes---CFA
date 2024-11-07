using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CFA.API.Models.Response;

namespace CFA.API.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Values
                .SelectMany(x => x.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();

            var response = new ApiErrorResponse
            {
                Success = false,
                Message = "Error de validación del modelo",
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No se requiere implementación
    }
}