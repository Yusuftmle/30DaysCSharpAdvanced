namespace Day03_CustomMiddleware.Middlewares
{
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
                _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                ArgumentNullException => (400, "Bad Request - Null parameter"),
                InvalidOperationException => (400, "Bad Request - Invalid operation"),
                TimeoutException => (408, "Request Timeout"),
                _ => (500, "Internal Server Error")
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                error = message,
                details = exception.Message,
                timestamp = DateTime.Now,
                path = context.Request.Path.Value
            };

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));

        }
    }
}
