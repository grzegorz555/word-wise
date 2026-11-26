using System.Net;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using WordWise.Api.Domain;
using WordWise.Api.Features.Words;
using WordWise.Api.Infrastructure.Data;
using WordWise.Api.Tests.Infrastructure;

namespace WordWise.Api.Tests.Features.Words;

[Collection(PostgresCollectionDefinition.Name)]
public sealed class WordsEndpointTests(PostgresApiFactory factory)
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

    [Fact]
    public async Task GetCards_ThreeRankedWords_ReturnsCardsSortedByRank()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var scope = factory.Services.CreateAsyncScope();
        var db = await GetEmptyCatalogAsync(scope, ct);

        db.Concepts.AddRange(
            new Concept
            {
                Key = "be",
                PartOfSpeech = "verb",
                Words = {
            new Word { Language = LanguageCodes.Polish, Text = "być", FrequencyRank = 8 },
            new Word { Language = LanguageCodes.English, Text = "be", FrequencyRank = 8 },
            new Word { Language = LanguageCodes.Turkish, Text = "olmak", FrequencyRank = 8 }
            }
            },
            new Concept
            {
                Key = "do",
                PartOfSpeech = "verb",
                Words = {
            new Word { Language = LanguageCodes.Polish, Text = "robić", FrequencyRank = 2 },
            new Word { Language = LanguageCodes.English, Text = "do", FrequencyRank = 2 },
            new Word { Language = LanguageCodes.Turkish, Text = "yapmak", FrequencyRank = 2 }
            }
            },
            new Concept
            {
                Key = "see",
                PartOfSpeech = "verb",
                Words = {
            new Word { Language = LanguageCodes.Polish, Text = "widzieć", FrequencyRank = 5 },
            new Word { Language = LanguageCodes.English, Text = "see", FrequencyRank = 5 },
            new Word { Language = LanguageCodes.Turkish, Text = "görmek", FrequencyRank = 5 }
            }
            }
        );
        await db.SaveChangesAsync(ct);

        using var client = factory.CreateClient();

        var response = await client.GetAsync(new Uri("api/words/cards?base=pl&target=en", UriKind.Relative), ct);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<IReadOnlyList<WordCardResponse>>(ct);

        responseContent.ShouldNotBeNull();
        responseContent.Select(c => c.Rank).ShouldBe([2, 5, 8]);
        responseContent.Select(c => c.Text).ShouldBe(["do", "see", "be"]);
    }

    private static async Task<WordWiseDbContext> GetEmptyCatalogAsync(
    AsyncServiceScope scope, CancellationToken cancellationToken)
    {
        var db = scope.ServiceProvider.GetRequiredService<WordWiseDbContext>();
        await db.Words.ExecuteDeleteAsync(cancellationToken);
        await db.Concepts.ExecuteDeleteAsync(cancellationToken);
        return db;
    }
}
