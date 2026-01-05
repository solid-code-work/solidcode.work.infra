using MassTransit;
using solidcode.work.infra.Abstraction;

namespace solidcode.work.infra.MassTransit;

public sealed class MassTransitHelpercs : IMessageProducer
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitHelpercs(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<TMessage>(TMessage message)
        where TMessage : class
    {
        return _publishEndpoint.Publish(message);
    }
}
