namespace solidcode.work.infra.Abstraction;

public interface IMessageProducer
{
    Task PublishAsync<TMessage>(TMessage message, CancellationToken ct = default)
        where TMessage : class;
}
