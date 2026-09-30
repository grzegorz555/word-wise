# ADR-0001: Monorepo i stack technologiczny

- Status: Accepted
- Data: 2026-09-30

## Kontekst
Projekt solo, do nauki i portfolio. Pracuję z pomocą Claude Code, więc jeden wspólny kontekst jest tańszy i prostszy.

## Decyzja
- Jedno repo (GitHub): `src/api` (.NET), `src/web` (Angular), `tests/`, `docs/`, `data/`.
- **.NET 10 (LTS)** + ASP.NET Core, SDK przypięty w `global.json`.
- **Angular 22** (standalone, signals, zoneless). Pakiety `@angular/*` przypięte do `22.2.0`, bo w chwili startu `@angular/forms@22.2.1` nie był opublikowany i `^22.2.0` nie dawało się zainstalować.
- Central Package Management (`Directory.Packages.props`) i wspólne ustawienia (`Directory.Build.props`).
- Rozwiązanie w formacie `.slnx`.

## Konsekwencje
- Jedna historia gita i jedno CI. Claude widzi FE i BE naraz.
- Wersje NuGet zmieniasz w jednym pliku.

## Do nauki
- Central Package Management: https://learn.microsoft.com/nuget/consume-packages/central-package-management
- Directory.Build.props: https://learn.microsoft.com/visualstudio/msbuild/customize-by-directory
- global.json: https://learn.microsoft.com/dotnet/core/tools/global-json
