using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler (ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
            "Error Message: {ExceptionMessage}, Time of occurrence {Time}",
            exception.Message, DateTime.UtcNow);

        (string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            ),
            ValidationException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            BadRequestException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            NotFoundException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status404NotFound
            ),
            _ => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            ),
        };

        httpContext.Response.StatusCode = details.StatusCode;

        var problemDetails = new ProblemDetails
        {
            Detail = details.Detail,
            Instance = httpContext.Request.Path,
            Status = details.StatusCode,
            Title = details.Title,
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.ValidationResult.MemberNames);
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}