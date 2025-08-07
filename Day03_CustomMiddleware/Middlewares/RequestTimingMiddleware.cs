using System.Diagnostics;
namespace Day03_CustomMiddleware.Middlewares;
public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;
    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var watch = Stopwatch.StartNew();
        await _next(context); // pass to the next middleware
        watch.Stop();
         var elapsedMs = watch.ElapsedMilliseconds;
        _logger.LogInformation("Request for {Path} took {ElapsedMilliseconds}ms",
            context.Request.Path, elapsedMs);
    }
}