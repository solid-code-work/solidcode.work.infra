using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Solidcode.Work.Infra.Abstractions;
using Solidcode.Work.Infra.Entities;

namespace Solidcode.Work.Infra.Services;

public sealed class AggregateAuditService
{
    public void TouchAggregateRoots(
        DbContext context,
        string user)
    {
        var changedChildren =
            context.ChangeTracker
                .Entries<IAggregateChild>()
                .Where(x =>
                    x.State == EntityState.Added ||
                    x.State == EntityState.Modified ||
                    x.State == EntityState.Deleted)
                .ToList();

        foreach (var childEntry in changedChildren)
        {
            var attribute =
                childEntry.Entity.GetType()
                    .GetCustomAttribute<AggregateRootAttribute>();

            if (attribute is null)
                continue;

            var rootId = childEntry.Entity.AggregateRootId;

            var rootEntry =
                context.ChangeTracker
                    .Entries()
                    .FirstOrDefault(x =>
                        x.Entity.GetType() == attribute.RootType &&
                        x.State == EntityState.Unchanged &&
                        ((Entity)x.Entity).Id == rootId);

            if (rootEntry?.Entity is AggregateRoot root)
            {
                root.MarkUpdated(user);
                rootEntry.State = EntityState.Modified;
            }
        }
    }
}