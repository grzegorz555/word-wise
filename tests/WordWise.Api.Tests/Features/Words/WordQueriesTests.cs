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
}
