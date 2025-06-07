using Domain.Entity;
using Domain.IRepositories;

namespace Infrastructure.Persistence.Repositories;

public class DreamCategoryRepository(ApplicationDbContext context) : IDreamCategoryRepository
{
    public async Task AddAsync(DreamCategory entity, CancellationToken cancellationToken = default)
    {
        await context.DreamCategory.AddAsync(entity, cancellationToken);
    }

    public async Task<DreamCategory?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.DreamCategory.FindAsync([id], cancellationToken);
    }

    public Task UpdateAsync(DreamCategory entity, CancellationToken cancellationToken = default)
    {
        context.DreamCategory.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(DreamCategory entity, CancellationToken cancellationToken = default)
    {
        context.DreamCategory.Remove(entity);
        return Task.CompletedTask;
    }
}