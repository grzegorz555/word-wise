using System.Net;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using WordWise.Api.Tests.TestSupport;

namespace WordWise.Api.Tests;

// "Fail safely" (docs/conventions/security.md): errors are ProblemDetails, and outside Development
// they carry no exception message or stack trace.
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/error-handling-api#problem-details
public sealed class ErrorHandlingTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private const string ProblemJson = "application/problem+json";

    [Fact]
    public async Task Get_UnhandledExceptionInProduction_ReturnsProblemDetailsWithoutInternals()
    {
        using var client = CreateClient("Production");

        var response = await client.GetAsync(
            new Uri(ThrowingTestController.Path, UriKind.Relative), TestContext.Current.CancellationToken);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        MediaTypeOf(response).ShouldBe(ProblemJson);
        body.ShouldNotContain(ThrowingTestController.ExceptionMessage);
        body.ShouldNotContain(nameof(ThrowingTestController)); // a stack trace would name the throwing type
    }

    [Fact]
    public async Task Get_UnhandledExceptionInDevelopment_ReturnsProblemDetailsWithExceptionDetails()
    {
        using var client = CreateClient("Development");

        var response = await client.GetAsync(
            new Uri(ThrowingTestController.Path, UriKind.Relative), TestContext.Current.CancellationToken);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        // The developer exception page answers a non-HTML Accept header with ProblemDetails + details.
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        MediaTypeOf(response).ShouldBe(ProblemJson);
        body.ShouldContain(ThrowingTestController.ExceptionMessage);
    }

    [Fact]
    public async Task Get_UnknownRoute_ReturnsNotFoundProblemDetails()
    {
        using var client = CreateClient("Production");

        var response = await client.GetAsync(
            new Uri("/api/does-not-exist", UriKind.Relative), TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        MediaTypeOf(response).ShouldBe(ProblemJson);
    }

    // WithWebHostBuilder creates a derived factory for this test only; the class fixture disposes it.
    // Docs: https://learn.microsoft.com/aspnet/core/test/integration-tests#inject-mock-services
    private HttpClient CreateClient(string environment) =>
        factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment(environment);
            builder.ConfigureTestServices(services =>
                services.AddControllers().AddApplicationPart(typeof(ThrowingTestController).Assembly));
        }).CreateClient();

    // A separate call on purpose: in `ContentType?.MediaType.ShouldBe(...)` the `?.` short-circuits the whole
    // chain, so with no Content-Type the assertion would never run and the test would pass.
    // Docs: https://learn.microsoft.com/dotnet/csharp/fundamentals/null-safety/null-operators
    private static string? MediaTypeOf(HttpResponseMessage response) =>
        response.Content.Headers.ContentType?.MediaType;
}
