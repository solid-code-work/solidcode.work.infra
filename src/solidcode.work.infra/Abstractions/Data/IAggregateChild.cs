namespace Solidcode.Work.Infra.Abstractions;

public interface IAggregateChild
{
    Guid AggregateRootId { get; }
}