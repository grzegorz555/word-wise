using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WordWise.Api.Domain;

namespace WordWise.Api.Infrastructure.Data.Configurations;

internal sealed class WordConfiguration : IEntityTypeConfiguration<Word>
{
    public void Configure(EntityTypeBuilder<Word> builder)
    {
        builder.ToTable("Words");

        // TODO(human): 1) Language and Text: required, max length.
        // 2) The relationship Word → Concept (one Concept has many Words), required.
        // 3) Unique composite index (ConceptId, Language, Text) from ADR-0005.
        // hint: HasOne(...).WithMany(...).HasForeignKey(...); HasIndex(w => new { ... }).IsUnique()
        // Concept "Composite index": https://learn.microsoft.com/ef/core/modeling/indexes#composite-index
        // Think: which query from spec 001 (R2: Language = T, sorted by rank) would benefit from a second, non-unique index?
    }
}
