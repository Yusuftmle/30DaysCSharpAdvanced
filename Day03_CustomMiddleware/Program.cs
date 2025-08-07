using Day03_CustomMiddleware.Middlewares;
using Day03_CustomMiddleware.Models;

var builder = WebApplication.CreateBuilder(args);

// Servisleri ekle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Development environment ayarlarý
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// *** MIDDLEWARE PIPELINE SIRASI ÖNEMLÝ! ***
// En üstten baþlayarak her middleware sýrayla çalýþýr

// 1. Exception Handling (en üstte olmalý)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 2. Request Timing (performans ölçümü)
app.UseMiddleware<RequestTimingMiddleware>();

// 3. Request Logging (gelen istekleri logla)  
app.UseMiddleware<RequestLoggingMiddleware>();

// 4. Rate Limiting (güvenlik)
app.UseMiddleware<RateLimitMiddleware>();

// 5. Built-in middleware'ler
app.UseHttpsRedirection();
app.UseAuthorization();

// 6. Controllers (son adým)
app.MapControllers();

// Test endpoint'leri ekleme
MapTestEndpoints(app);

app.Run();
// Test endpoint'lerini tanýmlama
static void MapTestEndpoints(WebApplication app)
{
    // Hýzlý response test etmek için
    app.MapGet("/api/fast", () =>
    {
        return Results.Ok(new { message = "Hýzlý response!", timestamp = DateTime.Now });
    });

    // Yavaþ response test etmek için
    app.MapGet("/api/slow", async () =>
    {
        await Task.Delay(2000); // 2 saniye bekle
        return Results.Ok(new { message = "Yavaþ response!", timestamp = DateTime.Now });
    });

    // Hata fýrlatma test etmek için
    app.MapGet("/api/error", () =>
    {
        throw new Exception("Test exception - middleware yakalayacak!");
    });

    // Rate limit test etmek için
    app.MapGet("/api/test-rate", () =>
    {
        return Results.Ok(new { message = "Rate limit test!", count = Random.Shared.Next(1, 100) });
    });

    // JSON response test etmek için
    app.MapPost("/api/data", (TestModel model) =>
    {
        return Results.Ok(new
        {
            received = model,
            processed = DateTime.Now,
            status = "success"
        });
    });
}