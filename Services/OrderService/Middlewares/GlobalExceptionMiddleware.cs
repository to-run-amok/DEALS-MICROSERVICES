using System.Text.Json;

public static class MiddleExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
public class GlobalExceptionMiddleware
{
    
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        catch(UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex,ex.Message);

            await HandleExceptionAsync(
                context,
                StatusCodes.Status401Unauthorized,
                ex.Message
            );
        }
         catch(KeyNotFoundException ex)
        {
            _logger.LogWarning(ex,ex.Message);

            await HandleExceptionAsync(
                context,
                StatusCodes.Status404NotFound,
                ex.Message
            );
        }
        catch(InvalidOperationException ex)
        {
            _logger.LogWarning(ex,ex.Message);

            await HandleExceptionAsync(
                context,
                StatusCodes.Status400BadRequest,
                ex.Message
            );
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex,"An Unhandled exception occurred.");

            await HandleExceptionAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred."
            );
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            statusCode,
            message
        };

        var jsonResponse = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(jsonResponse);
    }
    
    
}