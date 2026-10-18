# ADR-0003: EF Core + PostgreSQL

- Status: Accepted
- Date: 2026-09-30

## Decision
- PostgreSQL 18 in Docker (`compose.yaml`), provider `Npgsql.EntityFrameworkCore.PostgreSQL`.
- Code-first with EF Core migrations. Configuration via `IEntityTypeConfiguration<T>` (Fluent API).
- Connection string in `appsettings.Development.json` / user-secrets, never in code.

## Consequences
- Docker is required to run the API.
- Integration tests will eventually use Testcontainers (backlog). Tests without a database are enough to start with. *Update 2026-10-08: decided in ADR-0013 (the shared Postgres, one database per test run).*

## Learning resources
- EF Core: https://learn.microsoft.com/ef/core/
- Migrations: https://learn.microsoft.com/ef/core/managing-schemas/migrations/
- Npgsql EF: https://www.npgsql.org/efcore/
- User secrets: https://learn.microsoft.com/aspnet/core/security/app-secrets
