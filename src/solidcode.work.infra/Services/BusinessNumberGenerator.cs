using Microsoft.EntityFrameworkCore;
using Solidcode.Work.Infra.Abstractions;
using Solidcode.Work.Infra.Entities;

namespace Solidcode.Work.Infra.Services;

public sealed class BusinessNumberGenerator : IBusinessNumberGenerator
{
    private readonly DbContext _context;

    public BusinessNumberGenerator(DbContext context)
    {
        _context = context;
    }

    public async Task<TResponse<string>> GenerateAsync(
        string documentType,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentType))
            return TResponseFactory.BadRequest<string>(
                "DocumentType cannot be empty.");

        var sequence = await _context
            .Set<NumberSequence>()
            .SingleOrDefaultAsync(
                x => x.DocumentType == documentType,
                cancellationToken);

        if (sequence is null)
        {
            sequence = new NumberSequence(documentType);
            _context.Set<NumberSequence>().Add(sequence);
        }

        sequence.Increment();

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result <= 0)
            return TResponseFactory.Error<string>(
                "Failed to generate business number.");

        var number = $"{documentType}-{DateTime.UtcNow:yyyy}-{sequence.CurrentValue:D6}";

        return TResponseFactory.Ok<string>(number);
    }
}