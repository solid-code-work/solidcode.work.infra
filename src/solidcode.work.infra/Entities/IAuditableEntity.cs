
namespace solidcode.work.infra.Entities;

public interface IAuditableEntity : IEntity
{
    public DateTime CreatedAt { get; }
    public string? CreatedBy { get; }

}