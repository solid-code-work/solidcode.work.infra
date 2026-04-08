namespace solidcode.work.infra.Abstraction;

public interface IIntegrationOutbox
{
    Guid Id { get; }
    string EventType { get; }
    string Payload { get; }
    bool Published { get; }

    void MarkPublished();
}
