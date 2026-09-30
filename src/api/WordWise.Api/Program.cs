var builder = WebApplication.CreateBuilder(args);

// Services (Dependency Injection container).
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // https://aka.ms/aspnet/openapi
builder.Services.AddHealthChecks(); // https://learn.microsoft.com/aspnet/core/host-and-deploy/health-checks

var app = builder.Build();

// HTTP request pipeline (middleware order matters).
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // JSON document: /openapi/v1.json

    // Swagger UI on top of the built-in document: /swagger (ADR-0009)
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "WordWise API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
