using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using WordWise.Api.Domain;
using WordWise.Api.Infrastructure.Data;
using WordWise.Api.Infrastructure.Data.Seed;

namespace WordWise.Api.Tests.Infrastructure.Data.Seed;

// Integration tests against the shared test database (ADR-0013): idempotency is about what is already in the
// database, so an in-memory fake wouldn't prove it. Tests in one collection run one at a time, so each test
// can start from an empty catalog.
[Collection(PostgresCollectionDefinition.Name)]
public sealed class WordSeedImporterTests(PostgresApiFactory factory)
{
    private static readonly WordSeed Be = new("be", "verb",
    [
        new(LanguageCodes.English, ["be"], 1),
        new(LanguageCodes.Polish, ["być"], 1),
        new(LanguageCodes.Turkish, ["olmak"], 1),
    ]);

    private static readonly WordSeed Say = new("say", "verb",
    [
        new(LanguageCodes.English, ["say"], 3),
        new(LanguageCodes.Polish, ["mówić", "powiedzieć"], 3),
        new(LanguageCodes.Turkish, ["demek", "söylemek"], null),
    ]);

    [Fact]
    public async Task ImportAsync_EmptyCatalog_AddsConceptsAndOneWordPerForm()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var scope = factory.Services.CreateAsyncScope();
        var db = await GetEmptyCatalogAsync(scope, cancellationToken);

        var added = await CreateImporter(scope).ImportAsync([Be, Say], cancellationToken);

        added.ShouldBe(2);
        (await db.Concepts.CountAsync(cancellationToken)).ShouldBe(2);
        // be: 3 words; say: en 1 + pl 2 + tr 2 = 5 words.
        (await db.Words.CountAsync(cancellationToken)).ShouldBe(8);
    }

    [Fact]
    public async Task ImportAsync_Synonyms_SavesEachFormAsWordOfSameConceptWithSameRank()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var scope = factory.Services.CreateAsyncScope();
        var db = await GetEmptyCatalogAsync(scope, cancellationToken);

        await CreateImporter(scope).ImportAsync([Say], cancellationToken);

        var concept = await db.Concepts.AsNoTracking().Include(c => c.Words).SingleAsync(cancellationToken);
        concept.Key.ShouldBe("say");
        concept.PartOfSpeech.ShouldBe("verb");

        var polish = concept.Words.Where(w => w.Language == LanguageCodes.Polish).ToList();
        polish.Select(w => w.Text).ShouldBe(["mówić", "powiedzieć"], ignoreOrder: true);
        polish.ShouldAllBe(w => w.FrequencyRank == 3);

        // An empty rank in the CSV stays null in the database, with Turkish characters intact.
        var turkish = concept.Words.Where(w => w.Language == LanguageCodes.Turkish).ToList();
        turkish.Select(w => w.Text).ShouldBe(["demek", "söylemek"], ignoreOrder: true);
        turkish.ShouldAllBe(w => w.FrequencyRank == null);
    }

    [Fact]
    public async Task ImportAsync_Twice_KeepsSameRowCounts()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var scope = factory.Services.CreateAsyncScope();
        var db = await GetEmptyCatalogAsync(scope, cancellationToken);
        await CreateImporter(scope).ImportAsync([Be, Say], cancellationToken);
        var wordsAfterFirstImport = await db.Words.CountAsync(cancellationToken);

        // A new scope = a new DbContext, like the next start of the app.
        await using var secondScope = factory.Services.CreateAsyncScope();
        var addedSecondTime = await CreateImporter(secondScope).ImportAsync([Be, Say], cancellationToken);

        addedSecondTime.ShouldBe(0);
        (await db.Concepts.CountAsync(cancellationToken)).ShouldBe(2);
        (await db.Words.CountAsync(cancellationToken)).ShouldBe(wordsAfterFirstImport);
    }

    [Fact]
    public async Task ImportAsync_UnsupportedLanguage_ThrowsAndSavesNothing()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var scope = factory.Services.CreateAsyncScope();
        var db = await GetEmptyCatalogAsync(scope, cancellationToken);
        var german = new WordSeed("house", "noun",
        [
            new(LanguageCodes.English, ["house"], 20),
            new("de", ["Haus"], 20),
        ]);

        var exception = await Should.ThrowAsync<InvalidDataException>(
            () => CreateImporter(scope).ImportAsync([Be, german], cancellationToken));

        exception.Message.ShouldContain("house");
        exception.Message.ShouldContain("de");
        (await db.Concepts.CountAsync(cancellationToken)).ShouldBe(0);
    }

    private static WordSeedImporter CreateImporter(AsyncServiceScope scope) =>
        scope.ServiceProvider.GetRequiredService<WordSeedImporter>();

    // ExecuteDeleteAsync sends one DELETE per table, without loading rows. Words first: they reference Concepts.
    // Docs: https://learn.microsoft.com/ef/core/saving/execute-insert-update-delete
    private static async Task<WordWiseDbContext> GetEmptyCatalogAsync(
        AsyncServiceScope scope, CancellationToken cancellationToken)
    {
        var db = scope.ServiceProvider.GetRequiredService<WordWiseDbContext>();
        await db.Words.ExecuteDeleteAsync(cancellationToken);
        await db.Concepts.ExecuteDeleteAsync(cancellationToken);
        return db;
    }
}
