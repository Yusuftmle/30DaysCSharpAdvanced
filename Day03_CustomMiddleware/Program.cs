using Day03_CustomMiddleware.Middlewares;
using Day03_CustomMiddleware.Models;

var builder = WebApplication.CreateBuilder(args);

// Servisleri ekle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Development environment ayarlar�
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// *** MIDDLEWARE PIPELINE SIRASI �NEML�! ***
// En �stten ba�layarak her middleware s�rayla �al���r

// 1. Exception Handling (en �stte olmal�)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 2. Request Timing (performans �l��m�)
app.UseMiddleware<RequestTimingMiddleware>();

// 3. Request Logging (gelen istekleri logla)  
app.UseMiddleware<RequestLoggingMiddleware>();

// 4. Rate Limiting (g�venlik)
app.UseMiddleware<RateLimitMiddleware>();

// 5. Built-in middleware'ler
app.UseHttpsRedirection();
app.UseAuthorization();

// 6. Controllers (son ad�m)
app.MapControllers();

// Test endpoint'leri ekleme
MapTestEndpoints(app);

app.Run();
// Test endpoint'lerini tan�mlama
static void MapTestEndpoints(WebApplication app)
{
    // H�zl� response test etmek i�in
    app.MapGet("/api/fast", () =>
    {
        return Results.Ok(new { message = "H�zl� response!", timestamp = DateTime.Now });
    });

    // Yava� response test etmek i�in
    app.MapGet("/api/slow", async () =>
    {
        await Task.Delay(2000); // 2 saniye bekle
        return Results.Ok(new { message = "Yava� response!", timestamp = DateTime.Now });
    });

    // Hata f�rlatma test etmek i�in
    app.MapGet("/api/error", () =>
    {
        throw new Exception("Test exception - middleware yakalayacak!");
    });

    // Rate limit test etmek i�in
    app.MapGet("/api/test-rate", () =>
    {
        return Results.Ok(new { message = "Rate limit test!", count = Random.Shared.Next(1, 100) });
    });

    // JSON response test etmek i�in
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