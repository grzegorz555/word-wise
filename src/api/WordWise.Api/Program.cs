using Microsoft.EntityFrameworkCore;

using WordWise.Api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Services (Dependency Injection container).
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection
builder.Services.AddControllers();

// EF Core + PostgreSQL (ADR-0003). AddDbContext registers the context as scoped (one per request).
// The connection string comes from configuration ("ConnectionStrings:WordWise"), never from code.
// The lambda runs when the context is first needed, so a missing value fails there, not at startup.
// Docs: https://learn.microsoft.com/ef/core/dbcontext-configuration/#dbcontext-in-dependency-injection-for-aspnet-core
builder.Services.AddDbContext<WordWiseDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("WordWise")
            ?? throw new InvalidOperationException("Connection string 'WordWise' not found.")));

builder.Services.AddOpenApi(); // https://aka.ms/aspnet/openapi
builder.Services.AddHealthChecks(); // https://learn.microsoft.com/aspnet/core/host-and-deploy/health-checks

// Problem details service (RFC 9457): error responses get an application/problem+json body.
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/error-handling-api#problem-details
builder.Services.AddProblemDetails();

var app = builder.Build();

// HTTP request pipeline (middleware order matters).
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/middleware

// Fail safely (docs/conventions/security.md). Error handling comes first, so it wraps everything after it.
// - Outside Development, UseExceptionHandler catches an unhandled exception and returns a generic 500
//   ProblemDetails (type, title, status, traceId): no exception message, no stack trace.
// - In Development, WebApplication adds the developer exception page itself. With AddProblemDetails it
//   answers a non-HTML Accept header with ProblemDetails too, but including the stack trace and headers.
// - UseStatusCodePages fills error responses that have no body yet, e.g. 404 for an unknown route.
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/error-handling#problem-details
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

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
