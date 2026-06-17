namespace Solidcode.Work.Infra.Abstractions;

public interface IDomainEventHolder
{
    List<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}