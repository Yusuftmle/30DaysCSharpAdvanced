using System.Collections.Concurrent;

namespace Day03_CustomMiddleware.Middlewares
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitMiddleware> _logger;

        // IP başına request sayısını tutacak dictionary
        private static readonly ConcurrentDictionary<string, RequestInfo> _requests = new();
        private const int MaxRequests = 10; // 1 dakikada maksimum 10 request
        private static readonly TimeSpan TimeWindow = TimeSpan.FromMinutes(1);

        public RateLimitMiddleware(RequestDelegate next, ILogger<RateLimitMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIP = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (IsRateLimited(clientIP))
            {
                _logger.LogWarning("Rate limit exceeded for IP: {ClientIP}", clientIP);

                context.Response.StatusCode = 429; // Too Many Requests
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Rate limit exceeded",
                    message = $"Maximum {MaxRequests} requests per minute allowed",
                    retryAfter = 60,
                    timestamp = DateTime.Now
                };

                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
                return;
            }

            await _next(context);
        }

        private static bool IsRateLimited(string clientIP)
        {
            var now = DateTime.UtcNow;

            _requests.AddOrUpdate(clientIP,
                new RequestInfo { Count = 1, WindowStart = now },
                (key, existing) =>
                {
                    // Eğer zaman penceresi geçtiyse sıfırla
                    if (now - existing.WindowStart > TimeWindow)
                    {
                        return new RequestInfo { Count = 1, WindowStart = now };
                    }

                    // Request sayısını artır
                    existing.Count++;
                    return existing;
                });

            return _requests[clientIP].Count > MaxRequests;
        }

        private class RequestInfo
        {
            public int Count { get; set; }
            public DateTime WindowStart { get; set; }
        }
    }
}
