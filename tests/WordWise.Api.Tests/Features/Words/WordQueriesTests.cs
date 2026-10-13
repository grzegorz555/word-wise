using WordWise.Api.Domain;
using WordWise.Api.Features.Words;

namespace WordWise.Api.Tests.Features.Words;

public sealed class WordQueriesTests
{
    [Fact]
    public void SortTranslationsAndApplyOrder_PolishCulture_SortsTranslationsWithPolishLetters()
    {
        var cards = new List<WordCardResponse>
        {
            new WordCardResponse(1, "Hello", 1, "Noun", new List<string> { "alfabet", "zamek", "koń", "ćma" }),
            new WordCardResponse(2, "World", 2, "Noun", new List<string> { "mydło", "dom", "kot", "łoś" }),
        };

        var sortedCards = WordQueries.SortTranslationsAndApplyOrder(cards, LanguageCodes.GetCulture(LanguageCodes.Polish), CardOrder.Sequential);

        sortedCards[0].Translations.ShouldBe(new List<string> { "alfabet", "ćma", "koń", "zamek" });
        sortedCards[1].Translations.ShouldBe(new List<string> { "dom", "kot", "łoś", "mydło" });
    }

    [Fact]
    public void SortTranslationsAndApplyOrder_Random_ReturnsSameCardsWithSortedTranslations()
    {
        var cards = new List<WordCardResponse>
        {
            new WordCardResponse(1, "Hello", 1, "Noun", new List<string> { "alfabet", "zamek", "koń", "ćma" }),
            new WordCardResponse(2, "World", 2, "Noun", new List<string> { "mydło", "dom", "kot", "łoś" }),
            new WordCardResponse(3, "Hello", 3, "Noun", new List<string> { "złoto", "srebro", "ołów", "tlen" }),
            new WordCardResponse(4, "World", 4, "Noun", new List<string> { "kurczak", "indyk", "kura", "bocian" }),
        };

        var results = WordQueries.SortTranslationsAndApplyOrder(cards, LanguageCodes.GetCulture(LanguageCodes.Polish), CardOrder.Random);

        results.Select(c => c.WordId).ShouldBe([1, 2, 3, 4], ignoreOrder: true);

        results.Single(c => c.WordId == 1).Translations.ShouldBe(new List<string> { "alfabet", "ćma", "koń", "zamek" });
        results.Single(c => c.WordId == 2).Translations.ShouldBe(new List<string> { "dom", "kot", "łoś", "mydło" });
        results.Single(c => c.WordId == 3).Translations.ShouldBe(new List<string> { "ołów", "srebro", "tlen", "złoto" });
        results.Single(c => c.WordId == 4).Translations.ShouldBe(new List<string> { "bocian", "indyk", "kura", "kurczak" });
    }
}
