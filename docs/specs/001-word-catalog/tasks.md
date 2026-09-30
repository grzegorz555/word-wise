# Tasks: Spec 001, katalog słów

Tryby: `[solo]` piszesz sam · `[learn]` szkielet + TODO(human) · `[gen]` Claude generuje, Ty robisz explain-back.
Tryby możesz zmienić przed startem zadania.

- [ ] **T1 `[gen]`** Pakiety EF Core + Npgsql, `WordWiseDbContext`, connection string w `appsettings.Development.json`, rejestracja w DI.
  - *Do zrozumienia:* DI lifetime `Scoped` dla DbContext, konfiguracja z `IConfiguration`.
- [ ] **T2 `[learn]`** Encje `Concept`, `Word` + `IEntityTypeConfiguration` (relacja 1-N, unikalne indeksy z ADR-0005).
  - *Ty piszesz:* właściwości encji i konfigurację indeksów. *Claude:* szkielet plików.
- [ ] **T3 `[gen]`** Pierwsza migracja `InitialWordCatalog` + `dotnet ef database update`.
  - *Predict before run:* jakie tabele i indeksy powstaną? Zapisz, potem porównaj z plikiem migracji.
- [ ] **T4 `[solo]`** Parser CSV (`WordSeedCsvParser`): wiersz → rekord z listą form per język. Najpierw testy (synonimy, pusta ranga, polskie i tureckie znaki).
- [ ] **T5 `[learn]`** Import idempotentny (`WordSeedImporter`) uruchamiany przy starcie w Development.
  - *Do zrozumienia:* `IHostedService` vs wywołanie w `Program.cs`, `AnyAsync`, jeden `SaveChangesAsync`.
- [ ] **T6 `[learn]`** `WordsController` + zapytanie LINQ dla R2–R4 (`AsNoTracking`, `Select` do `WordCardResponse`).
  - *Ty piszesz:* zapytanie LINQ. *Claude:* kontroler, DTO, walidacja R1.
- [ ] **T7 `[solo]`** Testy integracyjne endpointu: 400 dla R1 (bez bazy).
- [ ] **T8 `[gen]`** Review agentem `code-reviewer` + wpis w dzienniku + `/quiz-me` z tematów T1–T6.
