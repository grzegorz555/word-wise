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

    private async Task<List<WordCardResponse>> LoadCardsAsync(
        string baseLanguage,
        string targetLanguage,
        CancellationToken cancellationToken)
    {
        IQueryable<Word> words = dbContext.Words.AsNoTracking();

        IQueryable<WordCardResponse> cards = from word in words
                                             where word.Language == targetLanguage &&
                                             word.Concept.Words.Any(conceptWord => conceptWord.Language == baseLanguage) &&
                                             word.FrequencyRank != null
                                             orderby word.FrequencyRank, word.Id
                                             select new WordCardResponse(word.Id, word.Text,
                                             (int)word.FrequencyRank!,
                                             word.Concept.PartOfSpeech,
                                             (from translation in word.Concept.Words where translation.Language == baseLanguage select translation.Text).ToList());

        return await cards.ToListAsync(cancellationToken);
    }

    // Pure logic, no database: `internal static`, so WordWise.Api.Tests can call it directly
    // (InternalsVisibleTo in WordWise.Api.csproj) and unit-test it without a DbContext (testing.md).
    // Docs: https://learn.microsoft.com/dotnet/standard/assembly/friend
    internal static IReadOnlyList<WordCardResponse> SortTranslationsAndApplyOrder(
        List<WordCardResponse> cards,
        CultureInfo baseCulture,
        CardOrder order)
    {
        var comparer = StringComparer.Create(baseCulture, ignoreCase: false);
        WordCardResponse[] sortedCards = cards.Select(card =>
            card with { Translations = card.Translations.OrderBy(t => t, comparer).ToList() }
        ).ToArray();

        if (order == CardOrder.Random)
        {
            Random.Shared.Shuffle(sortedCards);
        }

        return sortedCards;
    }
}
