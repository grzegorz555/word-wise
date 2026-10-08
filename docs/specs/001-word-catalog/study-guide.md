# Study guide: spec 001, tasks T4–T7

What to read and decide **before** each remaining task. No solutions here: `[solo]` and `[learn]` mean you write the code.
Each section: *read first* → *decide yourself* → *traps* → *done when*. Links verified via the `microsoft-learn` MCP (2026-10-08), unless marked otherwise.

---

## T4 `[solo]`: CSV parser (`WordSeedCsvParser`), tests first

### Read first
- Splitting strings, `StringSplitOptions` (`TrimEntries`, `RemoveEmptyEntries`): https://learn.microsoft.com/dotnet/csharp/fundamentals/strings/common-tasks/split · https://learn.microsoft.com/dotnet/api/system.stringsplitoptions
- Parsing numbers safely (`int.TryParse` with `CultureInfo.InvariantCulture`): https://learn.microsoft.com/dotnet/api/system.int32.tryparse
- Records (immutable result type, value equality, which makes `ShouldBe` in tests easy): https://learn.microsoft.com/dotnet/csharp/fundamentals/types/records
- Unit testing best practices (naming, one reason to fail, no logic in tests): https://learn.microsoft.com/dotnet/core/testing/unit-testing-best-practices
- xUnit v3 `[Theory]` + `[InlineData]` / `[MemberData]`: https://xunit.net/docs/getting-started/v3/getting-started (not on Microsoft Learn, so not checked via MCP)
- fCC course: strings, arrays, `if`, loops, methods. Everything in this task is **in the course** except records and xUnit.

### Decide yourself (write the answers down before coding)
1. **Input:** a file path, a `TextReader`, or `IEnumerable<string>` lines? Which one makes the tests need no files on disk?
2. **Output:** what does one row become? The spec says "a record with a list of forms per language". How do you represent "language → forms + rank" so that T5 can use it without knowing the CSV?
3. **Bad rows:** throw (fail fast, which row and why?), skip, or collect errors? The seed is checked into the repo, so a broken row is a developer bug.
4. **Header:** check it, or skip the first line blindly? What happens if someone reorders the columns?
5. Where does trimming happen, and is `" dom | chata "` valid?

### Traps
- `int.Parse("")` throws. An empty rank means "not in the top 1000" (`null`), not 0 (ADR-0005).
- The default culture: `int.Parse` uses the current culture, so use the invariant culture for data files (ADR-0006; rule CA1305 explains why: https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1305).
- `ł`, `ą`, `ş`, `ğ`, `İ`, `ı`: read the file as UTF-8 (the `File`/`StreamReader` default) and add at least one test with such characters.
- Do not lowercase anything with `ToLower()`: Turkish `I` → `ı`, not `i`.

### Example on a different problem (allowed in `[solo]`)
Parsing `"red=1;green=;blue=3"` into `(string Name, int? Value)` pairs, test-first:

```csharp
[Theory]
[InlineData("red=1", "red", 1)]
[InlineData("green=", "green", null)]
public void ParsePair_ValidInput_ReturnsNameAndValue(string input, string name, int? value)
{
    var result = PairParser.ParsePair(input);

    result.ShouldBe((name, value));
}
```

The shape to copy: one `[Theory]` per rule, the expected values in `[InlineData]`, and a name that reads as a sentence (`docs/conventions/testing.md`).

### Done when
- The acceptance criterion "the CSV parser correctly reads `a|b` synonyms and empty ranks" is covered by tests you wrote first.
- `code-reviewer` agent: no issues with the conventions. Journal entry with "In my own words".

---

## T5 `[learn]`: idempotent import at startup (Development)

