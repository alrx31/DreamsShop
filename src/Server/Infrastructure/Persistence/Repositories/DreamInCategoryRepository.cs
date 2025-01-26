using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class DreamInCategoryRepository
    (ApplicationDbContext context) : IDreamInCategoryRepository
{
    public async Task<List<DreamInCategory>> GetAllAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await context.DreamInCategory
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await context.DreamInCategory.CountAsync(cancellationToken);
    }

    public async Task<List<DreamInCategory>> GetRangeAsync(int skip, int take,
        CancellationToken cancellationToken = default)
    {
        return await context.DreamInCategory
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public Task AddAsync(DreamInCategory entity, CancellationToken cancellationToken = default)
    {
        context.DreamInCategory.Add(entity);
        
        return Task.CompletedTask;
    }

    public async Task<DreamInCategory?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.DreamInCategory.FindAsync([id], cancellationToken);
    }

    public Task UpdateAsync(DreamInCategory entity, CancellationToken cancellationToken = default)
    {
        context.DreamInCategory.Update(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(DreamInCategory entity, CancellationToken cancellationToken = default)
    {
        context.DreamInCategory.Remove(entity);

        return Task.CompletedTask;
    }
}