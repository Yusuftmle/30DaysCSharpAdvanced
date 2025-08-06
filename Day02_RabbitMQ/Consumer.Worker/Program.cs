

// Program.cs - Bu dosya olmadan hiçbir şey çalışmaz!
var builder = Host.CreateApplicationBuilder(args);

// Logging ekle (konsola log yazsın)
builder.Services.AddLogging(configure =>
    configure.AddConsole().SetMinimumLevel(LogLevel.Information));

// Worker servisini ekle - İŞTE BURASI ÖNEMLİ!
builder.Services.AddHostedService<Worker>();

// Host'u oluştur ve çalıştır
var host = builder.Build();

Console.WriteLine("🚀 Worker servisi başlatılıyor...");
Console.WriteLine("⏹️  Durdurmak için Ctrl+C basın");

// Worker burada otomatik başlar!
await host.RunAsync();

Console.WriteLine("👋 Worker servisi durduruldu.");

