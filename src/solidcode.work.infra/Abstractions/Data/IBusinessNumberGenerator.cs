using Solidcode.Work.Infra.Entities;

namespace Solidcode.Work.Infra.Abstractions;

public interface IBusinessNumberGenerator
{
    Task<TResponse<string>> GenerateAsync(
        string documentType,
        CancellationToken cancellationToken = default);
}