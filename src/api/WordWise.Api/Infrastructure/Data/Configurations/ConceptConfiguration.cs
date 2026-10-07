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

        builder.Property(c => c.Key).IsRequired().HasMaxLength(255);
        builder.HasIndex(c => c.Key).IsUnique();
        builder.Property(c => c.PartOfSpeech).IsRequired().HasMaxLength(30);
    }
}
