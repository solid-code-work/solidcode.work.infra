using Solidcode.Work.Infra.Abstractions;

namespace Solidcode.Work.Infra.Entities;

public sealed class AuditLog : IEntity
{
    public Guid Id { get; private set; }

    public string EntityName { get; private set; } = string.Empty;
    public string EntityId { get; private set; } = string.Empty;

    public string Action { get; private set; } = string.Empty;

    public string UserId { get; private set; } = string.Empty;
    public string UserName { get; private set; } = string.Empty;

    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }

    public DateTime Timestamp { get; private set; }

    private AuditLog() { }

    public static AuditLog Create(
        string entityName,
        string entityId,
        string action,
        string userId,
        string userName,
        string? oldValues,
        string? newValues)
    {
        return new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityName = entityName,
            EntityId = entityId,
            Action = action,
            UserId = userId,
            UserName = userName,
            OldValues = oldValues,
            NewValues = newValues,
            Timestamp = DateTime.UtcNow
        };
    }
}