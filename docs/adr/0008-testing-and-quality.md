# ADR-0008: Testing and quality gates

- Status: Accepted
- Date: 2026-09-30

## Decision
- Backend: **xUnit v3** on **Microsoft.Testing.Platform** (`global.json` → `test.runner`) + **Shouldly** (assertions). FluentAssertions rejected because it has a commercial license since v8.
- API integration tests via `WebApplicationFactory<Program>`.
- Frontend: Vitest (default in Angular 22) + Prettier.
- Unit tests are planned on both sides from the first feature: xUnit v3 for pure logic in the API, Vitest for services, guards, interceptors and components in the web app.
- `TreatWarningsAsErrors`, `AnalysisLevel=latest-recommended`, `EnforceCodeStyleInBuild`, `.editorconfig`, LF.
- CI (GitHub Actions): format check → build → test, separately for API and web.

## Consequences
- Analyzer warnings must be fixed or deliberately disabled in `.editorconfig` with a comment, e.g. CA1707 in tests, to allow `Method_Scenario_Expected` names.

## Learning resources
- Integration tests: https://learn.microsoft.com/aspnet/core/test/integration-tests
- Code analysis: https://learn.microsoft.com/dotnet/fundamentals/code-analysis/overview
- Microsoft.Testing.Platform: https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro
