using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class DreamInOrderRepository
    (ApplicationDbContext context) : IDreamInOrderRepository
{
    public async Task<List<DreamInOrder>> GetAllAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await context.DreamInOrder
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await context.DreamInOrder.CountAsync(cancellationToken);
    }

    public async Task<List<DreamInOrder>> GetRangeAsync(int skip, int take,
        CancellationToken cancellationToken = default)
    {
        return await context.DreamInOrder
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(DreamInOrder entity, CancellationToken cancellationToken = default)
    {
        await context.DreamInOrder.AddAsync(entity, cancellationToken);
    }

    public async Task<DreamInOrder?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.DreamInOrder.FindAsync([id], cancellationToken);
    }

    public Task UpdateAsync(DreamInOrder entity, CancellationToken cancellationToken = default)
    {
        context.DreamInOrder.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(DreamInOrder entity, CancellationToken cancellationToken = default)
    {
        context.DreamInOrder.Remove(entity);
        
        return Task.CompletedTask;
    }
}