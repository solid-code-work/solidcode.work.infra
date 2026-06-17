namespace Solidcode.Work.Infra.Abstractions;

public interface IMessageProducer
{
    Task PublishAsync<TMessage>(TMessage message, CancellationToken ct = default)
        where TMessage : class;
}
