# Backend (ASP.NET Core 10): context for Claude

Full conventions: `docs/conventions/backend.md`. Tests: `docs/conventions/testing.md`.

## Structure (vertical slices)
```
WordWise.Api/
  Program.cs                 # DI + pipeline; only Add*/Map* calls, no logic
  Features/<Feature>/        # e.g. Words/, Study/, Account/
    <Feature>Controller.cs   # thin controller: input validation → handler/service → HTTP result
    <UseCase>.cs             # request/response DTO + use-case logic
  Domain/                    # EF entities (Concept, Word, UserWordProgress, AppUser)
  Infrastructure/Data/       # WordWiseDbContext, IEntityTypeConfiguration configs, migrations
```

## Key rules
- The controller doesn't access `DbContext` directly; the feature code does. The controller returns `ActionResult<T>` / `ProblemDetails`.
- DTOs are `record`s. Entities are never returned from the API.
- Async everywhere there is I/O, with `CancellationToken` passed to EF.
- Read-only queries: `AsNoTracking()` + `Select` projection to a DTO.
- EF configuration via Fluent API (`IEntityTypeConfiguration<T>`), not attributes.
- Language as an ISO 639-1 code (`en`/`pl`/`tr`); comparisons via `StringComparison.Ordinal`/`CultureInfo`, never the default culture.
- The build has `TreatWarningsAsErrors` + `latest-recommended` analyzers. An analyzer warning is a learning opportunity: explain the rule with a link (CAxxxx) instead of disabling it.
