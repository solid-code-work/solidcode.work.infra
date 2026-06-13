namespace SolidCode.Work.Infra.Abstractions;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    string CreatedBy { get; }

    DateTime? UpdatedAt { get; }
    string? UpdatedBy { get; }
}