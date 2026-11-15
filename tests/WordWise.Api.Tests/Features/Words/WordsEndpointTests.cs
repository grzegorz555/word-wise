
using System.Net;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WordWise.Api.Tests.Features.Words;

public sealed class WordsEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetCards_MissingBase_Returns400()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync(new Uri("api/words/cards?target=pl", UriKind.Relative), TestContext.Current.CancellationToken);
        var responseContent = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        responseContent.ShouldNotBeNull();
        responseContent.Errors.ShouldContainKey("base");
    }

    [Fact]
    public async Task GetCards_SameBaseAndTarget_Returns400()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync(new Uri("api/words/cards?base=pl&target=pl", UriKind.Relative), TestContext.Current.CancellationToken);
        var responseContent = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        responseContent.ShouldNotBeNull();
        responseContent.Errors.ShouldContainKey("target");
    }

    [Fact]
    public async Task GetCards_UnsupportedBase_Returns400()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync(new Uri("api/words/cards?base=xx&target=pl", UriKind.Relative), TestContext.Current.CancellationToken);
        var responseContent = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        responseContent.ShouldNotBeNull();
        responseContent.Errors.ShouldContainKey("base");
    }
}
