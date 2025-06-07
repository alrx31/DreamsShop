using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class DreamCategoryRepository(ApplicationDbContext context) : IDreamCategoryRepository
{
    public async Task AddAsync(DreamCategory entity, CancellationToken cancellationToken = default)
    {
        await context.DreamCategory.AddAsync(entity, cancellationToken);
    }

    public async Task<DreamCategory?> GetAsync(Guid[] ids,CancellationToken cancellationToken = default)
    {
        return await context.DreamCategory.FindAsync([..ids], cancellationToken);
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

    public Task<IQueryable<DreamCategory>> GetCategoriesByDreamIdAsync(Guid dreamId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(context.DreamCategory.Where(x => x.DreamId == dreamId).Distinct());
    }
}