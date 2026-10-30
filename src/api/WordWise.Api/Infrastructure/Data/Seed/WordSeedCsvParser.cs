using System.Globalization;

namespace WordWise.Api.Infrastructure.Data.Seed;

public static class WordSeedCsvParser
{

    private const string ConceptKeyColumnName = "concept_key";
    private const string PartOfSpeechColumnName = "part_of_speech";
    private const string RankSuffix = "_rank";
    public static IReadOnlyList<WordSeed> Parse(
        IEnumerable<string> lines)
    {
        var linesList = lines.ToList();
        string[] headerNames = linesList.First().Split(";", StringSplitOptions.TrimEntries);
        Dictionary<string, int> headerIndexes = new(StringComparer.Ordinal);
        List<(string Code, int FormsIndex, int RankIndex)> languagesPositions = [];

        for (int i = 0; i < headerNames.Length; i++)
        {
            headerIndexes[headerNames[i]] = i;
        }

        for (int i = 0; i < headerNames.Length; i++)
        {
            if (headerNames[i] == ConceptKeyColumnName
                || headerNames[i] == PartOfSpeechColumnName
                || headerNames[i].EndsWith(RankSuffix, StringComparison.Ordinal)
                )
            {
                continue;
            }

            if (!headerIndexes.TryGetValue(headerNames[i] + RankSuffix, out int rankIndex))
            {
                throw new InvalidDataException($"Line 1: missing column {headerNames[i] + RankSuffix} for language {headerNames[i]}");
            }

            languagesPositions.Add((headerNames[i], i, rankIndex));
        }

        var linesWithoutHeader = linesList.Skip(1);
        List<WordSeed> wordSeeds = [];

        foreach ((int index, string line) in linesWithoutHeader.Index())
        {
            string[] lineParts = line.Split(';', StringSplitOptions.TrimEntries);
            List<WordSeedLanguage> wordSeedsLanguages = [];

            if (lineParts.Length != headerNames.Length)
            {
                throw new InvalidDataException($"Line {index + 2}: expected {headerNames.Length} columns, found {lineParts.Length}");
            }

            foreach ((string Code, int FormsIndex, int RankIndex) language in languagesPositions)
            {
                var forms = lineParts[language.FormsIndex].Split('|', StringSplitOptions.TrimEntries);
                int? rank = lineParts[language.RankIndex] == "" ? null : int.Parse(lineParts[language.RankIndex], CultureInfo.InvariantCulture);
                wordSeedsLanguages.Add(new WordSeedLanguage(language.Code, forms, rank));
            }

            wordSeeds.Add(new WordSeed(lineParts[headerIndexes[ConceptKeyColumnName]], lineParts[headerIndexes[PartOfSpeechColumnName]], wordSeedsLanguages));
        }

        return wordSeeds;
    }
}
