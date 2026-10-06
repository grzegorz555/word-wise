using Microsoft.EntityFrameworkCore;

namespace WordWise.Api.Infrastructure.Data;

// The application's unit of work and gateway to PostgreSQL (ADR-0003).
// Registered with AddDbContext in Program.cs, so it has a scoped lifetime: one instance per HTTP request.
// Docs: https://learn.microsoft.com/ef/core/dbcontext-configuration/
public sealed class WordWiseDbContext(DbContextOptions<WordWiseDbContext> options) : DbContext(options)
{
    // DbSet<Concept> and DbSet<Word> arrive in T2, together with their IEntityTypeConfiguration classes.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Picks up every IEntityTypeConfiguration<T> in this assembly (Infrastructure/Data/Configurations).
        // Docs: https://learn.microsoft.com/ef/core/modeling/#grouping-configuration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WordWiseDbContext).Assembly);
    }
}
