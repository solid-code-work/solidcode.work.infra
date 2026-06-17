using Solidcode.Work.Infra.Entities;

namespace Solidcode.Work.Infra.Abstractions;

public interface IIntegrationOutboxService<TOutbox>
    where TOutbox : IIntegrationOutbox
{
    Task<TResponse<List<TOutbox>>> GetUnpublishedAsync();
    Task<TResponse> MarkPublishedAsync(Guid eventId);
}
