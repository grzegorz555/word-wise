namespace WordWise.Api.Infrastructure.Data.Seed;

public sealed record WordSeedLanguage(
    string LanguageCode,
    IReadOnlyList<string> Forms,
    int? Rank);
