# ADR-0006: EN/PL/TR languages and UI i18n (Transloco)

- Status: Accepted
- Date: 2026-09-30

## Decision
- Supported languages: `en`, `pl`, `tr` (ISO 639-1 codes). Each is both a UI language and a learning language.
- Learning direction: **any pair** (base ≠ target), independent of the UI language.
- UI i18n: **Transloco** (runtime, single build, files `src/web/public/i18n/*.json`), not `@angular/localize` (build per language, switching language requires a reload).
- The UI language is stored in the user profile. Before login: the browser language or the default `pl`.

## Consequences
- We compare text deliberately: Turkish `i/İ` and `ı/I` break `ToUpper()`/`ToLower()` in the default culture. In C# we use `StringComparison.Ordinal`/`OrdinalIgnoreCase` for keys and `CultureInfo.GetCultureInfo("tr")` when comparing TR content.
- Every new translation key has to be added in 3 files.

## Learning resources
- Transloco: https://jsverse.gitbook.io/transloco
- String comparison in .NET: https://learn.microsoft.com/dotnet/standard/base-types/best-practices-strings
- "Turkish I problem": https://learn.microsoft.com/dotnet/standard/base-types/best-practices-strings#string-operations-that-use-the-invariant-culture
