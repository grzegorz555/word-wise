namespace WordWise.Api.Infrastructure.Data.Seed;

public sealed record WordSeed(
    string ConceptKey,
    string PartOfSpeech,
    IReadOnlyList<WordSeedLanguage> Languages);
