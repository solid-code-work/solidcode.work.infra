using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Abstraction;

public interface IIntegrationOutboxService<TOutbox>
    where TOutbox : IIntegrationOutbox
{
    Task<TResult<List<TOutbox>>> GetUnpublishedAsync();
    Task<TResult> MarkPublishedAsync(Guid eventId);
}
