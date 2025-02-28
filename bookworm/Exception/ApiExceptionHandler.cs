
using System.ComponentModel.DataAnnotations;
using System.Net;
using Bookworm.Dto;
using Microsoft.AspNetCore.Diagnostics;

namespace Bookworm.Exception;

public class ApiExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ApiExceptionHandler> _logger;

    public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
    {
        ErrorResponse response = new ErrorResponse();
        int statusCode = (int)HttpStatusCode.InternalServerError;
        switch (exception)
        {
            // ✅ Handles Unauthorized errors (401)
            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                response.ErrorMessage = "Unauthorized access.";
                break;

            // ✅ Handles Forbidden errors (403)
            case InvalidOperationException:
                statusCode = (int)HttpStatusCode.Forbidden;
                response.ErrorMessage = "Forbidden.";
                break;

            // ✅ Handles Resource Not Found errors (404)
            case KeyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                response.ErrorMessage = "Resource not found.";
                break;

            // ✅ Handles Validation Errors (400) - Extracts field errors from ModelState
            case ValidationException validationException:
                statusCode = (int)HttpStatusCode.BadRequest;
                response.ErrorMessage = validationException.Message;
                break;

            // ✅ Handles ModelState validation errors (400)
            case BadHttpRequestException:
                statusCode = (int)HttpStatusCode.BadRequest;
                response.ErrorMessage = "Bad request.";
                break;

            // ✅ Handles all unexpected errors (500)
            default:
                _logger.LogError(exception, "Unhandled exception occurred."); // Log the full error
                response.ErrorMessage = "An unexpected error occurred.";
                break;
        }

        response.StatusCode = statusCode;

        // 🔹 Set HTTP response
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}
