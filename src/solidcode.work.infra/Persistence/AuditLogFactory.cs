using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Solidcode.Work.Infra.Entities;
using Solidcode.Work.Infra.Enums;

namespace Solidcode.Work.Infra.Persistence;

public static class AuditLogFactory
{
    private static readonly HashSet<string> IgnoredProperties =
    [
        "CreatedAt",
        "CreatedBy",
        "UpdatedAt",
        "UpdatedBy"
    ];

    public static AuditLog? Create(
        EntityEntry entry,
        string userId,
        string userName)
    {
        if (entry.Entity is AuditLog)
            return null;

        var entityName = entry.Entity.GetType().Name;
        var entityId = GetEntityId(entry);

        return entry.State switch
        {
            EntityState.Added => AuditLog.Create(
                entityName,
                entityId,
                AuditAction.Create.ToString(),
                userId,
                userName,
                null,
                SerializeCurrentValues(entry)),

            EntityState.Modified => CreateUpdateAuditLog(
                entry,
                entityName,
                entityId,
                userId,
                userName),

            EntityState.Deleted => AuditLog.Create(
                entityName,
                entityId,
                AuditAction.Delete.ToString(),
                userId,
                userName,
                SerializeOriginalValues(entry),
                null),

            _ => null
        };
    }

    private static AuditLog? CreateUpdateAuditLog(
        EntityEntry entry,
        string entityName,
        string entityId,
        string userId,
        string userName)
    {
        var oldValues = GetOldModifiedValues(entry);
        var newValues = GetNewModifiedValues(entry);

        if (oldValues.Count == 0)
            return null;

        return AuditLog.Create(
            entityName,
            entityId,
            AuditAction.Update.ToString(),
            userId,
            userName,
            JsonSerializer.Serialize(oldValues),
            JsonSerializer.Serialize(newValues));
    }

    private static string GetEntityId(EntityEntry entry)
    {
        var idProperty = entry.Properties
            .FirstOrDefault(x => x.Metadata.Name == "Id");

        return idProperty?.CurrentValue?.ToString()
               ?? idProperty?.OriginalValue?.ToString()
               ?? string.Empty;
    }

    private static string SerializeCurrentValues(EntityEntry entry)
    {
        var values = entry.CurrentValues.Properties
            .Where(p => !IgnoredProperties.Contains(p.Name))
            .ToDictionary(
                p => p.Name,
                p => entry.CurrentValues[p]);

        return JsonSerializer.Serialize(values);
    }

    private static string SerializeOriginalValues(EntityEntry entry)
    {
        var values = entry.OriginalValues.Properties
            .Where(p => !IgnoredProperties.Contains(p.Name))
            .ToDictionary(
                p => p.Name,
                p => entry.OriginalValues[p]);

        return JsonSerializer.Serialize(values);
    }

    private static Dictionary<string, object?> GetOldModifiedValues(EntityEntry entry)
    {
        return entry.Properties
            .Where(p =>
                p.IsModified &&
                !IgnoredProperties.Contains(p.Metadata.Name))
            .ToDictionary(
                p => p.Metadata.Name,
                p => p.OriginalValue);
    }

    private static Dictionary<string, object?> GetNewModifiedValues(EntityEntry entry)
    {
        return entry.Properties
            .Where(p =>
                p.IsModified &&
                !IgnoredProperties.Contains(p.Metadata.Name))
            .ToDictionary(
                p => p.Metadata.Name,
                p => p.CurrentValue);
    }
}