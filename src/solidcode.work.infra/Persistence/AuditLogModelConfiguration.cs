using Microsoft.EntityFrameworkCore;
using Solidcode.Work.Infra.Entities;

namespace Solidcode.Work.Infra.Persistence;

public static class AuditLogModelConfiguration
{
    public static void ApplyConfigureAuditLog(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EntityName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.EntityId)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Action)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.UserId)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.UserName)
                .HasMaxLength(200);

            entity.Property(x => x.OldValues)
                .HasColumnType("nvarchar(max)");

            entity.Property(x => x.NewValues)
                .HasColumnType("nvarchar(max)");

            entity.Property(x => x.Timestamp)
                .IsRequired();

            entity.HasIndex(x => x.EntityName);
            entity.HasIndex(x => x.EntityId);
            entity.HasIndex(x => x.Timestamp);
        });
    }
}