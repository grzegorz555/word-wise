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
        // TODO(human) 1: idempotency (R5). If the catalog already has data, add nothing and return 0.
        //   Hint: one query that answers "is there at least one row?" without loading any rows.
        //   Question: why "any row" and not "count == seeds.Count"? What happens when the CSV grows?

        // TODO(human) 2: reject unsupported language codes BEFORE adding anything (T4 decision 4).
        //   Throw InvalidDataException (like the parser) naming the concept key and the code.
        //   The supported codes are the constants in LanguageCodes. Compare with StringComparison.Ordinal.

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
