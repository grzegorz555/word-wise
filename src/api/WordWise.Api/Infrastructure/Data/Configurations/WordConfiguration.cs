using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WordWise.Api.Domain;

namespace WordWise.Api.Infrastructure.Data.Configurations;

internal sealed class WordConfiguration : IEntityTypeConfiguration<Word>
{
    public void Configure(EntityTypeBuilder<Word> builder)
    {
        builder.ToTable("Words");

        builder.Property(w => w.Language).IsRequired().HasMaxLength(2);
        builder.Property(w => w.Text).IsRequired().HasMaxLength(255);
        builder.HasOne(w => w.Concept).WithMany(c => c.Words).HasForeignKey(w => w.ConceptId).IsRequired();
        builder.HasIndex(w => new { w.ConceptId, w.Language, w.Text }).IsUnique();

        // TODO(human) #3: a second index for the card query (spec 001, R2: filter by Language, sort by FrequencyRank).
        //   Decide: column order (which column goes first, and why?) and unique or not (can two EN words share a rank?).
        //   Then add a migration: dotnet ef migrations add <DescriptiveName> --project src/api/WordWise.Api
        //   Predict before you run it: what will Up() and Down() contain?
        // Hint: https://learn.microsoft.com/ef/core/modeling/indexes#composite-index
        //   https://www.postgresql.org/docs/current/indexes-multicolumn.html
    }
}
