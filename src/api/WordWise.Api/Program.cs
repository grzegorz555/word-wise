using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

using WordWise.Api.Features.Words;
using WordWise.Api.Infrastructure.Data;
using WordWise.Api.Infrastructure.OpenApi;
using WordWise.Api.Infrastructure.Routing;

var builder = WebApplication.CreateBuilder(args);

// Services (Dependency Injection container).
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection
// JSON numbers only as numbers (JsonNumberHandling.Strict), not also as strings ("17"), which is the
// ASP.NET Core default (AllowReadingFromString). Set twice, because there are two sets of JSON options:
// MVC options (AddJsonOptions) for what controllers read and write at runtime, and the HTTP JSON options
// (ConfigureHttpJsonOptions) that the OpenAPI generator reads. Then the contract says `"type": "integer"`
// and the runtime agrees with it.
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/include-metadata#numeric-types
//   https://learn.microsoft.com/aspnet/core/fundamentals/openapi/include-metadata#set-json-serialization-options-globally
// The route convention makes [controller] lowercase kebab-case: WordsController → /api/words.
// Docs: https://learn.microsoft.com/aspnet/core/mvc/controllers/routing#use-a-parameter-transformer-to-customize-token-replacement
builder.Services.AddControllers(options =>
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())))
    .AddJsonOptions(options => options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict);
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict);

// EF Core + PostgreSQL (ADR-0003). AddDbContext registers the context as scoped (one per request).
// The connection string comes from configuration ("ConnectionStrings:WordWise"), never from code.
// The lambda runs when the context is first needed, so a missing value fails there, not at startup.
// Docs: https://learn.microsoft.com/ef/core/dbcontext-configuration/#dbcontext-in-dependency-injection-for-aspnet-core
builder.Services.AddDbContext<WordWiseDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("WordWise")
            ?? throw new InvalidOperationException("Connection string 'WordWise' not found.")));

// Feature services (ADR-0002). Scoped, like the DbContext they use: one instance per request.
builder.Services.AddScoped<WordCardQueries>();

// OpenAPI document (https://aka.ms/aspnet/openapi). The schema transformer adds `enum` for [AllowedValues].
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/customize-openapi#use-schema-transformers
builder.Services.AddOpenApi(options => options.AddSchemaTransformer<AllowedValuesSchemaTransformer>());
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
