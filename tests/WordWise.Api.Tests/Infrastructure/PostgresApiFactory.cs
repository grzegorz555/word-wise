using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using WordWise.Api.Infrastructure.Data;

namespace WordWise.Api.Tests.Infrastructure;

// The whole API in memory (WebApplicationFactory), pointed at a fresh Postgres database that exists
// only for this test run (ADR-0013). Shared through PostgresCollectionDefinition, so it is created once per run.
//
// Lifecycle (xUnit v3 IAsyncLifetime):
//   InitializeAsync: MigrateAsync creates the database wordwise_test_<8 hex chars> and applies every migration.
//   DisposeAsync:    EnsureDeletedAsync drops it again.
// Server, port and credentials come from the app's own configuration (appsettings.Development.json locally,
// the ConnectionStrings__WordWise variable in CI). Only the database name is replaced, so no secrets live here.
//
// Docs:
//   EF Core, testing against your production database system: https://learn.microsoft.com/ef/core/testing/testing-with-the-database
//   Customize WebApplicationFactory: https://learn.microsoft.com/aspnet/core/test/integration-tests#customize-webapplicationfactory
//   xUnit v3 shared context (xunit.net, not checked via MCP): https://xunit.net/docs/shared-context
public sealed class PostgresApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Unique per run: test runs from several worktrees never share a database,
    // and the development database "wordwise" is never touched.
    private readonly string _databaseName = "wordwise_test_" + Guid.NewGuid().ToString("N")[..8];

    // xUnit calls DisposeAsync even when InitializeAsync threw, so remember whether there may be a database to drop.
    private bool _databaseMayExist;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Configuration override: the source added last wins, so this in-memory value beats appsettings*.json
        // and environment variables. It is visible only after the app is built, which is enough here because
        // Program.cs reads the connection string lazily, inside the AddDbContext lambda.
        // Docs: https://learn.microsoft.com/aspnet/core/test/integration-tests (section "Customize the WebApplicationFactory with test configurations")
        //       https://learn.microsoft.com/dotnet/core/extensions/configuration-providers#memory-configuration-provider
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var appConnectionString = context.Configuration.GetConnectionString("WordWise")
                ?? throw new InvalidOperationException("Connection string 'WordWise' not found.");

            // Parse instead of editing text: only Database changes; host, port, user and password stay as they are.
            // Docs (npgsql.org, not checked via MCP): https://www.npgsql.org/doc/connection-string-parameters.html
            var testConnectionString = new NpgsqlConnectionStringBuilder(appConnectionString)
            {
                Database = _databaseName,
            }.ConnectionString;

            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:WordWise"] = testConnectionString,
            });
        });
    }

    public async ValueTask InitializeAsync()
    {
        // Services starts the in-memory app. A scope gives a scoped WordWiseDbContext, like one HTTP request does.
        await using var scope = Services.CreateAsyncScope();
        var db = GetTestDbContext(scope.ServiceProvider);

        _databaseMayExist = true;
        try
        {
            // Creates the database if it doesn't exist, then applies all migrations from Migrations/.
            // Docs: https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.relationaldatabasefacadeextensions.migrateasync
            await db.Database.MigrateAsync(TestContext.Current.CancellationToken);
        }
        catch (NpgsqlException ex) when (ex is not PostgresException)
        {
            // A plain NpgsqlException means no connection at all (a PostgresException means the server answered).
            _databaseMayExist = false;
            throw new InvalidOperationException("Postgres is not running: start it with `docker compose up -d` (ADR-0013).", ex);
        }
    }

    public override async ValueTask DisposeAsync()
    {
        if (_databaseMayExist)
        {
            await using var scope = Services.CreateAsyncScope();
            await GetTestDbContext(scope.ServiceProvider).Database.EnsureDeletedAsync();
        }

        await base.DisposeAsync();
    }

    // Safety net: if the configuration override ever stops working, fail instead of migrating or dropping
    // the development database.
    private WordWiseDbContext GetTestDbContext(IServiceProvider services)
    {
        var db = services.GetRequiredService<WordWiseDbContext>();
        var actualDatabase = db.Database.GetDbConnection().Database;

        return string.Equals(actualDatabase, _databaseName, StringComparison.Ordinal)
            ? db
            : throw new InvalidOperationException(
                $"The app uses database '{actualDatabase}' instead of the test database '{_databaseName}'.");
    }
}
