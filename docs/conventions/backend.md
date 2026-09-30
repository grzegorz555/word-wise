# Konwencje backendu (C# / ASP.NET Core)

Bazujemy na oficjalnych wytycznych. Przy wątpliwości wygrywa dokumentacja:
- C# coding conventions: https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions
- Identifier naming: https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/identifier-names
- Framework Design Guidelines: https://learn.microsoft.com/dotnet/standard/design-guidelines/

## Nazewnictwo
| Element | Styl | Przykład |
|---|---|---|
| Klasy, rekordy, metody, właściwości | PascalCase | `StudySession`, `GetNextCard()` |
| Interfejsy | `I` + PascalCase | `IWordImporter` |
| Parametry, zmienne lokalne | camelCase | `targetLanguage` |
| Pola prywatne | `_camelCase` | `_dbContext` |
| Metody async | sufiks `Async` | `ImportAsync` |

## Struktura kodu
- Jeden typ publiczny na plik, plik nazwany jak typ. File-scoped namespaces (`namespace WordWise.Api.Features.Words;`).
- Namespace odzwierciedla folder.
- Klasy domyślnie `sealed`, jeśli nie są projektowane do dziedziczenia.
- Primary constructors dla DI w klasach serwisów/kontrolerów, np. `public sealed class WordsController(WordQueries queries) : ControllerBase`.

## API
- Routing: `[Route("api/[controller]")]`, rzeczowniki w liczbie mnogiej (`/api/words`, `/api/study/sessions`).
- Zwracaj `ActionResult<T>`. Błędy jako `ProblemDetails` (`Problem(...)`, `ValidationProblem(...)`).
- DTO jako `record` z sufiksem `Response`/`Request` (`WordCardResponse`).
- Walidacja wejścia: atrybuty DataAnnotations na DTO + `[ApiController]` (automatyczne 400).
- `CancellationToken cancellationToken` jako ostatni parametr akcji, przekazywany dalej.

## EF Core
- `AsNoTracking()` + `Select` do DTO dla odczytów.
- Konfiguracja w `Infrastructure/Data/Configurations/*Configuration.cs`.
- Migracje: `dotnet ef migrations add <Nazwa> --project src/api/WordWise.Api`. Nazwy migracji w PascalCase, opisowe.
- Nie wołaj `SaveChangesAsync` w pętli.

## Stringi i kultura
- Klucze, kody języków: `StringComparison.Ordinal`.
- Treść w danym języku: jawna `CultureInfo` (patrz ADR-0006, „Turkish I”).

## Logowanie
- `ILogger<T>` z message templates: `logger.LogInformation("Imported {Count} words", count)`, bez interpolacji `$""`. Reguła CA2254.
