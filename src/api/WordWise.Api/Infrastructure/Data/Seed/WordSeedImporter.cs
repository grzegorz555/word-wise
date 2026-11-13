using Microsoft.EntityFrameworkCore;

using WordWise.Api.Domain;

namespace WordWise.Api.Infrastructure.Data.Seed;

// Saves parsed seed rows (WordSeedCsvParser) as Concepts and Words (spec 001 T5, ADR-0007).
// Takes records, not a file path, so tests need no file on disk; the caller opens the file
// (DevelopmentDatabaseSetup). Scoped, like the WordWiseDbContext it uses.
// Docs: https://learn.microsoft.com/ef/core/saving/basic
public sealed class WordSeedImporter(WordWiseDbContext db)
{
    // Returns the number of Concepts added: 0 when the catalog already has data (R5: idempotent).
    public async Task<int> ImportAsync(IReadOnlyList<WordSeed> seeds, CancellationToken cancellationToken)
    {
        if (await db.Concepts.AnyAsync(cancellationToken))
        {
            return 0;
        }

        foreach (WordSeed wordSeed in seeds)
        {
            foreach (WordSeedLanguage language in wordSeed.Languages)
            {
                if (language.LanguageCode is not (LanguageCodes.English or LanguageCodes.Polish or LanguageCodes.Turkish))
                {
                    throw new InvalidDataException($"Concept '{wordSeed.ConceptKey}': language code '{language.LanguageCode}' is not supported.");
                }
            }
        }

        // TODO(human) 3: map each WordSeed to one Concept, and each form of each language to one Word.
        //   Synonyms ("mówić|powiedzieć") = several Words with the same Concept, Language and Rank.
        //   Hint: add Words to concept.Words; EF sets ConceptId itself when it saves the graph.
        //   Docs: https://learn.microsoft.com/ef/core/saving/related-data#adding-a-graph-of-new-entities

        // TODO(human) 4: save everything with ONE SaveChangesAsync (one transaction: all rows or none),
        //   pass cancellationToken, and return the number of Concepts added.
        //   Docs: https://learn.microsoft.com/ef/core/saving/transactions#default-transaction-behavior

        _ = db;
        await Task.CompletedTask;
        throw new NotImplementedException("TODO(human): spec 001 T5");
    }
}
