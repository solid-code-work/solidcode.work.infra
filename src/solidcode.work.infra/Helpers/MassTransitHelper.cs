using MassTransit;
using Solidcode.Work.Infra.Abstractions;

namespace Solidcode.Work.Infra;

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

        Console.WriteLine($"Publishing message of type {typeof(TMessage).Name}");
        return _publishEndpoint.Publish(message, ct);
    }
}
