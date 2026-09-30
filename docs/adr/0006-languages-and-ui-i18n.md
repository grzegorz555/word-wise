# ADR-0006: Języki EN/PL/TR i i18n interfejsu (Transloco)

- Status: Accepted
- Data: 2026-09-30

## Decyzja
- Obsługiwane języki: `en`, `pl`, `tr` (kody ISO 639-1). Każdy jest jednocześnie językiem UI i językiem nauki.
- Kierunek nauki: **dowolna para** (bazowy ≠ docelowy), niezależna od języka UI.
- i18n UI: **Transloco** (runtime, jeden build, pliki `src/web/public/i18n/*.json`), a nie `@angular/localize` (build per język, zmiana języka wymaga przeładowania).
- Język UI zapisywany w profilu użytkownika. Przed zalogowaniem: język przeglądarki albo domyślny `pl`.

## Konsekwencje
- Tekst porównujemy świadomie: tureckie `i/İ` i `ı/I` psują `ToUpper()`/`ToLower()` w domyślnej kulturze. W C# używamy `StringComparison.Ordinal`/`OrdinalIgnoreCase` dla kluczy i `CultureInfo.GetCultureInfo("tr")` przy porównaniach treści TR.
- Każdy nowy klucz tłumaczenia trzeba dodać w 3 plikach.

## Do nauki
- Transloco: https://jsverse.gitbook.io/transloco
- Porównywanie stringów w .NET: https://learn.microsoft.com/dotnet/standard/base-types/best-practices-strings
- „Turkish I problem”: https://learn.microsoft.com/dotnet/standard/base-types/best-practices-strings#string-operations-that-use-the-invariant-culture
