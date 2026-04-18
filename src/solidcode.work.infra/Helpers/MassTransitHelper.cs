using MassTransit;
using solidcode.work.infra.Abstraction;

namespace solidcode.work.infra;

public sealed class MassTransitHelpercs : IMessageProducer
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitHelpercs(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<TMessage>(TMessage message, CancellationToken ct = default)
        where TMessage : class
    {

        Console.WriteLine($"📤 Publishing message of type {typeof(TMessage).Name}");
        return _publishEndpoint.Publish(message, ct);
    }
}
