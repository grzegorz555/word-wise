# Testing conventions

## Backend
- Project `tests/WordWise.Api.Tests`, folder structure mirrors `src/api/WordWise.Api`.
- Test name: `Method_Scenario_ExpectedResult`, e.g. `GetCards_RandomOrder_ReturnsAllWordsOnce`.
- **Arrange / Act / Assert** layout, one reason to fail per test.
- Assertions: Shouldly (`result.ShouldBe(...)`, `list.ShouldContain(...)`).
- Kinds:
  - **unit**: pure logic (e.g. card order, CSV parser), no database;
  - **integration**: `WebApplicationFactory<Program>` (the whole HTTP pipeline). Tests that need the database use the shared Postgres with one database per test run (ADR-0013).
- **Database tests** (ADR-0013), example: `tests/WordWise.Api.Tests/Infrastructure/PostgresApiFactoryTests.cs`:
  - Mark the class `[Collection(PostgresCollectionDefinition.Name)]` and take `PostgresApiFactory factory` in the constructor. xUnit creates one factory per test run: it creates the database `wordwise_test_<id>` with `MigrateAsync` and drops it at the end.
  - HTTP: `factory.CreateClient()`. Data: `await using var scope = factory.Services.CreateAsyncScope();` + `GetRequiredService<WordWiseDbContext>()`.
  - The database is shared by all tests of the run, so **each test creates the data it needs** and doesn't depend on other tests or their order (e.g. don't assert an exact row count of a whole table).
  - Run `docker compose up -d` first. Without Postgres these tests fail with "Postgres is not running…"; tests without a database don't need Docker.
- Pass `TestContext.Current.CancellationToken` to async calls (xUnit v3 analyzer rule).
- Running: `dotnet test --solution WordWise.slnx` (Microsoft.Testing.Platform).

## Frontend
- Vitest + TestBed. Test user-visible behavior (text, clicks), not implementation details.
- Transloco in tests: `TranslocoTestingModule.forRoot({ langs: { pl, en }, ... })`.

## Learning rule
For `[learn]` and `[solo]` tasks **the author writes the tests first** (or at least a list of cases), and Claude assesses their completeness.

Documentation: https://learn.microsoft.com/dotnet/core/testing/unit-testing-best-practices
