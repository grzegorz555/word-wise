namespace WordWise.Api.Infrastructure.Data.Seed;

public sealed record WordSeed(
    string ConceptKey,
    string PartOfSpeech,
    IReadOnlyList<WordSeedLanguage> Languages);

public sealed record WordSeedLanguage(
    string LanguageCode,
    IReadOnlyList<string> Forms,
    int? Rank);
