using System.Net;
using System.Text.Json;

namespace TaskScheduler.Api.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred");

            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case ArgumentNullException or ArgumentException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var response = new
            {
                error = ex.Message,
                exception = ex.GetType().Name
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
