namespace Solidcode.Work.Infra.Abstractions;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}