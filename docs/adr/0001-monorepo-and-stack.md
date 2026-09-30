# ADR-0001: Monorepo and technology stack

- Status: Accepted
- Date: 2026-09-30

## Context
A solo project for learning and portfolio. I work with Claude Code, so a single shared context is cheaper and simpler.

## Decision
- One repo (GitHub): `src/api` (.NET), `src/web` (Angular), `tests/`, `docs/`, `data/`.
- **.NET 10 (LTS)** + ASP.NET Core, SDK pinned in `global.json`.
- **Angular 22** (standalone, signals, zoneless). `@angular/*` packages are pinned to `22.2.0` because at project start `@angular/forms@22.2.1` had not been published and `^22.2.0` could not be installed.
- Central Package Management (`Directory.Packages.props`) and shared settings (`Directory.Build.props`).
- Solution in the `.slnx` format.

## Consequences
- One git history and one CI. Claude sees FE and BE at the same time.
- NuGet versions are changed in a single file.

## Learning resources
- Central Package Management: https://learn.microsoft.com/nuget/consume-packages/central-package-management
- Directory.Build.props: https://learn.microsoft.com/visualstudio/msbuild/customize-by-directory
- global.json: https://learn.microsoft.com/dotnet/core/tools/global-json
