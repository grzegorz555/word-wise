# Learning journal

After each task Claude adds an entry in this format (newest on top). Fill in "In my own words" — that is your explain-back.

```
## YYYY-MM-DD: <spec/task>, mode [solo|learn|gen]
- Concepts: <names, e.g. Options pattern, scoped lifetime>
- Docs: <learn.microsoft.com links>
- In my own words: <2–3 sentences, written by the author>
- Check question: <question for /quiz-me>
- fCC course: <related module or "beyond the course">
```

---

## 2026-11-17 — Quiz

- Topics: spec 001 T1–T6 + roadmap §0. Score: **2.5 / 5**.
  - 🟡 Parser: an empty forms column (`;;`) throws `Line 3: empty forms found for language pl` (not the header error); line number = `index + 2`.
  - ✅ `DbContext` as a singleton: not thread-safe ("A second operation was started…"), tracker grows; default scoped = one per request (unit of work).
  - 🟡 §0: "actively refused" = `RST` (nothing listens on the port) vs timeout (no answer to `SYN`: firewall, network); check with `Test-NetConnection localhost -Port 5432`, `netstat -ano`.
  - 🟡 One `SaveChangesAsync` = one transaction → 0 rows after a failed `INSERT`; next start: `AnyAsync` is false, the import fails again, the API doesn't start (fail fast before `app.Run()`).
  - ❌ Composite index: equality column first (`Language`), then the `ORDER BY` column, so the `'tr'` entries are contiguous and already sorted. `AsNoTracking` skips snapshots and identity resolution; with a projection to a DTO EF tracks nothing anyway.
- Weak topics to review: composite index column order + change tracking (try `EXPLAIN` during T9), TCP/sockets (roadmap §0).

## 2026-11-17: Spec 001 / T8, review of the whole word catalog, mode [gen]

- Done: the `code-reviewer` agent reviewed spec 001 (T1–T7). Findings became T9 `[solo]` (tests for R2–R4 on the Postgres fixture) and T10 `[gen]` (small fixes); the auth impact is noted in spec 002 T6.
- Concepts: requirement traceability (every spec requirement has a test; R2–R4 had none), test fixture choice and test isolation (`WebApplicationFactory<Program>` reads the developer connection string, `PostgresApiFactory` uses a separate database, ADR-0013), enum model binding accepts numeric strings (`?order=5`), so validate with `[EnumDataType]`, one `SaveChangesAsync` = one transaction (no partial import), culture-aware sorting (`StringComparer.Create`) vs ordinal comparison, fallback authorization policy and the middleware order `UseAuthentication()` → `UseAuthorization()`.
- Docs:
  - https://learn.microsoft.com/aspnet/core/test/integration-tests
  - https://learn.microsoft.com/aspnet/core/mvc/models/validation
  - https://learn.microsoft.com/ef/core/saving/transactions
  - https://learn.microsoft.com/aspnet/core/security/authorization/secure-data
- In my own words:
- Check question: Why would a 200 test for `GET /api/words/cards` written with `WebApplicationFactory<Program>` break ADR-0013, while the 400 tests from T7 don't?
- fCC course: enums are in the course; model binding, EF transactions and authorization go beyond the course.

## 2026-11-15: Spec 001 / T7, integration tests for R1 (400 without a database), mode [solo]

- You wrote: `tests/WordWise.Api.Tests/Features/Words/WordsEndpointTests.cs`, one test per R1 rule (missing `base`, unsupported `base`, `base` = `target`). Each asserts the 400 status and the field key in `Errors`.
- Concepts: integration test vs unit test (the whole pipeline: routing → model binding → validation), `WebApplicationFactory<Program>` + `IClassFixture`, automatic HTTP 400 from `[ApiController]` (the action, and so the database, is never reached), `HttpResponseMessage` vs a deserialized body (`ReadFromJsonAsync<ValidationProblemDetails>`), `await` instead of `.Result` (xUnit1031), null-state analysis (CS8602) and `ShouldNotBeNull()`, `Errors` as a dictionary field name → messages, one variable per test (only the tested parameter is wrong), LF vs CRLF line endings (`dotnet format`).
- Docs:
  - https://learn.microsoft.com/aspnet/core/test/integration-tests
  - https://learn.microsoft.com/aspnet/core/web-api/#automatic-http-400-responses
  - https://learn.microsoft.com/dotnet/api/system.net.http.json.httpcontentjsonextensions.readfromjsonasync
  - https://learn.microsoft.com/dotnet/csharp/language-reference/compiler-messages/nullable-warnings#possible-dereference-of-null
- In my own words:
- Check question: Why does `GetCards_SameBaseAndTarget_Returns400` pass without Postgres running, while a test expecting 200 from the same endpoint needs a database?
- fCC course: `var` and method calls are in the course; async/await, nullable reference types and ASP.NET Core integration tests go beyond the course.

## 2026-11-13: Spec 001 / T5, `WordSeedImporter` (idempotent seed import at Development startup), mode [learn]

- You wrote: `TODO(human)` 1–4 in `WordSeedImporter.ImportAsync`: skip when the catalog already has concepts (`AnyAsync`, R5), reject unsupported language codes before adding anything, map each seed to a `Concept` with one `Word` per form, save with one `SaveChangesAsync` and return `seeds.Count`.
- Concepts: validate-then-act (check all input before changing state), pattern matching with constant and logical patterns (`is not (A or B or C)`, ordinal and case-sensitive for strings), `const` fields usable in attributes but not as a collection, object initializer (`new Concept { Key = … }`, not a TypeScript object literal), adding a graph of new entities (`Add` on the parent tracks the children reachable through `Words`), relationship fixup (EF inserts the `Concept`, reads the generated `Id`, sets `ConceptId` on the `Word`s), change tracker vs database (`Add` is in memory, `CountAsync` queries the database), one `SaveChangesAsync` = one transaction, `SaveChangesAsync` returns the number of all rows written (concepts + words), test output is shown only for failing tests.
- Docs:
  - https://learn.microsoft.com/dotnet/csharp/language-reference/operators/patterns#logical-patterns
  - https://learn.microsoft.com/dotnet/csharp/fundamentals/strings/common-tasks/compare#compare-against-constants-with-pattern-matching-is-or-switch
  - https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers
  - https://learn.microsoft.com/ef/core/saving/related-data#adding-a-graph-of-new-entities
  - https://learn.microsoft.com/ef/core/change-tracking/relationship-changes#overview-of-foreign-keys-and-navigations
  - https://learn.microsoft.com/ef/core/saving/transactions#default-transaction-behavior
- In my own words: Saving Concepts and their Words in the database + checking and validation data
- Check question: Why does `await db.Concepts.CountAsync()` return 0 right after `db.Concepts.Add(concept)` without `SaveChangesAsync`?
- fCC course: nested `foreach`, `if`, `throw` and `new` are in the course; pattern matching, object initializers and EF Core change tracking go beyond the course.

## 2026-11-11: Spec 001 / T4, `WordSeedCsvParser` invalid input and edge cases (decisions 3–12), mode [solo]

