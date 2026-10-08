# Testing conventions

## Backend
- Project `tests/WordWise.Api.Tests`, folder structure mirrors `src/api/WordWise.Api`.
- Test name: `Method_Scenario_ExpectedResult`, e.g. `GetCards_RandomOrder_ReturnsAllWordsOnce`.
- **Arrange / Act / Assert** layout, one reason to fail per test.
- Assertions: Shouldly (`result.ShouldBe(...)`, `list.ShouldContain(...)`).
- Kinds:
  - **unit**: pure logic (e.g. card order, CSV parser), no database;
  - **integration**: `WebApplicationFactory<Program>` (the whole HTTP pipeline). Tests that need the database use the shared Postgres with one database per test run (ADR-0013).
- Pass `TestContext.Current.CancellationToken` to async calls (xUnit v3 analyzer rule).
- Running: `dotnet test --solution WordWise.slnx` (Microsoft.Testing.Platform).

## Frontend
- Vitest + TestBed. Test user-visible behavior (text, clicks), not implementation details.
- Transloco in tests: `TranslocoTestingModule.forRoot({ langs: { pl, en }, ... })`.

## Learning rule
For `[learn]` and `[solo]` tasks **the author writes the tests first** (or at least a list of cases), and Claude assesses their completeness.

Documentation: https://learn.microsoft.com/dotnet/core/testing/unit-testing-best-practices
