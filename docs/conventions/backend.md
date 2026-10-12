# Backend conventions (C# / ASP.NET Core)

We follow the official guidelines. When in doubt, the documentation wins:
- C# coding conventions: https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions
- Identifier naming: https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/identifier-names
- Framework Design Guidelines: https://learn.microsoft.com/dotnet/standard/design-guidelines/

## Naming
| Element | Style | Example |
|---|---|---|
| Classes, records, methods, properties | PascalCase | `StudySession`, `GetNextCard()` |
| Interfaces | `I` + PascalCase | `IWordImporter` |
| Parameters, local variables | camelCase | `targetLanguage` |
| Private fields | `_camelCase` | `_dbContext` |
| Async methods | `Async` suffix | `ImportAsync` |

## Code structure
- One public type per file, file named after the type. File-scoped namespaces (`namespace WordWise.Api.Features.Words;`).
- The namespace mirrors the folder.
- Classes are `sealed` by default unless designed for inheritance.
- Primary constructors for DI in service/controller classes, e.g. `public sealed class WordsController(WordQueries queries) : ControllerBase`.

## API
- Routing: `[Route("api/[controller]")]`, plural nouns (`/api/words`, `/api/study/sessions`).
- Paths are lowercase kebab-case (`WordsController` → `/api/words`, `StudySessionsController` → `/api/study-sessions`) via the `SlugifyParameterTransformer` route convention in `Program.cs` (https://learn.microsoft.com/aspnet/core/mvc/controllers/routing#use-a-parameter-transformer-to-customize-token-replacement).
- Return `ActionResult<T>`. Errors as `ProblemDetails` (`Problem(...)`, `ValidationProblem(...)`).
- DTOs as `record` with a `Response`/`Request` suffix (`WordCardResponse`).
- Input validation: DataAnnotations attributes on DTOs + `[ApiController]` (automatic 400).
- `CancellationToken cancellationToken` as the last action parameter, passed down the call chain.

## EF Core
- `AsNoTracking()` + `Select` into a DTO for reads.
- Configuration in `Infrastructure/Data/Configurations/*Configuration.cs`.
- Migrations: `dotnet ef migrations add <Name> --project src/api/WordWise.Api`. Migration names in PascalCase, descriptive.
- Before pushing a model change, add a migration. CI checks it with `dotnet ef migrations has-pending-model-changes` (no database needed).
- Don't call `SaveChangesAsync` in a loop.

## Strings and culture
- Keys, language codes: `StringComparison.Ordinal`.
- Content in a given language: explicit `CultureInfo` (see ADR-0006, "Turkish I").

## Logging
- `ILogger<T>` with message templates: `logger.LogInformation("Imported {Count} words", count)`, no `$""` interpolation. Rule CA2254.
