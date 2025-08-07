namespace Day03_CustomMiddleware.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Request bilgilerini logla
            _logger.LogInformation(
                "Incoming Request: {Method} {Path} from {RemoteIP}",
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress
            );

            // Request headers (önemli olanları)
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var contentType = context.Request.ContentType;

            if (!string.IsNullOrEmpty(userAgent))
                _logger.LogDebug("User-Agent: {UserAgent}", userAgent);

            if (!string.IsNullOrEmpty(contentType))
                _logger.LogDebug("Content-Type: {ContentType}", contentType);

            await _next(context);

            // Response bilgilerini logla  
            _logger.LogInformation(
                "Response: {StatusCode} for {Method} {Path}",
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path
            );
        }
    }
}
