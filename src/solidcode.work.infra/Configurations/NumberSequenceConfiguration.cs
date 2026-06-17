using Microsoft.EntityFrameworkCore;
using Solidcode.Work.Infra.Entities;

namespace Solidcode.Work.Infra.Configurations;

public static class NumberSequenceConfiguration
{
    public static void ApplyNumberSequence(
        this ModelBuilder builder)
    {
        builder.Entity<NumberSequence>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.DocumentType)
                .HasMaxLength(20)
                .IsRequired();

            entity.HasIndex(x => x.DocumentType)
                .IsUnique();

            entity.Property(x => x.RowVersion)
                .IsRowVersion();
        });
    }
}