using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using WordWise.Api.Infrastructure.Data;

namespace WordWise.Api.Tests.Infrastructure;

// Smoke test of the test infrastructure itself (ADR-0013): if this fails, every database test fails too.
// xUnit injects the shared factory because the class belongs to PostgresCollectionDefinition.
[Collection(PostgresCollectionDefinition.Name)]
public sealed class PostgresApiFactoryTests(PostgresApiFactory factory)
{
    [Fact]
    public async Task InitializeAsync_NewTestRun_CreatesMigratedTestDatabase()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var scope = factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<WordWiseDbContext>();

        var pendingMigrations = await db.Database.GetPendingMigrationsAsync(cancellationToken);

        db.Database.GetDbConnection().Database.ShouldStartWith("wordwise_test_");
        pendingMigrations.ShouldBeEmpty();
        // The Words table exists. The row count isn't checked: other tests in the same run may add words.
        await Should.NotThrowAsync(() => db.Words.CountAsync(cancellationToken));
    }
}
