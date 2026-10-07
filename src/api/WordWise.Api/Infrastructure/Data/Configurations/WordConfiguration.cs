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
    }
}
