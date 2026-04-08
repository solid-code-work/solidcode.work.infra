namespace solidcode.work.infra.Abstraction;

public interface IMessageProducer
{
    Task PublishAsync<TMessage>(TMessage message)
        where TMessage : class;
}
