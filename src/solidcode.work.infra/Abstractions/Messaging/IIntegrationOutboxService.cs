using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Abstraction;

public interface IIntegrationOutboxService<TOutbox>
    where TOutbox : IIntegrationOutbox
{
    Task<TResponse<List<TOutbox>>> GetUnpublishedAsync();
    Task<TResponse> MarkPublishedAsync(Guid eventId);
}
