using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Solidcode.Work.Infra.Abstractions;
using Solidcode.Work.Infra.Entities;

namespace Solidcode.Work.Infra.Persistence;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private const string SystemUser = "SYSTEM";

    private readonly ICurrentUser _currentUser;

    public AuditSaveChangesInterceptor(
        ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);

        return base.SavingChanges(
            eventData,
            result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context);

        return base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }

    private void ApplyAudit(
        DbContext? context)
    {
        if (context is null)
            return;

        var userId = string.IsNullOrWhiteSpace(_currentUser.UserId)
            ? SystemUser
            : _currentUser.UserId;

        var userName = string.IsNullOrWhiteSpace(_currentUser.UserName)
            ? SystemUser
            : _currentUser.UserName;

        ApplyAuditableEntityTracking(
            context,
            userName);

        var auditLogs = CreateAuditLogs(
            context,
            userId,
            userName);

        if (auditLogs.Count > 0)
        {
            context.Set<AuditLog>()
                .AddRange(auditLogs);
        }
    }

    private static void ApplyAuditableEntityTracking(
        DbContext context,
        string userName)
    {
        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.MarkCreated(userName);
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.MarkUpdated(userName);
            }
        }
    }

    private static List<AuditLog> CreateAuditLogs(
        DbContext context,
        string userId,
        string userName)
    {
        var logs = new List<AuditLog>();

        var entries = context.ChangeTracker
            .Entries()
            .Where(e =>
                e.Entity is not AuditLog &&
                e.State is EntityState.Added
                    or EntityState.Modified
                    or EntityState.Deleted);

        foreach (var entry in entries)
        {
            var log = AuditLogFactory.Create(
                entry,
                userId,
                userName);

            if (log is not null)
            {
                logs.Add(log);
            }
        }

        return logs;
    }
}