# ADR-0003: EF Core + PostgreSQL

- Status: Accepted
- Data: 2026-09-30

## Decyzja
- PostgreSQL 18 w Dockerze (`compose.yaml`), provider `Npgsql.EntityFrameworkCore.PostgreSQL`.
- Code-first z migracjami EF Core. Konfiguracja przez `IEntityTypeConfiguration<T>` (Fluent API).
- Connection string w `appsettings.Development.json` / user-secrets, nigdy w kodzie.

## Konsekwencje
- Do uruchomienia API potrzebny jest Docker.
- Testy integracyjne docelowo na Testcontainers (backlog). Na start wystarczą testy bez bazy.

## Do nauki
- EF Core: https://learn.microsoft.com/ef/core/
- Migracje: https://learn.microsoft.com/ef/core/managing-schemas/migrations/
- Npgsql EF: https://www.npgsql.org/efcore/
- User secrets: https://learn.microsoft.com/aspnet/core/security/app-secrets
