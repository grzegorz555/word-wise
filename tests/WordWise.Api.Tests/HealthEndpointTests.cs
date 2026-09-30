using System.Net;

using Microsoft.AspNetCore.Mvc.Testing;

namespace WordWise.Api.Tests;

// Integration test: boots the whole API in memory via WebApplicationFactory.
// Docs: https://learn.microsoft.com/aspnet/core/test/integration-tests
public sealed class HealthEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetHealth_ReturnsOk()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync(new Uri("/health", UriKind.Relative), TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
