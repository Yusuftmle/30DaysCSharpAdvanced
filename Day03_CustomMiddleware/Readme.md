# Day03 - ASP.NET Core Custom Middleware

Bu proje, ASP.NET Core'da custom middleware yazma, middleware pipeline'ını anlama ve gerçek dünya senaryolarında kullanma konularını kapsar.

## 📋 İçindekiler

- [Middleware Nedir?](#middleware-nedir)
- [Middleware Pipeline](#middleware-pipeline)
- [Proje Yapısı](#proje-yapısı)
- [Middleware Türleri](#middleware-türleri)
- [Kullanım Alanları](#kullanım-alanları)
- [Kurulum ve Çalıştırma](#kurulum-ve-çalıştırma)
- [Test Senaryoları](#test-senaryoları)
- [Best Practices](#best-practices)
- [Gerçek Dünya Örnekleri](#gerçek-dünya-örnekleri)

## 🔧 Middleware Nedir?

Middleware, ASP.NET Core'da **HTTP request/response pipeline**'ında yer alan yazılım bileşenleridir. Her middleware:

- Gelen HTTP isteğini işleyebilir
- Pipeline'daki bir sonraki middleware'e isteği iletebilir  
- Response dönerken de işlem yapabilir
- Pipeline'ı durdurabilir (short-circuit)

### 🎯 Temel Mantık:
```
Request  → MW1 → MW2 → MW3 → Controller → Response
         ↓    ↓    ↓              ↑
         ←    ←    ← ← ← ← ← ← ← ← ←
```

## 🔄 Middleware Pipeline

Pipeline'da **sıralama kritik**tir! Middleware'ler eklendiği sırayla çalışır.

### 📊 Standart Pipeline Sırası:
```csharp
app.UseExceptionHandler();      // 1. Exception handling (en üstte)
app.UseHttpsRedirection();      // 2. HTTPS yönlendirme
app.UseStaticFiles();           // 3. Static dosyalar  
app.UseRouting();               // 4. Routing
app.UseAuthentication();        // 5. Kimlik doğrulama
app.UseAuthorization();         // 6. Yetkilendirme
app.MapControllers();           // 7. Endpoint routing
```

### ⚠️ Neden Sıralama Önemli?

```csharp
// YANLIŞ - Exception middleware en altta
app.UseAuthentication();
app.UseExceptionHandler();  // Exception'ları yakalayamaz!

// DOĞRU - Exception middleware en üstte  
app.UseExceptionHandler();  // Tüm exception'ları yakalar
app.UseAuthentication();
```

## 📁 Proje Yapısı

```
Day03_CustomMiddleware/
│
├── Program.cs                          # Pipeline konfigürasyonu
├── Controllers/
│   └── TestController.cs               # Test endpoint'leri
├── Middlewares/
│   ├── RequestTimingMiddleware.cs      # Performance monitoring
│   ├── ExceptionHandlingMiddleware.cs  # Global error handling
│   ├── RequestLoggingMiddleware.cs     # Request/Response logging
│   └── RateLimitMiddleware.cs          # API rate limiting
└── Models/
    └── TestModels.cs                   # Test için modeller
```

## 🛠️ Middleware Türleri

### 1. **Request Timing Middleware** 
**Amaç:** API endpoint'lerinin response sürelerini ölçme

```csharp
public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        var watch = Stopwatch.StartNew();
        await _next(context);  // Sonraki middleware'e geç
        watch.Stop();
        
        _logger.LogInformation("Request for {Path} took {ElapsedMs}ms",
            context.Request.Path, watch.ElapsedMilliseconds);
    }
}
```

**Kullanım Alanları:**
- Performance monitoring
- Slow query detection  
- API response time analytics
- SLA compliance tracking

### 2. **Exception Handling Middleware**
**Amaç:** Uygulama genelinde exception'ları yakala ve kullanıcı dostu response döndür

```csharp
public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await _next(context);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception occurred");
        await HandleExceptionAsync(context, ex);
    }
}
```

**Kullanım Alanları:**
- Global error handling
- Structured error responses
- Error logging ve monitoring
- Security (error detail hiding)

### 3. **Request Logging Middleware**
**Amaç:** Gelen istekleri ve dönen response'ları detaylı logla

**Kullanım Alanları:**
- Audit trail
- Debugging ve troubleshooting
- API usage analytics
- Security monitoring

### 4. **Rate Limiting Middleware**
**Amaç:** API'ye yapılan istekleri sınırla (DDoS protection)

**Kullanım Alanları:**
- API güvenliği
- Resource protection
- Fair usage policy
- Cost control (external API calls)

## 🌍 Kullanım Alanları

### **E-Commerce Uygulaması:**
```csharp
app.UseMiddleware<SecurityHeadersMiddleware>();    // Security headers
app.UseMiddleware<RateLimitMiddleware>();          // DDoS protection  
app.UseMiddleware<RequestLoggingMiddleware>();     // Audit logging
app.UseMiddleware<CacheMiddleware>();              // Response caching
app.UseMiddleware<CompressionMiddleware>();        // Response compression
```

### **Banking API:**
```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();  // Error handling
app.UseMiddleware<AuditMiddleware>();              // Financial audit
app.UseMiddleware<FraudDetectionMiddleware>();     // Fraud protection
app.UseMiddleware<EncryptionMiddleware>();         // Data encryption
app.UseMiddleware<SessionTimeoutMiddleware>();     // Session management
```

### **Microservice Gateway:**
```csharp
app.UseMiddleware<LoadBalancingMiddleware>();      // Load balancing
app.UseMiddleware<CircuitBreakerMiddleware>();     // Circuit breaker pattern
app.UseMiddleware<RetryMiddleware>();              // Retry logic
app.UseMiddleware<ServiceDiscoveryMiddleware>();   // Service discovery
```

## 🚀 Kurulum ve Çalıştırma

### **Gereksinimler:**
- .NET 6.0 veya üzeri
- Visual Studio 2022 / VS Code

### **Adım 1: Projeyi Klonla**
```bash
git clone [repository-url]
cd Day03_CustomMiddleware
```

### **Adım 2: Bağımlılıkları Yükle**
```bash
dotnet restore
```

### **Adım 3: Çalıştır**
```bash
dotnet run
```

### **Adım 4: Test Et**
```
Swagger UI: https://localhost:5001/swagger
```

## 🧪 Test Senaryoları

### **1. Performance Testing**
```bash
# Hızlı endpoint
GET /api/fast
Response: ~10ms

# Yavaş endpoint  
GET /api/slow
Response: ~2000ms

# Load test
GET /api/test/load/100
Response: ~1000ms
```

### **2. Exception Handling**
```bash
# Genel exception
GET /api/error
Response: 500 Internal Server Error

# Null exception
GET /api/test/exception/null  
Response: 400 Bad Request

# Timeout exception
GET /api/test/exception/timeout
Response: 408 Request Timeout
```

### **3. Rate Limiting**
```bash
# 10 istek/dakika limiti
for i in {1..12}; do
  curl http://localhost:5000/api/test-rate
done

# 11. istekten sonra:
Response: 429 Too Many Requests
```

### **4. Request Logging**
```bash
POST /api/data
{
  "name": "John Doe",
  "age": 30,
  "email": "john@example.com"
}

# Console'da göreceksin:
# [INFO] Incoming Request: POST /api/data from 127.0.0.1
# [INFO] Response: 200 for POST /api/data
```

## 💡 Best Practices

### **1. Middleware Sıralaması**
```csharp
// En kritik middleware'ler en üstte
app.UseExceptionHandler();     // 1. Exception handling
app.UseHttpsRedirection();     // 2. Security  
app.UseAuthentication();       // 3. Auth
app.UseCustomMiddleware();     // 4. Custom logic
app.MapControllers();          // 5. Routing (en altta)
```

### **2. Performance Considerations**
```csharp
public async Task InvokeAsync(HttpContext context)
{
    // ❌ Blocking operation
    Thread.Sleep(1000);
    
    // ✅ Async operation
    await Task.Delay(1000);
    
    await _next(context);
}
```

### **3. Exception Safety**
```csharp
public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await _next(context);
    }
    catch (Exception ex) when (!(ex is OutOfMemoryException))
    {
        // Handle specific exceptions, let critical ones bubble up
        _logger.LogError(ex, "Middleware error");
        throw;
    }
}
```

### **4. Resource Management**
```csharp
public async Task InvokeAsync(HttpContext context)
{
    using var activity = ActivitySource.StartActivity("CustomMiddleware");
    
    // Dispose edilen kaynaklarda using kullan
    await _next(context);
}
```

## 🏢 Gerçek Dünya Örnekleri

### **1. Netflix - Content Delivery**
```csharp
app.UseMiddleware<GeoLocationMiddleware>();        // Kullanıcı konumu
app.UseMiddleware<ContentFilteringMiddleware>();   // İçerik filtreleme
app.UseMiddleware<BandwidthDetectionMiddleware>(); // Bağlantı hızı
app.UseMiddleware<CDNRoutingMiddleware>();          // CDN yönlendirme
```

### **2. Amazon - E-Commerce**
```csharp
app.UseMiddleware<SecurityScanningMiddleware>();   // Güvenlik tarama
app.UseMiddleware<PersonalizationMiddleware>();    // Kişiselleştirme
app.UseMiddleware<InventoryCheckMiddleware>();     // Stok kontrolü
app.UseMiddleware<PricingMiddleware>();            // Dinamik fiyatlama
```

### **3. GitHub - Code Repository**
```csharp
app.UseMiddleware<GitHooksMiddleware>();           // Git hook işlemleri
app.UseMiddleware<SecurityScanMiddleware>();       // Kod güvenlik tarama
app.UseMiddleware<LicenseValidationMiddleware>();  // Lisans kontrolü
app.UseMiddleware<CollaborationMiddleware>();      // İşbirliği özellikleri
```

### **4. Slack - Team Communication**
```csharp
app.UseMiddleware<MessageFilteringMiddleware>();   // Mesaj filtreleme
app.UseMiddleware<NotificationMiddleware>();       // Bildirim sistemi
app.UseMiddleware<PresenceMiddleware>();           // Kullanıcı durumu
app.UseMiddleware<ThreadingMiddleware>();          // Mesaj thread'leri
```

## 🔒 Security Middleware Örnekleri

### **CORS Middleware**
```csharp
public class CorsMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
        
        if (context.Request.Method == "OPTIONS")
        {
            context.Response.StatusCode = 200;
            return;
        }
        
        await _next(context);
    }
}
```

### **API Key Validation**
```csharp
public class ApiKeyMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("X-API-Key"))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key missing");
            return;
        }
        
        var apiKey = context.Request.Headers["X-API-Key"];
        if (!IsValidApiKey(apiKey))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }
        
        await _next(context);
    }
}
```

## 📊 Monitoring ve Analytics

### **Custom Metrics**
```csharp
public class MetricsMiddleware
{
    private static readonly Counter RequestCounter = 
        Metrics.CreateCounter("http_requests_total", "Total HTTP requests");
        
    public async Task InvokeAsync(HttpContext context)
    {
        RequestCounter.Inc();
        
        using var timer = Metrics.CreateHistogram("http_request_duration")
            .NewTimer();
            
        await _next(context);
    }
}
```

## 🎯 Sonuç

Middleware'ler, modern web uygulamalarının omurgasıdır. Bu proje ile:

✅ **Middleware pipeline'ının nasıl çalıştığını öğrendik**  
✅ **4 farklı middleware türü geliştirdik**  
✅ **Gerçek dünya senaryolarında nasıl kullanıldığını gördük**  
✅ **Performance, security ve monitoring konularını ele aldık**  
✅ **Test edilebilir bir yapı kurduk**

### 🚀 Sonraki Adımlar:
- [ ] Custom middleware'leri genişlet
- [ ] Health check middleware ekle  
- [ ] JWT authentication middleware yaz
- [ ] Caching middleware implement et
- [ ] Message queue middleware entegrasyonu

---

**30 Days Challenge - Day 3** 🎯  
*ASP.NET Core Middleware ile cross-cutting concerns'leri ele alıyoruz!*

## 📚 Kaynaklar

- [Microsoft Docs - ASP.NET Core Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/)
- [Middleware Best Practices](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write)
- [Performance Testing Tools](https://docs.microsoft.com/en-us/aspnet/core/test/load-tests)

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-middleware`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add amazing middleware'`)
4. Branch'i push edin (`git push origin feature/amazing-middleware`)
5. Pull Request oluşturun

