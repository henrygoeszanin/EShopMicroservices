using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler (ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
            "Error Message: {ExceptionMessage}, Time of ocurrentce {Time}",
            exception.Message, DateTime.UtcNow);

        (string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException =>
            (
            exception.Message,
            exception.GetType().Name,
            context.Response.StatusCode = StatusCodes.Status500InternalServerError
            ),
            ValidationException => (
            exception.Message,
            exception.GetType().Name,
            context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            BadRequestException => (
            exception.Message,
            exception.GetType().Name,
            context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            NotFoundException => (
            exception.Message,
            exception.GetType().Name,
            context.Response.StatusCode = StatusCodes.Status404NotFound
            ),
            _ => (
            exception.Message,
            exception.GetType().Name,
            context.Response.StatusCode = StatusCodes.Status500InternalServerError
            ),
        };

        var problemDetails = new ProblemDetails
        {
            Detail = details.Detail,
            Instance = context.Request.Path,
            Status = details.StatusCode,
            Title = details.Title,
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if(exception is ValidationException validationException)
        {
            // na video aula o validationException é escrito como validationException.Errors
            // mas aqui está dando erro quando coloca-se validationException.Errors
            problemDetails.Extensions.Add("ValidationErrors", validationException);
        }

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}

