# Backend (ASP.NET Core 10): kontekst dla Claude

Pełne konwencje: `docs/conventions/backend.md`. Testy: `docs/conventions/testing.md`.

## Struktura (vertical slices)
```
WordWise.Api/
  Program.cs                 # DI + pipeline; tylko wywołania Add*/Map*, bez logiki
  Features/<Feature>/        # np. Words/, Study/, Account/
    <Feature>Controller.cs   # cienki kontroler: walidacja wejścia → handler/serwis → wynik HTTP
    <UseCase>.cs             # request/response DTO + logika przypadku użycia
  Domain/                    # encje EF (Concept, Word, UserWordProgress, AppUser)
  Infrastructure/Data/       # WordWiseDbContext, konfiguracje IEntityTypeConfiguration, migracje
```

## Najważniejsze reguły
- Kontroler nie odwołuje się bezpośrednio do `DbContext`; robi to kod feature'a. Kontroler zwraca `ActionResult<T>` / `ProblemDetails`.
- DTO to `record`y. Encji nie zwracamy z API.
- Asynchronicznie wszędzie z I/O, `CancellationToken` przekazywany do EF.
- Zapytania tylko do odczytu: `AsNoTracking()` + projekcja `Select` do DTO.
- Konfiguracja EF przez Fluent API (`IEntityTypeConfiguration<T>`), nie atrybuty.
- Język jako kod ISO 639-1 (`en`/`pl`/`tr`); porównania przez `StringComparison.Ordinal`/`CultureInfo`, nigdy przez domyślną kulturę.
- Build ma `TreatWarningsAsErrors` + analyzery `latest-recommended`. Ostrzeżenie analyzera to okazja do nauki: wyjaśnij regułę z linkiem (CAxxxx) zamiast ją wyłączać.
