# ADR-0007: Word data: seed of ~50 words + CSV import

- Status: Accepted
- Date: 2026-09-30

## Context
Full top-1000 lists require research (licenses, e.g. CC BY-SA), lemmatization (Turkish writes inflected forms as separate words) and translations. That is a separate task and should not block building the app.

## Decision
- File `data/seed/words.seed.csv` with ~50 meanings. Separator `;`, UTF-8. Synonyms separated by `|`.
- Columns: `concept_key;part_of_speech;en;en_rank;pl;pl_rank;tr;tr_rank`.
- **Ranks in the seed are approximate (order in the file)**, not real frequency data. They will be replaced by importing the full lists.
- The import is idempotent (re-running it does not duplicate data), so the same code will later serve the full lists.

## Consequences
- The source of the full data and its license will be described in a new ADR before it lands in the public repo.

## Learning resources
- Seeding in EF Core: https://learn.microsoft.com/ef/core/modeling/data-seeding
