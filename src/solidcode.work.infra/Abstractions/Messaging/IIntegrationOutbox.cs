namespace Solidcode.Work.Infra.Abstractions;

public interface IIntegrationOutbox
{
    Guid Id { get; }
    string EventType { get; }
    string Payload { get; }
    bool Published { get; }

    void MarkPublished();
}
