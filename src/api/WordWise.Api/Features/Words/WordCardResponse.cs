namespace WordWise.Api.Features.Words;

// One flashcard: a target-language Word and its translations in the base language (spec 001, Contract).
// A positional record: the compiler generates the constructor, init-only properties and value equality.
// Docs: https://learn.microsoft.com/dotnet/csharp/fundamentals/types/records
public sealed record WordCardResponse(
    int WordId,
    string Text,
    int Rank,
    string PartOfSpeech,
    IReadOnlyList<string> Translations);
