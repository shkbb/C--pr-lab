using System.Net;
using System.Text.Json;

namespace Library.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception has occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        // Default to 500 Internal Server Error
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var resultMessage = "Internal Server Error. Please try again later.";

        // We can map specific exception types to HTTP status codes
        if (exception is ArgumentException || exception is ArgumentNullException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            resultMessage = exception.Message;
        }
        else if (exception is InvalidOperationException)
        {
            // E.g. Business rule violation
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity; // 422
            resultMessage = exception.Message;
        }

        var result = JsonSerializer.Serialize(new
        {
            statusCode = context.Response.StatusCode,
            message = resultMessage,
            detailed = exception.Message // in production, detailed message should be hidden
        });

        return context.Response.WriteAsync(result);
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
