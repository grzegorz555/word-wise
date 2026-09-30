# Konwencje frontendu (Angular 22)

Źródło: Angular Style Guide: https://angular.dev/style-guide oraz best practices z Angular CLI MCP.

- Standalone, signals (`signal`, `computed`, `input()`, `output()`), zoneless, `@if/@for/@switch`.
- `inject()` zamiast konstruktora. `ChangeDetectionStrategy.OnPush` jest domyślne w zoneless, więc stan trzymaj w sygnałach.
- Pliki: `nazwa.ts`, `nazwa.html`, `nazwa.scss`, `nazwa.spec.ts` (konwencja Angular 20+ bez sufiksu `.component`).
- Foldery: `core/` (singletony: i18n, auth, http), `features/<feature>/` (strony + komponenty funkcji), `shared/` (komponenty prezentacyjne).
- Wszystkie teksty przez Transloco (`| transloco`), klucze `feature.element` (`study.revealButton`), dodawane w `pl`, `en`, `tr`.
- HTTP: serwisy w `features/<feature>/data-access/`, typy DTO odpowiadają kontraktowi OpenAPI.
- Dostępność: przyciski to `<button>`, stan przełączników w `aria-pressed`, etykiety dla nawigacji.
