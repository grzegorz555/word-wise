# Konwencje testów

## Backend
- Projekt `tests/WordWise.Api.Tests`, struktura folderów lustrzana do `src/api/WordWise.Api`.
- Nazwa testu: `Method_Scenario_ExpectedResult`, np. `GetCards_RandomOrder_ReturnsAllWordsOnce`.
- Układ **Arrange / Act / Assert**, jeden powód do porażki na test.
- Asercje: Shouldly (`result.ShouldBe(...)`, `list.ShouldContain(...)`).
- Rodzaje:
  - **unit**: czysta logika (np. kolejność kart, parser CSV), bez bazy;
  - **integration**: `WebApplicationFactory<Program>` (cały pipeline HTTP). Baza docelowo przez Testcontainers.
- `TestContext.Current.CancellationToken` przekazuj do wywołań async (reguła analyzera xUnit v3).
- Uruchamianie: `dotnet test --solution WordWise.slnx` (Microsoft.Testing.Platform).

## Frontend
- Vitest + TestBed. Testuj zachowanie widoczne dla użytkownika (tekst, kliknięcie), a nie szczegóły implementacji.
- Transloco w testach: `TranslocoTestingModule.forRoot({ langs: { pl, en }, ... })`.

## Zasada nauki
Przy zadaniach `[learn]` i `[solo]` **autor pisze testy pierwszy** (lub przynajmniej listę przypadków), a Claude ocenia ich kompletność.

Dokumentacja: https://learn.microsoft.com/dotnet/core/testing/unit-testing-best-practices
