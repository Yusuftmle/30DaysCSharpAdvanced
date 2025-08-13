// Program.cs - .NET 9 Optimized Version
using System.Reflection;
using Day06.API.Context.cs;
using Day06.API.Repository;
using Day06.API.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = WebApplication.CreateBuilder(args);

// 🧠 SEMANTIC KERNEL SETUP - Current API (v1.0+)
builder.Services.AddKernel();

// Add OpenAI connector
builder.Services.AddOpenAIChatCompletion(
    modelId: "gpt-3.5-turbo", // or "gpt-4"
    apiKey: builder.Configuration["OpenAI:ApiKey"] ?? "YOUR_API_KEY"
);

// 🎯 .NET 9 - AddOpenApi() daha performanslı
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// 🎨 SWAGGER CONFIGURATION (.NET 9 Style)
builder.Services.ConfigureSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Semantic Kernel Prompt Chaining API",
        Version = "v1",
        Description = "Sequential, Conditional, and Parallel Prompt Chaining Examples"
    });
});
builder.Services.AddDbContext<BlogPostContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
// Register our services
var assm = Assembly.GetExecutingAssembly();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assm));
builder.Services.AddScoped<IBlogPostEntityRepository, BlogPostEntityRepository>();

builder.Services.AddScoped<IOutlineRepository, OutlineRepository>();
builder.Services.AddScoped<IContentBlockRepository, ContentBlockRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();


// 🌐 CORS - .NET 9 improved syntax
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// 📝 Logging - .NET 9 built-in improvements
builder.Logging.ClearProviders();

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var app = builder.Build();

// 🎭 .NET 9 - Simplified middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();  // .NET 9 yeni yöntem
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Prompt Chaining API v1");  // .NET 9 endpoint
        c.RoutePrefix = string.Empty;
        c.DefaultModelsExpandDepth(-1);
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.DisplayRequestDuration();
    });
}

// 🚀 Middleware Pipeline - .NET 9 order
app.UseCors();  // Default policy kullanır
app.MapControllers();

// 📊 Startup messages
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("🚀 Semantic Kernel Prompt Chaining API (.NET 9) is starting...");
logger.LogInformation("📖 Swagger UI: https://localhost:5001");
logger.LogInformation("🔗 Blog Chain: POST /api/blogchain/generate");

app.Run();