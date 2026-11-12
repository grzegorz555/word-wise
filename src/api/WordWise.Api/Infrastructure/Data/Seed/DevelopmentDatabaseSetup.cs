using Microsoft.EntityFrameworkCore;

namespace WordWise.Api.Infrastructure.Data.Seed;

// Development only (spec 001, scope 2): after `docker compose up -d` + `dotnet run` the database has the
// schema and the seed words, without a manual `dotnet ef database update`.
// Called from Program.cs before app.Run(), so no request reaches a half-ready database, and a failure stops
// the start where you can see it. Never in production: there the schema changes as a separate, reviewed
// deployment step, and the app's database user shouldn't have the rights to change the schema.
// Docs: https://learn.microsoft.com/ef/core/managing-schemas/migrations/applying#apply-migrations-at-runtime
public static partial class DevelopmentDatabaseSetup
{
    // Switched on only by the launch profiles (Properties/launchSettings.json, .vscode/launch.json), not in
    // appsettings.Development.json: WebApplicationFactory in tests also runs as Development and loads that file,
    // and tests must never migrate or seed the development database.
    public const string RunAtStartupKey = "Seed:RunAtStartup";

    private const string CsvPathKey = "Seed:CsvPath";

    public static async Task SetUpDevelopmentDatabaseAsync(this WebApplication app)
    {
        // The app's root provider can't hand out a scoped DbContext, so create a scope, like one request does.
        // Docs: https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection#resolve-a-service-at-app-start-up
        await using var scope = app.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var cancellationToken = app.Lifetime.ApplicationStopping;

        await services.GetRequiredService<WordWiseDbContext>().Database.MigrateAsync(cancellationToken);

        // The path in configuration is relative to the content root (the project folder under `dotnet run`).
        var csvPath = Path.GetFullPath(
            app.Configuration[CsvPathKey]
                ?? throw new InvalidOperationException($"Configuration value '{CsvPathKey}' not found."),
            app.Environment.ContentRootPath);

        var lines = await File.ReadAllLinesAsync(csvPath, cancellationToken); // UTF-8 by default (ADR-0007)
        var seeds = WordSeedCsvParser.Parse(lines);
        var added = await services.GetRequiredService<WordSeedImporter>().ImportAsync(seeds, cancellationToken);

        LogSeedImported(app.Logger, added, csvPath);
    }

    // Compile-time logging source generator (CA1848): the generated method checks IsEnabled first and parses the
    // message template once, instead of on every LogInformation call. {Added} and {CsvPath} stay structured fields.
    // Docs: https://learn.microsoft.com/dotnet/core/extensions/logging/source-generation
    //       https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1848
    [LoggerMessage(Level = LogLevel.Information, Message = "Word seed: {Added} concepts added from {CsvPath}.")]
    private static partial void LogSeedImported(ILogger logger, int added, string csvPath);
}
