# ADR-0008: Testy i bramki jakości

- Status: Accepted
- Data: 2026-09-30

## Decyzja
- Backend: **xUnit v3** na **Microsoft.Testing.Platform** (`global.json` → `test.runner`) + **Shouldly** (asercje). FluentAssertions odrzucone, bo od v8 ma licencję komercyjną.
- Testy integracyjne API przez `WebApplicationFactory<Program>`.
- Frontend: Vitest (domyślny w Angular 22) + Prettier.
- `TreatWarningsAsErrors`, `AnalysisLevel=latest-recommended`, `EnforceCodeStyleInBuild`, `.editorconfig`, LF.
- CI (GitHub Actions): format check → build → test, osobno dla API i web.

## Konsekwencje
- Ostrzeżenia analyzera trzeba naprawić albo świadomie wyłączyć w `.editorconfig` z komentarzem, np. CA1707 w testach, żeby móc używać nazw `Method_Scenario_Expected`.

## Do nauki
- Testy integracyjne: https://learn.microsoft.com/aspnet/core/test/integration-tests
- Code analysis: https://learn.microsoft.com/dotnet/fundamentals/code-analysis/overview
- Microsoft.Testing.Platform: https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro
