# ADR-0005: Word model "Concept + per-language forms"

- Status: Accepted
- Date: 2026-09-30

## Context
The top-1000 lists for EN, PL and TR differ. Words can be ambiguous (EN *right* = prawy / racja). We need any pair of learning directions.

## Considered options
1. **Concept + Word(Language, Text, FrequencyRank?)**: a translation = words sharing the same Concept.
2. Word per language + a translation-pair table: many-to-many, three grids of links to maintain.
3. Flat cards per direction: data duplication, 6 directions, fragmented progress.

## Decision
Option 1. Details in `docs/architecture.md#data-model-adr-0005`.
- One meaning = one Concept (an ambiguous word appears in several Concepts).
- Synonyms within one language are allowed (several Words with the same Concept and Language).
- `FrequencyRank` is per language, `null` = outside the top 1000.
- Progress (`UserWordProgress`) is tracked per target word, shared across all base languages.

## Consequences
- Adding a language = new Word rows, no schema changes.
- A word without a translation in the base language is skipped in a session (rule in spec 001).
- Data work: assigning words to Concepts during import.

## Learning resources
- Relationships in EF Core: https://learn.microsoft.com/ef/core/modeling/relationships
- Indexes (unique, composite): https://learn.microsoft.com/ef/core/modeling/indexes
