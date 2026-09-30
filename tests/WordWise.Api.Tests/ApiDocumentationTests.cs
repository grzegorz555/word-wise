using System.Net;

using Microsoft.AspNetCore.Mvc.Testing;

namespace WordWise.Api.Tests;

// WebApplicationFactory runs the app in the "Development" environment by default,
// so the OpenAPI document and Swagger UI are available here.
public sealed class ApiDocumentationTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Theory]
    [InlineData("/openapi/v1.json")]
    [InlineData("/swagger/index.html")]
    public async Task Get_DocumentationEndpoint_ReturnsOk(string path)
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync(new Uri(path, UriKind.Relative), TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
