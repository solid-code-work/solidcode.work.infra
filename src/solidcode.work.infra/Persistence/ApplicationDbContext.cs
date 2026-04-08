using Microsoft.EntityFrameworkCore;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Persistence;

public class ApplicationDbContext : IApplicationDbContext
{
    private readonly DbContext _context;

    public ApplicationDbContext(DbContext context)
    {
        _context = context;
    }

    public async Task<TResponse<int>> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var affected = await _context.SaveChangesAsync(cancellationToken);
            return TResponseFactory.Ok(affected, "Changes saved successfully.");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                Console.WriteLine($"Concurrency conflict on {entry.Entity.GetType().Name}");
            }

            return TResponseFactory.Error<int>(ex.Message);
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error<int>(ex.Message);
        }
    }
}