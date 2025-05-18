namespace Middleware.ExceptionHandler;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandler> _logger;
    public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task<HttpResponse> Invoke(HttpContext context) {
        try
        {
            await _next(context);
            return context.Response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EXCEPTION ENCOUNTERED: {Error}", ex.Message);
            return context.Response;
        }
    }
}