### Read first
- **Data seeding in EF Core (four options, and which one the docs recommend):** https://learn.microsoft.com/ef/core/modeling/data-seeding
- Hosted services and why they cannot take a `DbContext` in the constructor (singleton vs scoped): https://learn.microsoft.com/aspnet/core/fundamentals/host/hosted-services#consuming-a-scoped-service-in-a-background-task
- Service lifetimes, "don't resolve a scoped service from a singleton": https://learn.microsoft.com/dotnet/core/extensions/dependency-injection/service-lifetimes#scoped
- fCC course: beyond the course (DI, hosting, EF Core).

### Decide yourself
The task says "`IHostedService` vs a call in `Program.cs`". The EF Core docs add a third option and call it the recommended one: `UseSeeding` / `UseAsyncSeeding` on the `DbContext` options (EF 9+), which runs after `Migrate` and after `dotnet ef database update`. Before T5, write down for each of the three:
- When does it run (every start? only after migrations?), and in which environments?
- How do you keep it to Development only?
- What happens with two API instances starting at once? (The docs mention migration locking.)
- How would you test "import 2× → the same number of words" (R5)?

If you pick a different approach than the task describes, that is a small design decision: note it in the journal (or `/compare-options` → ADR if it affects deployment).

### Traps
- `SaveChangesAsync` in a loop: one round trip per row. One call at the end (`docs/conventions/backend.md`).
- Idempotent means "same result when run twice", not just "check `AnyAsync` once". What if the seed file gets a new row later?
- Validation that the database does not do for you (T3 journal): `FrequencyRank` range and language codes. Use `LanguageCodes` from T6.

---

## T6 `[learn]`: your `TODO(human)` #1–#3 (PR #18)

### Read first
- Projections and related data in queries: https://learn.microsoft.com/ef/core/querying/related-data/
- See the SQL that EF generates (`ToQueryString`): https://learn.microsoft.com/ef/core/logging-events-diagnostics/simple-logging
- What runs in SQL and what runs in .NET: https://learn.microsoft.com/ef/core/querying/client-eval
- Why EF does not translate `string.Equals(..., StringComparison)` and what collations are: https://learn.microsoft.com/ef/core/miscellaneous/collations-and-case-sensitivity
- Composite indexes: https://learn.microsoft.com/ef/core/modeling/indexes
- fCC course: LINQ is beyond the course; lambdas and `List<T>` are in it.

### Decide yourself
- #1: is it one query or two? Count the SQL statements in the log.
- #3: column order of the index. Which column do you filter by with `=`, and which do you sort by?
- #3: unique or not? Look at the seed: can two words in one language share a rank?

### Done when
- `GET /api/words/cards?base=pl&target=tr` returns cards sorted by `rank` (acceptance criterion), and `order=random` returns the same set.
- The migration's `Up()` matches your prediction.

---

## T7 `[solo]`: integration tests for R1 (no database)

### Read first
- Integration tests with `WebApplicationFactory`: https://learn.microsoft.com/aspnet/core/test/integration-tests
- Reading the error body (`ReadFromJsonAsync<ValidationProblemDetails>`): https://learn.microsoft.com/dotnet/api/system.net.http.json.httpcontentjsonextensions.readfromjsonasync
- The 400 response format: https://learn.microsoft.com/aspnet/core/web-api/#automatic-http-400-responses
- An existing example in the repo: `tests/WordWise.Api.Tests/HealthEndpointTests.cs`.

### Decide yourself
- Which R1 cases are worth a test? The PR #18 description lists the cases Claude probed. Do you need all of them, or one per rule?
- Is checking the status code enough, or should a test also check *which* field failed (`errors["target"]`)?
- Why does this test need no database even though the controller depends on `WordWiseDbContext`?

### Traps
- Pass `TestContext.Current.CancellationToken` to every async call (xUnit v3 analyzer).
- Test names: `Method_Scenario_ExpectedResult`, e.g. `GetCards_SameBaseAndTarget_Returns400`.

---

## After T7: T8 `[gen]`
`code-reviewer` on the whole spec, journal, `/quiz-me` on T1–T6. Then tick spec 001 in [issue #9](https://github.com/grzegorz555/word-wise/issues/9).
