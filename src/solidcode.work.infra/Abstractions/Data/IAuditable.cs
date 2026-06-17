namespace Solidcode.Work.Infra.Abstractions;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    string CreatedBy { get; }

    DateTime? UpdatedAt { get; }
    string? UpdatedBy { get; }

    void MarkCreated(string user);
    void MarkUpdated(string user);
}