using System.Globalization;

using Microsoft.EntityFrameworkCore;

using WordWise.Api.Domain;
using WordWise.Api.Infrastructure.Data;

namespace WordWise.Api.Features.Words;

// Use-case logic for GET /api/words/cards (ADR-0002: a plain class in DI, no MediatR).
// Registered as scoped in Program.cs, the same lifetime as the WordWiseDbContext it depends on.
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection#service-lifetimes
public sealed class WordQueries(WordWiseDbContext dbContext)
{
    public async Task<IReadOnlyList<WordCardResponse>> GetCardsAsync(
        WordCardsRequest request,
        CancellationToken cancellationToken)
    {
        // Two steps: what the database can do (filter, join, sort by rank), then what only .NET can do
        // (culture-aware sorting, shuffling). Docs: https://learn.microsoft.com/ef/core/querying/client-eval
        var cards = await LoadCardsAsync(request.BaseLanguage, request.TargetLanguage, cancellationToken);

        return SortTranslationsAndApplyOrder(cards, LanguageCodes.GetCulture(request.BaseLanguage), request.Order);
    }

    private Task<List<WordCardResponse>> LoadCardsAsync(
        string baseLanguage,
        string targetLanguage,
        CancellationToken cancellationToken)
    {
        IQueryable<Word> words = dbContext.Words.AsNoTracking();

        // TODO(human) #1: build the EF Core query on `words` and run it with ToListAsync(cancellationToken).
        //   - R2: only target-language words that have a FrequencyRank, ordered by it ascending.
        //   - R3: skip a word when its Concept has no Word in the base language.
        //   - R4: one card per target Word; Translations = texts of the base-language Words of the same Concept.
        //   - Select straight into WordCardResponse (projection: only the needed columns, no tracking).
        //   - Then make the method `async`, `return await ...`, and delete the two lines below.
        // Hint: navigation properties (w.Concept.Words) inside Where/Select become a JOIN or subquery in SQL.
        //   Print the SQL with ToQueryString() while you work.
        //   https://learn.microsoft.com/ef/core/querying/related-data/
        //   https://learn.microsoft.com/ef/core/querying/#projections
        //   https://learn.microsoft.com/ef/core/logging-events-diagnostics/simple-logging#tostring-and-toquerystring
        _ = (words, baseLanguage, targetLanguage, cancellationToken);
        throw new NotImplementedException("TODO(human) #1: card query (spec 001, R2-R4).");
    }

    // Pure logic, no database: `internal static`, so WordWise.Api.Tests can call it directly
    // (InternalsVisibleTo in WordWise.Api.csproj) and unit-test it without a DbContext (testing.md).
    // Docs: https://learn.microsoft.com/dotnet/standard/assembly/friend
    internal static IReadOnlyList<WordCardResponse> SortTranslationsAndApplyOrder(
        List<WordCardResponse> cards,
        CultureInfo baseCulture,
        CardOrder order)
    {
        // TODO(human) #2: in memory, after the database query:
        //   - Write the unit tests first (no database): call this method with a few hand-made cards.
        //     For CardOrder.Random assert the same set of cards, not their order (a random order can't be predicted).
        //   - R4: sort the Translations of each card alphabetically in the culture of the base language.
        //     The database would sort with its own collation, so do it here with an explicit comparer.
        //     Records are immutable: use a `with` expression to get a copy with sorted Translations.
        //   - R2: for CardOrder.Random, return the same cards in random order.
        //   - Then delete the two lines below.
        // Hint: StringComparer.Create(culture, ignoreCase: false), Random.Shared.Shuffle(array).
        //   https://learn.microsoft.com/dotnet/standard/base-types/best-practices-strings
        //   https://learn.microsoft.com/dotnet/csharp/language-reference/operators/with-expression
        //   https://learn.microsoft.com/dotnet/api/system.random.shuffle
        _ = (cards, baseCulture, order);
        throw new NotImplementedException("TODO(human) #2: translation sort and random order (spec 001, R2, R4).");
    }
}
