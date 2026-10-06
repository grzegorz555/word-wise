using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WordWise.Api.Domain;

namespace WordWise.Api.Infrastructure.Data.Configurations;

// Fluent API mapping for Concept, picked up by ApplyConfigurationsFromAssembly in WordWiseDbContext.
// Docs: https://learn.microsoft.com/ef/core/modeling/#grouping-configuration
internal sealed class ConceptConfiguration : IEntityTypeConfiguration<Concept>
{
    public void Configure(EntityTypeBuilder<Concept> builder)
    {
        builder.ToTable("Concepts");

        // TODO(human): Key and PartOfSpeech are required with a sensible max length; Key is unique (ADR-0005).
        // hint: Property(...).IsRequired().HasMaxLength(n); HasIndex(...).IsUnique()
        // Concept "Unique index": https://learn.microsoft.com/ef/core/modeling/indexes#index-uniqueness
    }
}
