namespace WordWise.Api.Tests.Infrastructure;

// Collection fixture: every test class marked [Collection(PostgresCollectionDefinition.Name)] gets the same
// PostgresApiFactory in its constructor, so a test run has exactly one test database (ADR-0013).
// xUnit creates the fixture before the first test of the collection and disposes it after the last one.
// Classes in one collection don't run in parallel with each other; other test classes still do.
//
// Why not an assembly fixture ([assembly: AssemblyFixture(...)])? xUnit v3 creates assembly fixtures when
// the test assembly starts, and a failure there fails every test. Without Docker, even HealthEndpointTests
// would fail. A collection fixture is created only when a test of this collection runs.
// Docs (xunit.net, not checked via MCP): https://xunit.net/docs/shared-context#collection-fixture
// Named "...Definition", not "...Collection": CA1711 keeps the Collection suffix for real collection types.
// Docs: https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1711
[CollectionDefinition(Name)]
public sealed class PostgresCollectionDefinition : ICollectionFixture<PostgresApiFactory>
{
    public const string Name = "Postgres";
}
