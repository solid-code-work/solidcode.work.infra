using Solidcode.Work.Infra.Abstractions;

namespace Solidcode.Work.Infra.Entities;

public abstract class AuditableEntity : IAuditable
{
    public DateTime CreatedAt { get; private set; }
    public string CreatedBy { get; private set; } = null!;

    public DateTime? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }

    public void MarkCreated(string user)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = user;
    }

    public void MarkUpdated(string user)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = user;
    }
}