// CFA.API/Controllers/Base/BaseApiController.cs

using Microsoft.AspNetCore.Mvc;
using CFA.API.Models.Response;
using System;
using System.Collections.Generic;
using System.Net;
using FluentValidation;

namespace CFA.API.Controllers.Base;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult HandleResponse<T>(T data, string message = "Operación exitosa")
    {
        if (data == null)
            return NotFound(new BaseResponse<T> 
            { 
                Success = false,
                Message = "No se encontraron datos",
                Data = default
            });

        return Ok(new BaseResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        });
    }

    protected IActionResult HandleError(Exception ex)
    {
        var statusCode = ex switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            KeyNotFoundException => HttpStatusCode.NotFound,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };

        var response = new ApiErrorResponse
        {
            Success = false,
            Message = "Error en la operación",
            Errors = new[] { ex.Message }
        };

        return StatusCode((int)statusCode, response);
    }

    protected IActionResult HandlePagedResponse<T>(
        IEnumerable<T> data, 
        int totalCount, 
        int pageNumber, 
        int pageSize,
        string message = "Operación exitosa")
    {
        var response = new PagedResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        return Ok(response);
    }
}