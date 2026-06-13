using SolidCode.Work.Infra.Abstractions;

namespace SolidCode.Work.Infra.Entities;

public abstract class AuditableEntity : IAuditable
{
    public DateTime CreatedAt { get; private set; }
    public string CreatedBy { get; private set; } = null!;

    public DateTime? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }

    // Optional: controlled internal setters for infrastructure layer
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