- You wrote: validation in `WordSeedCsvParser.Parse` (rank not a number, rank ≤ 0, empty synonym, empty forms column, empty `concept_key`, no lines, blank lines) and the tests for each decision (PR #38).
- Concepts: Try-Parse pattern (`int.TryParse` with `out` and `CultureInfo.InvariantCulture`), definite assignment (CS0165) and short-circuit `||`, IDE0045 as a style suggestion only, `[Theory]` + `[InlineData]` (one method, one run per data row), fail fast (one bad line per test case), Arrange–Act–Assert (a test without an assertion always passes; a `Skip`ped test does not run), `"".Split('|')` returns `[""]`, multiple enumeration of `IEnumerable` (CA1851), `continue` inside the loop keeps file line numbers, string interpolation uses the current culture, a red test can mean a wrong expectation.
- Docs:
  - https://learn.microsoft.com/dotnet/api/system.int32.tryparse?view=net-10.0
  - https://learn.microsoft.com/dotnet/csharp/language-reference/compiler-messages/cs0165
  - https://learn.microsoft.com/dotnet/fundamentals/code-analysis/style-rules/ide0045
  - https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1851
  - https://learn.microsoft.com/dotnet/core/testing/unit-testing-csharp-with-xunit#create-a-test
  - https://learn.microsoft.com/dotnet/core/testing/unit-testing-best-practices#best-practices
- In my own words: _…_
- Check question: Why does a blank line have to be skipped with `continue` inside the loop and not with `Where` before it?
- fCC course: `if`/`else if`, loops, parsing numbers and debugging are in the course; `out` with definite assignment, LINQ, xUnit `[Theory]` and cultures go beyond the course.

## 2026-10-18: Spec 001 / T6, generated SQL + `TODO(human)` #3: index for the card query, mode [learn]

- You wrote: `HasIndex(w => new { w.Language, w.FrequencyRank })` and the migration `AddWordsLanguageRankIndex`.
- Concepts: SQL generated by EF Core in the console log (`Executed DbCommand`, parameter values hidden by default), `Any` → `EXISTS`, required navigation → `INNER JOIN`, composite B-tree index (usable from the first column: equality filter first, sort column second), primary key already has its own unique index, `dotnet ef migrations remove --force` for a migration that is applied locally but not committed, a new Compose volume needs `database update` (error 42P01), debugger vs `dotnet run`.
- Docs:
  - https://learn.microsoft.com/ef/core/modeling/indexes#composite-index
  - https://learn.microsoft.com/ef/core/managing-schemas/migrations/managing#remove-a-migration
  - https://learn.microsoft.com/ef/core/logging-events-diagnostics/simple-logging
- In my own words: Index which helps to find faster word cards, Id does not help a query because it is a main key which already has unique index.
- Check question: Why does an index on (`Id`, `FrequencyRank`) not help a query that filters by `Language` and sorts by `FrequencyRank`?
- fCC course: beyond the course (EF Core, SQL indexes); debugging is in the course.
- Deferred to the review phase: LINQ ↔ SQL practice and stored procedures (`docs/learning/roadmap.md` §3).

## 2026-10-13: Spec 001 / T6, `TODO(human)` #2: culture-aware translation sort + random order, mode [learn]

- You wrote: `WordQueries.SortTranslationsAndApplyOrder` and its unit tests (Polish letters, `CardOrder.Random`).
- Concepts: culture-sensitive comparison (`StringComparer.Create(culture, ignoreCase: false)`: `ć` after `c`, `ł` after `l`; ordinal sort puts them at the end), LINQ projection with `Select` (= `map` in JS) vs `List<T>.ForEach` (returns `void`), deferred execution (`OrderBy` returns a lazy `IOrderedEnumerable<T>`; `ToList()`/`ToArray()` runs it), implicit reference conversion (`List<string>` and `T[]` implement `IReadOnlyList<T>`), non-destructive mutation of a record with `with`, in-place `Random.Shared.Shuffle(T[])` (returns `void`, no overload for `List<T>`), statement vs expression lambda, testing randomness (assert the set with `ignoreOrder: true`, find items by key with `Single`, never by index), hard-coded expected values instead of values from the input, analyzers xUnit1013 and CA1822 on a test method without `[Fact]`, collection expressions (IDE0305, `[.. x]`).
- Docs:
  - https://learn.microsoft.com/dotnet/standard/base-types/best-practices-strings
  - https://learn.microsoft.com/dotnet/csharp/language-reference/operators/with-expression
  - https://learn.microsoft.com/dotnet/api/system.random.shuffle
  - https://learn.microsoft.com/dotnet/csharp/language-reference/operators/lambda-expressions (not checked via MCP)
  - https://learn.microsoft.com/dotnet/standard/linq/deferred-execution-lazy-evaluation (not checked via MCP)
- In my own words:
- Check question: Why can't the `CardOrder.Random` test check `result[0].Translations`, and why must it not compare with `cards[i].Translations` from the input?
- fCC course: arrays, zero-based indexes, `if`, methods; LINQ, lambdas, records and `with` are beyond the course.

## 2026-10-08: Spec 001 / T6, `WordsController` + card query skeleton, mode [learn]

- Claude wrote: controller, DTOs (`WordCardsRequest`, `WordCardResponse`, `CardOrder`), R1 validation, the `WordQueries` feature service and its DI registration, OpenAPI contract and the generated Angular client. After the review also: problem+json for 400, integer numbers and language `enum` in the contract, lowercase routes. **You write:** `TODO(human)` #1 (EF Core query, R2–R4), #2 (unit tests first, then the culture-aware sort of translations + random order in `SortTranslationsAndApplyOrder`), #3 (second index on `Words` + migration).
- Concepts: thin controller + feature class (ADR-0002), `[ApiController]` automatic 400 with `ValidationProblemDetails`, model binding of a complex type from the query string (`[FromQuery(Name = "base")]`, because `base` is a C# keyword), `[BindRequired]` (binding: key present) vs the implicit `[Required]` that MVC adds for non-nullable reference types (validation: value not empty; `?base=` binds as null), `[Display(Name)]` = the name in default messages, `[AllowedValues]` with `const` fields and an `ErrorMessage` without C# names, class-level validation with `IValidatableObject` (runs only after property validation passed), enum in OpenAPI (`integer` without `[JsonConverter]`, string `enum` with `JsonStringEnumConverter<T>` + `[JsonStringEnumMemberName]`), `[EndpointName]` = OpenAPI `operationId`, `ActionResult<T>` has no implicit conversion from an interface type, so `Ok(cards)`.
- Concepts from the review fixes:
  - **problem+json content type** (RFC 9457): `[Produces("application/json")]` is also a result filter and turned the automatic 400 into `application/json`; content types per status code go in `[ProducesResponseType]`.
  - **`JsonNumberHandling`**: the Web default `AllowReadingFromString` makes `int` `["integer", "string"]` in the contract (TS `number | string`); `Strict` is set twice, in the HTTP JSON options (read by the OpenAPI generator) and the MVC JSON options (controllers at runtime).
  - **OpenAPI schema transformer** (`IOpenApiSchemaTransformer`): `[AllowedValues]` doesn't set `enum`, so `AllowedValuesSchemaTransformer` copies the values (TS: `'en' | 'pl' | 'tr'`); `[DefaultValue]` → `default: "sequential"`.
  - **Slugify route convention**: `IOutboundParameterTransformer` + `RouteTokenTransformerConvention` turn `[controller]` into lowercase kebab-case (`/api/words/cards`); a regex always gets a timeout.
  - **Friend assembly** (`InternalsVisibleTo`): `internal` pure logic can be unit-tested without a database.
- Docs:
  - https://learn.microsoft.com/aspnet/core/web-api/#automatic-http-400-responses
  - https://learn.microsoft.com/aspnet/core/mvc/models/validation#ivalidatableobject
  - https://learn.microsoft.com/aspnet/core/mvc/models/validation#non-nullable-reference-types-and-required-attribute
  - https://learn.microsoft.com/aspnet/core/mvc/models/model-binding#complex-types
  - https://learn.microsoft.com/aspnet/core/fundamentals/openapi/include-metadata#include-openapi-metadata-for-endpoints
  - https://learn.microsoft.com/aspnet/core/fundamentals/openapi/include-metadata#include-openapi-metadata-for-data-types
  - https://learn.microsoft.com/aspnet/core/fundamentals/openapi/include-metadata#set-json-serialization-options-globally
  - https://learn.microsoft.com/aspnet/core/fundamentals/openapi/customize-openapi#use-schema-transformers
  - https://learn.microsoft.com/aspnet/core/mvc/controllers/routing#use-a-parameter-transformer-to-customize-token-replacement
  - https://learn.microsoft.com/aspnet/core/web-api/action-return-types#actionresultt-type
  - https://learn.microsoft.com/dotnet/standard/assembly/friend
  - https://learn.microsoft.com/ef/core/querying/client-eval
- Verified (temporary probe, not committed, T7 is yours): `base=tr&target=tr`, `base=xx`, missing `base`, `base=`, `base=EN`, `order=foo` → 400 with `Content-Type: application/problem+json` (also with `Accept: application/json`) and messages without C# names ("Allowed values: en, pl, tr.", "The base field is required.", "A value for the 'base' parameter or property was not provided.", "The value 'foo' is not valid for order."); `base=pl&target=tr` and `order=RANDOM` pass validation (500 from the `NotImplementedException` until the TODOs are done); `/api/words/cards` works (routing is case-insensitive, so `/api/Words/cards` too).
- Known gap (accepted): `?order=1` binds to `Random`, because `Enum.TryParse` also accepts numbers.
- Fun fact: `order=RANDOM` works although the OpenAPI document says `random`: query binding uses `Enum.TryParse` (case-insensitive), not the JSON converter.
- In my own words:
- Check question: Why does the base ≠ target rule live in `IValidatableObject` and not in an attribute on one property, and when does `Validate` not run at all?
- fCC course: enums and methods are in the course; model binding, validation and OpenAPI are beyond the course (ASP.NET Core).

## 2026-10-08: ADR-0013, Postgres fixture for integration tests (one database per test run), mode [gen]

- Concepts: custom `WebApplicationFactory<Program>` (`ConfigureWebHost`), configuration override with `ConfigureAppConfiguration` + `AddInMemoryCollection` (the source added last wins; it is visible only after `Build()`, which works because `Program.cs` reads the connection string lazily inside `AddDbContext`), `NpgsqlConnectionStringBuilder` (change only `Database`, keep server and credentials from the app's config), xUnit v3 `IAsyncLifetime` (`InitializeAsync` / `DisposeAsync`; xUnit calls `DisposeAsync` even when `InitializeAsync` threw), collection fixture vs assembly fixture (xUnit v3 creates assembly fixtures when the test assembly starts, so without Docker every test would fail; a collection fixture is created only when one of its tests runs), `MigrateAsync` creates the database if it doesn't exist, `EnsureDeletedAsync` drops it, test isolation by database (`wordwise_test_<8 hex>` per run), exception wrapping (`NpgsqlException` that is not a `PostgresException` = no connection), CA1711 (the `Collection` suffix is reserved for real collections, hence `PostgresCollectionDefinition`).
- Docs:
  - https://learn.microsoft.com/ef/core/testing/testing-with-the-database
  - https://learn.microsoft.com/aspnet/core/test/integration-tests#customize-webapplicationfactory (and the section "Customize the WebApplicationFactory with test configurations")
  - https://learn.microsoft.com/dotnet/core/extensions/configuration-providers#memory-configuration-provider
  - https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.relationaldatabasefacadeextensions.migrateasync
  - https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1711
  - https://xunit.net/docs/shared-context and https://xunit.net/docs/running-tests-in-parallel (xunit.net, not checked via MCP)
  - https://docs.github.com/en/actions/using-containerized-services/creating-postgresql-service-containers (GitHub docs, not checked via MCP)
- Fun fact: the first plan was an assembly fixture; reading the xUnit v3 source showed it is created at assembly start, so `HealthEndpointTests` would fail without Docker, against ADR-0013.
- In my own words: (1) An assembly fixture is created when the test run starts, so if Postgres is down it fails every test, even `HealthEndpointTests`; a collection fixture is created only when one of its tests runs, so only the database test fails. (2) The lambda in `AddDbContext` is only a recipe: it runs later, when the first `DbContext` is created, and by then `Build()` has already added the test database from `ConfigureAppConfiguration`. If `Program.cs` read the connection string before `Build()`, it would keep the dev database, and `GetTestDbContext` would stop the test before migrating or dropping it. (3) Idempotent means running the import twice leaves the same data as running it once. The database is shared by the whole run, so the test must not assume it starts empty: import, count, import again, and check the count did not change.
- Check question: Why are the database tests grouped in a collection fixture and not an assembly fixture, and what happens to `HealthEndpointTests` and to the database test when Docker is stopped?
- fCC course: beyond the course (integration testing, xUnit fixtures, EF Core migrations at runtime, CI service containers).

## 2026-10-08: Decision drills: test database (ADR-0013) and Azure hosting (ADR-0014), `/compare-options`

- Concepts: decision criteria with weights, reversibility as a criterion, test doubles vs the real database (EF Core in-memory provider and SQLite as fakes), test isolation (one database per test run), serverless containers and scale to zero, revisions and rollback, one origin for cookie auth (ADR-0004), forwarded headers behind an ingress, Data Protection key ring, globalization-invariant mode and ICU, deferring a decision to "the last responsible moment" (production database).
- Docs:
  - https://learn.microsoft.com/ef/core/testing/choosing-a-testing-strategy
  - https://learn.microsoft.com/azure/container-apps/billing
  - https://learn.microsoft.com/aspnet/core/host-and-deploy/scaling-aspnet-apps/scaling-aspnet-apps
  - https://learn.microsoft.com/dotnet/core/docker/container-images
- Your choices vs Claude's recommendation: test database D (the shared Postgres; Claude: Testcontainers), fixed by one database per run; compute Container Apps (same); SPA served by the API (your first idea: nginx; changed after the trade-offs); production database deferred.
- In my own words: Using Container Apps from Azure for hosting the app, SPA served by the API from wwwroot. Hosting comes after the app is built; if Azure turns out expensive, I may pick another provider.
- Check question: Why would every deploy on Container Apps sign all users out if nobody thinks about Data Protection keys, and why does an nginx container in front of the API make the forwarded-headers problem harder?
- fCC course: beyond the course (architecture, cloud).

## 2026-10-08: fix, ProblemDetails for unhandled errors and unknown routes (security.md "Fail safely"), mode [gen]

- Concepts: problem details service (`AddProblemDetails` registers `IProblemDetailsService`, RFC 9457, `application/problem+json`), exception handler middleware (`UseExceptionHandler()` without a path writes a generic 500 ProblemDetails), developer exception page (`WebApplication` adds it by itself in Development; with `AddProblemDetails` it answers a non-HTML `Accept` with ProblemDetails that include the stack trace), status code pages middleware (fills only error responses without a body, e.g. 404 for an unknown route), middleware order (error handling first, so it wraps the rest), environments in tests (`UseEnvironment("Production")` in `WithWebHostBuilder`), MVC application parts (a test-only controller from the test assembly), CA1822 vs MVC actions (a static method is not an action), `?.` short-circuiting.
- Docs:
  - https://learn.microsoft.com/aspnet/core/fundamentals/error-handling-api#problem-details
  - https://learn.microsoft.com/aspnet/core/fundamentals/error-handling#problem-details
  - https://learn.microsoft.com/aspnet/core/fundamentals/error-handling#developer-exception-page
  - https://learn.microsoft.com/aspnet/core/test/integration-tests#inject-mock-services
  - https://learn.microsoft.com/aspnet/core/mvc/advanced/app-parts
  - https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1822
  - https://learn.microsoft.com/dotnet/csharp/fundamentals/null-safety/null-operators
- Fun fact: the first version of the 404 test passed even without the fix. In `ContentType?.MediaType.ShouldBe(...)` the `?.` short-circuits when there is no `Content-Type`, so `ShouldBe` never ran. Running the new tests against the old `Program.cs` (red check) caught it.
- In my own words: Outside Development `UseExceptionHandler` returns a generic 500, so the client never learns the details of the problem: our classes, the depths of the code, the stack trace. In Development `WebApplication` itself adds `DeveloperExceptionPageMiddleware` at the very start of the pipeline; `UseExceptionHandler` would sit deeper, catch the exception first and cover it with the generic 500, hiding the debugging info from the developer. `UseStatusCodePages` leaves `NotFound()` and `ValidationProblem` alone because those responses already have a body, while an unknown route ends with an empty 404, so that one gets filled.
- Check question: An action throws an exception. What does the client get in Development and what in Production (status, `Content-Type`, which fields), and which middleware writes each response?
- fCC course: beyond the course (ASP.NET Core middleware, error handling).

## 2026-10-08: fix, Label in Name for the language switch (WCAG 2.2 SC 2.5.3), mode [gen]

- Concepts: accessible name vs visible label, SC 2.5.3 Label in Name (level A: the accessible name contains the visible text, best at the start), speech input ("click TR" matches by the name), `aria-label` overrides the button text in the accessible name, ARIA attribute binding in Angular (`[aria-label]` / `[attr.aria-label]`), the tooltip is not part of the name.
- Docs:
  - https://www.w3.org/WAI/WCAG22/Understanding/label-in-name.html
  - https://angular.dev/guide/templates/binding#aria-attributes
  - https://angular.dev/best-practices/a11y#aria-attributes-and-properties
  - https://developer.mozilla.org/en-US/docs/Glossary/Accessible_name
- In my own words: aria-label having aria-label with the full name of the language and shortcut - a speech-input user saying "click TR" will hit the button.
- Check question: The button shows "TR". Why does `aria-label="Türkçe"` fail SC 2.5.3 while `aria-label="TR, Türkçe"` passes, and who is affected by the difference?
- fCC course: beyond the course (frontend, accessibility).

## 2026-10-08: chore, CI check for a missing migration (`has-pending-model-changes`), mode [gen]

- Concepts: model snapshot (`WordWiseDbContextModelSnapshot.cs`) as the "last known model", `dotnet ef migrations has-pending-model-changes` compares the current model with the snapshot (exit 1 = a migration is missing), no database connection is opened, design-time host (the tools run `Program.cs` and stop before the app starts), design-time tools default to the `Development` environment, `--no-build --configuration Release` reuses the CI build, local tools (`dotnet tool restore`).
- Docs:
  - https://learn.microsoft.com/ef/core/managing-schemas/migrations/managing#checking-for-pending-model-changes
  - https://learn.microsoft.com/ef/core/managing-schemas/migrations/applying#environment-and-configuration
  - https://learn.microsoft.com/ef/core/cli/dbcontext-creation#from-application-services
- Fun fact: the check passed even with a connection string pointing to port 1 (no server there). So it really does not touch the database.
- In my own words: It checks whether the current EF model (entities + configuration) differs from the model snapshot in /Migrations, i.e. whether someone changed the model without adding a migration. No database needed, because both sides of the comparison are C# code.
- Check question: Why does the CI step need neither Postgres nor a `ConnectionStrings__WordWise` variable, but would fail if `appsettings.Development.json` had no connection string?
- fCC course: beyond the course (EF Core migrations, CI).

## 2026-10-08: Spec 001 / T3, migration `InitialWordCatalog` + `dotnet ef database update`, mode [gen]

- Concepts: code-first migrations (`Up`/`Down`, model snapshot, `__EFMigrationsHistory`), mapping C# → PostgreSQL types (`string` + `HasMaxLength(n)` → `character varying(n)`, `int?` → nullable `integer`), identity columns (`generated by default as identity`), FK constraint with `ON DELETE CASCADE`, convention names (`PK_`, `FK_<Dependent>_<Principal>_<FK>`, `IX_<Table>_<Columns>`), `Down` drops tables in reverse order because of the FK, `generated_code = true` in `.editorconfig` so analyzers (CA1861) skip generated code.
- Docs:
  - https://learn.microsoft.com/ef/core/managing-schemas/migrations/
  - https://learn.microsoft.com/ef/core/managing-schemas/migrations/applying#command-line-tools
  - https://learn.microsoft.com/ef/core/modeling/relationships/conventions#indexes
  - https://learn.microsoft.com/dotnet/fundamentals/code-analysis/configuration-options#general-options
- Prediction vs result: tables, columns, lengths, required FK, 1-N and both unique indexes were predicted correctly. Not in the database: "ConceptId cannot be 0" and "FrequencyRank 0–1000" are not constraints (0 is rejected only because no Concept has Id 0; the rank range must be validated in the import or with a check constraint). Not predicted: identity `Id`, `ON DELETE CASCADE`, no separate `IX_Words_ConceptId`.
- Fun fact: `dotnet ef` writes migrations with CRLF and `new[] { ... }`, so `dotnet format` (LF) and the CA1861 analyzer failed until `Migrations/` was marked as generated code.
- In my own words: Launching the migrations which created basic structures of database using prepared model. EFMigrationsHistory Table keeps Migration Id and ProductVersion. Second update without changes apply no migrations, it is already up to date.
- Check question: What does `__EFMigrationsHistory` store, and what happens when you run `dotnet ef database update` a second time?
- fCC course: beyond the course (EF Core migrations, SQL schema).

## 2026-10-08: chore, fixed Compose project name (`name: wordwise`), mode [gen]

- Concepts: Compose project name (defaults to the folder name, so each git worktree was a separate project), `container_name` is global per Docker host while volume names get the project prefix, top-level `name:` in `compose.yaml`.
- Docs:
  - https://docs.docker.com/compose/how-tos/project-name/
- In my own words:
- Check question: Before the fix, why did `docker compose up -d` in a second worktree fail on the container name, and why would it have started with an empty database if it had worked?
- fCC course: beyond the course (Docker Compose).

## 2026-10-07: Spec 001 / T2, `Concept` and `Word` entities + `IEntityTypeConfiguration`, mode [learn]

- Concepts: entity vs navigation vs foreign key (FK is a column, navigations exist only in C#), required one-to-many (non-nullable FK → required → cascade delete by convention), `= null!` for a reference navigation filled by EF, Fluent API (`IsRequired`, `HasMaxLength` as a data contract), unique index as a business rule, composite index and the leftmost-prefix rule, EF skips the FK index when an existing index already starts with the FK. One-to-many vs many-to-many (join table, with payload = `UserWordProgress`) vs one-to-one (FK + unique index).
- Docs:
  - https://learn.microsoft.com/ef/core/modeling/relationships/one-to-many#required-one-to-many
  - https://learn.microsoft.com/ef/core/modeling/relationships/many-to-many
  - https://learn.microsoft.com/ef/core/modeling/indexes#composite-index
  - https://learn.microsoft.com/ef/core/modeling/relationships/conventions#indexes
  - https://learn.microsoft.com/ef/core/saving/cascade-delete
  - https://learn.microsoft.com/ef/core/miscellaneous/nullable-reference-types
- Fun fact: `dotnet build` passed, but `dotnet format --verify-no-changes` failed on `FINALNEWLINE` (missing newline at end of file), which CI would catch.
- In my own words: Adding models (entities) for the Concept and Word as a preparation to keep in the database
- Check question: The unique index is `(ConceptId, Language, Text)`. Why does it not speed up `WHERE Language = 'tr'`, and why will the migration have no separate `IX_Words_ConceptId`?
- fCC course: beyond the course (EF Core modeling, relationships, indexes); types, `int?` and properties are from the course.

## 2026-10-05: Spec 001 / T1, EF Core + Npgsql + `WordWiseDbContext`, mode [gen]

- Concepts: `DbContext` as a unit of work, `AddDbContext` = scoped lifetime (one context per HTTP request), `DbContextOptions<T>` passed through a primary constructor, configuration keys (`ConnectionStrings:WordWise`, `GetConnectionString`, env var `ConnectionStrings__WordWise`), `ApplyConfigurationsFromAssembly`, Central Package Management and pinning a transitive package (MSB3277), local .NET tools (`dotnet-tools.json`, `dotnet tool restore`).
- Docs:
  - https://learn.microsoft.com/ef/core/dbcontext-configuration/#dbcontext-in-dependency-injection-for-aspnet-core
  - https://learn.microsoft.com/dotnet/core/extensions/dependency-injection/service-lifetimes#scoped
  - https://learn.microsoft.com/ef/core/miscellaneous/connection-strings#aspnet-core
  - https://learn.microsoft.com/ef/core/modeling/#grouping-configuration
  - https://learn.microsoft.com/dotnet/core/tools/local-tools-how-to-use
- Fun fact: Npgsql 10.0.3 depends on EF Core 10.0.4, `Design` on 10.0.12. The API built fine, but the test project saw both versions (MSB3277), so `Microsoft.EntityFrameworkCore.Relational` is pinned in `Directory.Packages.props`.
- In my own words: Setting the DB Context, Using EF Core Framework and definition of connection with Database by PSQL standing locally on the Docker container
- Check question: Why must a singleton service never take `WordWiseDbContext` in its constructor, and what do you use instead (hint: T5 `IHostedService`)?
- fCC course: beyond the course (DI, EF Core, configuration).

## 2026-10-05: Language switch styling from Figma, mode [learn]

- Concepts: design tokens as CSS custom properties, Tailwind v4 CSS-variable shorthand (`bg-(--token)`), data attribute variants (`data-[state=on]:`), stacking variants, conflicts decided by utility order in the generated CSS (not in `class`), overriding helm variant classes, non-text contrast.
- Docs:
  - https://tailwindcss.com/docs/hover-focus-and-other-states#data-attributes
  - https://www.w3.org/WAI/WCAG22/Understanding/non-text-contrast.html
- In my own words: Adjusting language switch using Tailwind & template optimalization
- Check question: Why does `data-[state=on]:bg-(--primary)` style only the active item without any `[class.x]` binding in the template?
- fCC course: beyond the course (frontend).

## 2026-10-04: Language switch with spartan Toggle Group, mode [learn]

- Concepts: helm as owned code (shadcn-style copy, not a library), host directives and their inputs (`[aria-label]` instead of `[attr.aria-label]`), `as const` and literal types, `(typeof X)[number]` (indexed access type), type predicate (`value is LanguageCode`) to narrow `unknown`, `toUpperCase()` for identifiers vs `toLocaleUpperCase('tr')` for text, `it.each` argument spreading, testing a pure function without TestBed.
- Docs:
  - https://www.spartan.ng/components/toggle-group
  - https://angular.dev/guide/directives/directive-composition-api
  - https://www.typescriptlang.org/docs/handbook/release-notes/typescript-3-4.html#const-assertions
  - https://www.typescriptlang.org/docs/handbook/2/narrowing.html#using-type-predicates
  - https://vitest.dev/api/#test-each
- Fun fact: removing `as const` made the build pass, because `LanguageCode` silently became `string` and the type guard stopped guarding anything. A green build is not proof that types work.
- In my own words: _…_
- Check question: Why does `it.each([['pl', 'en', 'tr']])` run only one test, and what does `value` hold in it?
- fCC course: beyond the course (TypeScript, frontend testing).

## 2026-10-03: Learning roadmap for full-stack delivery + spartan MCP, mode [gen]

- Concepts: MCP server as a documentation source, supply-chain risk of `npx -y` without a version (pinning), decision drill (criteria → options → trade-offs → "revisit when"), learning roadmap tied to project milestones.
- Docs:
  - https://www.spartan.ng/documentation/mcp
  - https://learn.microsoft.com/azure/well-architected/pillars
  - https://learn.microsoft.com/ef/core/managing-schemas/migrations/applying
- In my own words: _…_
- Check question: Why does `.mcp.json` pin `@spartan-ng/mcp@1.5.0` instead of `npx -y @spartan-ng/mcp`, and when should we bump it?
- fCC course: beyond the course (delivery).

## 2026-10-02: OpenAPI document at build time + generated Angular client, mode [gen]

- Concepts: API contract as the source of truth, build-time OpenAPI generation (MSBuild properties, `PrivateAssets`), code generation from a contract, "generated code is never edited", CI drift check (`git diff --exit-code`), Observables vs Promises for HTTP.
- Docs:
  - https://learn.microsoft.com/aspnet/core/fundamentals/openapi/aspnetcore-openapi#generate-openapi-documents-at-build-time
  - https://github.com/cyclosproject/ng-openapi-gen
- Fun fact: build-time generation starts the app's `Program.cs` with a mock server, so anything that needs real infrastructure (the database in spec 001) must be skipped in that mode.
- In my own words: _…_
- Check question: You add a new property to a response DTO in C#. Which two files change after `npm run api`, and what does CI do if you commit only the C# change?
- fCC course: beyond the course (.NET tooling, MSBuild).

## 2026-10-01: spartan/ui setup + WordWise theme, mode [gen]

- Concepts: headless primitives (brain) vs copied, styled components (helm), CSS custom properties as design tokens, `@theme inline`, OKLCH colors, WCAG contrast ratio (relative luminance), self-hosted fonts and `unicode-range`, npm `overrides` for transitive vulnerabilities, TypeScript path aliases.
- Docs:
  - https://www.spartan.ng/documentation/installation
  - https://www.spartan.ng/documentation/theming
  - https://www.w3.org/WAI/WCAG22/Understanding/non-text-contrast.html
  - https://docs.npmjs.com/cli/v11/configuring-npm/package-json#overrides
- Fun fact: the input border color was far too light (#E4E4E7 in Figma, #D4D4D8 in the first code draft: at most 1.48:1 on white) contrast; WCAG 1.4.11 needs 3:1 for component boundaries, so it became #8A8A93 (3.42:1). `npm audit fix` made things worse (downgraded nx, 15 findings); scoped `overrides` fixed it (0 findings).
- In my own words: _…_
- Check question: Why is `@spartan-ng/helm/button` not an npm package, and what does that mean when spartan releases a new version?
- fCC course: beyond the course (frontend).

## 2026-10-01: App header in Tailwind utilities, mode [learn]

- Concepts: spacing scale (`gap-4` = 4 × 0.25rem), variant + utility (`aria-pressed:bg-zinc-800`), styling by state attribute, Preflight resets (headings, margins), `host` metadata instead of `:host`, Prettier plugins for class sorting and wrapping.
- Docs:
  - https://tailwindcss.com/docs/hover-focus-and-other-states
  - https://tailwindcss.com/docs/preflight
  - https://angular.dev/guide/components/host-elements
- Fun fact: `aria-pressed:zinc-800` silently generated nothing (a variant needs a full utility, e.g. `bg-zinc-800`). `prettier-plugin-tailwindcss` moved the unknown classes to the front of the list, which exposed the typo.
- In my own words: _…_
- Check question: Why is `aria-pressed:bg-zinc-800` better here than `[class.bg-zinc-800]="isActive()"`?
- fCC course: beyond the course (frontend).

## 2026-10-01: Tailwind CSS setup + authentication framework plan, mode [gen]

- Concepts: utility-first CSS, PostCSS plugin, Tailwind v4 CSS-first config (`@import 'tailwindcss'`, `@theme`), class detection by plain-text scanning; cookie auth with Identity API endpoints, 401 instead of redirect for `[ApiController]` (.NET 10), route guard, HTTP interceptor.
- Docs:
  - https://angular.dev/guide/tailwind
  - https://tailwindcss.com/docs/styling-with-utility-classes
  - https://learn.microsoft.com/aspnet/core/security/authentication/identity-api-authorization
  - https://learn.microsoft.com/aspnet/core/security/authentication/api-endpoint-auth
- Fun fact: Tailwind v4 "is not designed to be used with CSS preprocessors like Sass", so global styles moved from `styles.scss` to `styles.css`. Native CSS already has nesting and variables.
- In my own words: _…_
- Check question: Why does `'bg-' + color` not work in Tailwind, while `[class.bg-red-600]="error()"` does?
- fCC course: beyond the course (frontend tooling, ASP.NET Core security).

## 2026-09-30: Repo setup, mode [gen]

- Concepts: `.slnx` solution, `Directory.Build.props`, Central Package Management, Roslyn analyzers (`TreatWarningsAsErrors`), `WebApplicationFactory`, Microsoft.Testing.Platform.
- Docs:
  - https://learn.microsoft.com/visualstudio/msbuild/customize-by-directory
  - https://learn.microsoft.com/nuget/consume-packages/central-package-management
  - https://learn.microsoft.com/aspnet/core/test/integration-tests
- Fun fact: analyzer **CA1707** blocked a test name with underscores. We disabled it only for `tests/**` in `.editorconfig`, because the `Method_Scenario_Expected` convention is common in tests.
- In my own words: _…_
- Check question: How does `Directory.Build.props` differ from `Directory.Packages.props`, and why is the package version not in the `.csproj`?
- fCC course: beyond the course (.NET tooling).
