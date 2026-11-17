# Tasks: Spec 001, word catalog

Modes: `[solo]` you write it yourself · `[learn]` skeleton + TODO(human) · `[gen]` Claude generates, you do an explain-back.
You can change the mode before starting a task.
Before each task: [study-guide.md](study-guide.md) (what to read, what to decide, traps).

- [x] **T1 `[gen]`** EF Core + Npgsql packages, `WordWiseDbContext`, connection string in `appsettings.Development.json`, DI registration.
  - *To understand:* `Scoped` DI lifetime for DbContext, configuration from `IConfiguration`.
- [x] **T2 `[learn]`** `Concept`, `Word` entities + `IEntityTypeConfiguration` (1-N relationship, unique indexes from ADR-0005).
  - *You write:* entity properties and index configuration. *Claude:* file skeletons.
- [x] **T3 `[gen]`** First migration `InitialWordCatalog` + `dotnet ef database update`.
  - *Predict before run:* which tables and indexes will be created? Write it down, then compare with the migration file.
- [x] **T4 `[solo]`** CSV parser (`WordSeedCsvParser`): row → record with a list of forms per language. Tests first (synonyms, empty rank, Polish and Turkish characters).
  - *Design decisions (author, 2026-10-10):*
    1. Input: lines (`IEnumerable<string>`), not a file path. The caller (T5) opens the file, so tests need no files on disk.
    2. Output: one record per row with a **collection** of languages (code, forms, rank), not one property per language. T5 maps it with one loop, and a new language needs no parser change.
    3. Bad row: throw (fail fast). The message names the line number and the reason. The seed is edited only by developers.
    4. Header: read the language columns from the header (`xx` + `xx_rank` pairs), not by fixed position. A missing `xx_rank` column is an error. Checking that a language code is supported belongs to T5 (`LanguageCodes`).
    5. Whitespace: `" dom | chata "` is valid. Trim both when splitting by `;` and when splitting by `|`.
    6. Empty synonym (`"dom||chata"`, `"dom|"`): throw, like any other bad row (decision 3).
  - *Design decisions, invalid input (author, 2026-10-30):*
    7. No lines at all: throw with a message that the header is missing.
    8. Header only: return an empty list. An empty seed is not an error.
    9. Blank line (e.g. a trailing newline added by an editor): skip it. A line with only whitespace counts as blank. Line numbers in error messages still count lines in the file, blank lines included.
    10. Empty forms column (`;;`): throw with the line number.
    11. Empty `concept_key`: throw with the line number.
    12. Rank 0 or negative: throw with the line number. Ranks start at 1.
- [x] **T5 `[learn]`** Idempotent import (`WordSeedImporter`) run at startup in Development.
  - *To understand:* `IHostedService` vs a call in `Program.cs`, `AnyAsync`, a single `SaveChangesAsync`.
  - *Status (2026-11-13):* Claude part done: `DevelopmentDatabaseSetup` (migrate + import before `app.Run()`, behind `Seed:RunAtStartup` set only by the launch profiles), tests against Postgres. `TODO(human)` 1–4 done: skip when the catalog has concepts (R5), reject unsupported language codes before adding anything, map seeds to `Concept` + one `Word` per form, one `SaveChangesAsync`.
- [x] **T6 `[learn]`** `WordsController` + LINQ query for R2–R4 (`AsNoTracking`, `Select` into `WordCardResponse`).
  - *You also write:* a second index on `Words` for R2 (filter by `Language`, sort by `FrequencyRank`): column order, uniqueness, and a migration for it.
  - *You write:* the LINQ query. *Claude:* controller, DTOs, R1 validation.
  - *Status (2026-10-16):* Claude part done. `TODO(human)` #1 and #2 done (`Features/Words/WordQueries.cs`). Left for you: check the generated SQL (`ToQueryString()`), #3 in `WordConfiguration.cs`.
  - *Status (2026-10-18):* generated SQL checked in the EF log. Deeper LINQ ↔ SQL analysis is deferred to the review phase (`docs/learning/roadmap.md` §3). #3 done: index `(Language, FrequencyRank)`, migration `AddWordsLanguageRankIndex`.
- [x] **T7 `[solo]`** Endpoint integration tests: 400 for R1 (no database).
  - *Status (2026-11-15):* `Features/Words/WordsEndpointTests.cs`: one test per rule (missing `base`, unsupported `base`, `base` = `target`). Each checks the status code and the field key in `ValidationProblemDetails.Errors`.
- [x] **T8 `[gen]`** Review with the `code-reviewer` agent + journal entry + `/quiz-me` on topics from T1–T6.
  - *Status (2026-11-17):* review done. Findings split into T9 and T10 below; the auth-related finding (spec 002 fallback policy turns these 400 tests into 401) is noted in spec 002 T6. Quiz: 2.5 / 5 (see the journal).
- [ ] **T9 `[solo]`** Tests for R2–R4 (from the T8 review): there is no test for the 200 path of `GET /api/words/cards`.
  - Move `WordsEndpointTests` from `WebApplicationFactory<Program>` to `PostgresApiFactory` + `[Collection(PostgresCollectionDefinition.Name)]` (ADR-0013: a 200 test must never reach the developer database).
  - Each test arranges its own data (the shared database has no guaranteed test order). Cases: cards sorted by `rank` ascending, `rank = null` left out (R2); a target word with no translation in the base language is left out (R3); two concepts with the same English form (e.g. *right*) give two cards (R4); the same rank is ordered by `Id`; `order` defaults to `sequential`; `order=random` returns the same set of cards; the JSON shape of a 200 response.
- [ ] **T10 `[gen]`** Small fixes from the T8 review:
  - `order` accepts only defined `CardOrder` values (`?order=5` → 400 with the `order` key), plus a test.
  - A Turkish case (`İ`/`ı`) in the `SortTranslationsAndApplyOrder` tests (repo rule 6).
  - Parser: `concept_key` longer than 255 or `part_of_speech` longer than 30 → error with the line number; a duplicate `concept_key` → error with the line number.
  - Readability: `word.FrequencyRank!.Value` instead of `(int)word.FrequencyRank!` in `WordQueries`; read the result through a second scope in `ImportAsync_Twice`; document that the parser does not support quoted fields.
