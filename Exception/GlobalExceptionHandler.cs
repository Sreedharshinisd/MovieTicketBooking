using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Exceptions;

namespace MovieTicketBooking.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "An exception occurred: {Message}",
            exception.Message);

        var (statusCode, title) = exception switch
        {
            ForbiddenAccessException =>
                (StatusCodes.Status403Forbidden, "Forbidden"),

            UnauthorizedAccessException =>
                (StatusCodes.Status401Unauthorized, "Unauthorized"),

            ArgumentException =>
                (StatusCodes.Status400BadRequest, "Bad Request"),

            KeyNotFoundException =>
                (StatusCodes.Status404NotFound, "Not Found"),

            InvalidOperationException =>
                (StatusCodes.Status409Conflict, "Conflict"),

            _ =>
                (StatusCodes.Status500InternalServerError,
                 "Internal Server Error")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred."
                : exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}