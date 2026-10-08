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
- [ ] **T4 `[solo]`** CSV parser (`WordSeedCsvParser`): row → record with a list of forms per language. Tests first (synonyms, empty rank, Polish and Turkish characters).
- [ ] **T5 `[learn]`** Idempotent import (`WordSeedImporter`) run at startup in Development.
  - *To understand:* `IHostedService` vs a call in `Program.cs`, `AnyAsync`, a single `SaveChangesAsync`.
- [ ] **T6 `[learn]`** `WordsController` + LINQ query for R2–R4 (`AsNoTracking`, `Select` into `WordCardResponse`).
  - *You also write:* a second index on `Words` for R2 (filter by `Language`, sort by `FrequencyRank`): column order, uniqueness, and a migration for it.
  - *You write:* the LINQ query. *Claude:* controller, DTOs, R1 validation.
- [ ] **T7 `[solo]`** Endpoint integration tests: 400 for R1 (no database).
- [ ] **T8 `[gen]`** Review with the `code-reviewer` agent + journal entry + `/quiz-me` on topics from T1–T6.
