# ADR-0013: Integration tests against the shared Postgres, one database per test run

- Status: Accepted
- Date: 2026-10-08

## Context
Spec 001 (T5: "importing 2× → the same number of words") and spec 002 (register → login → `me`, Identity tables) need tests against a real database. The migrations are Npgsql-specific (identity columns, `character varying(n)`), and card sorting depends on TR/PL culture (ADR-0006). Locally Postgres 18 runs from `compose.yaml`, and all git worktrees share one Compose project, so several sessions can run tests at the same time. CI runs on GitHub Actions `ubuntu-latest`.

## Decision criteria
1. Same behavior as production (Postgres 18, real migrations).
2. Isolation: parallel test runs and development data must not affect each other.
3. Learning value (industry practice).
4. Effort to set up and maintain; CI simplicity.
5. Speed.

## Considered options
1. **EF Core in-memory provider**: fastest to start, but not relational (no constraints, no transactions). The EF Core docs discourage it for testing.
2. **SQLite in-memory**: relational and fast, but a different SQL dialect; the Npgsql migrations can't run on it.
3. **Testcontainers**: a Postgres 18 container started by the test run. Fully self-contained (only Docker needed), the common industry choice; a few seconds of startup and one more package.
4. **The shared Postgres** (`compose.yaml` locally, a GitHub Actions service container in CI): same engine, no new dependency, fast. Needs `docker compose up -d` before running tests locally, and a shared server needs an isolation strategy.

## Decision
Option 4, chosen by the author in a `/compare-options` drill (Claude's recommendation was option 3). For isolation, **every test run gets its own database** on that server (`wordwise_test_<run id>`): a test fixture creates it, applies the migrations with `MigrateAsync`, points the app's connection string at it, and drops it at the end. The development database `wordwise` is never touched by tests.

## Consequences
- Tests that need the database fail with a clear message when Postgres is not running locally ("run `docker compose up -d`"); tests without a database keep working without Docker.
- CI: the `api` job gets a `postgres:18` service container and the test connection string as an environment variable (local, non-secret values).
- The fixture is the only place that knows where the server is, so switching to Testcontainers later changes one class.
- Data inside one run is shared between tests: each test creates the data it needs and does not depend on test order.

## Revisit when
- Tests become flaky because of shared state, or the suite gets slow enough to want parallel databases per test class.
- A contributor or an environment can't run `compose.yaml`, or several Postgres versions must be tested → option 3 (Testcontainers).

## Learning resources
- Choosing a testing strategy (EF Core): https://learn.microsoft.com/ef/core/testing/choosing-a-testing-strategy
- Testing against your production database system: https://learn.microsoft.com/ef/core/testing/testing-with-the-database
- Integration tests with `WebApplicationFactory`: https://learn.microsoft.com/aspnet/core/test/integration-tests
- GitHub Actions service containers for PostgreSQL: https://docs.github.com/en/actions/using-containerized-services/creating-postgresql-service-containers (official GitHub docs, not checked via MCP)
- Testcontainers for .NET (the option to revisit): https://dotnet.testcontainers.org/modules/postgres/ (official project site, not checked via MCP)
