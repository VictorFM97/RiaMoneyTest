using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace API.Middleware;

public class GlobalRequestHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalRequestHandlingMiddleware> _logger;

    public GlobalRequestHandlingMiddleware(ILogger<GlobalRequestHandlingMiddleware> logger) =>
        _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            _logger.LogInformation(
                $"Starting request {context.Request.Path}, method {context.Request.Method}");

            await next(context);

            _logger.LogInformation(
                $"Completed request {context.Request.Path}, method {context.Request.Method}, Status Code {context.Response.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Request failed, {context.Request.Path}, method {context.Request.Method}, error message: {ex.Message}");

            var errorStatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.StatusCode = errorStatusCode;
            context.Response.ContentType = "application/json";

            var problem = new List<string>
            {
                $"Error: {ex.Message}"
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
