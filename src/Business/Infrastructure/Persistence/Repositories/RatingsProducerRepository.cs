using Domain.Entity;
using Domain.IRepositories;
using Domain.IRepositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RatingsProducerRepository(ApplicationDbContext context): IRatingsProducerRepository
{
    public async Task<List<RatingsProducer>> GetAllAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await context.RatingsProducer.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await context.RatingsProducer.CountAsync(cancellationToken);
    }

    public async Task<List<RatingsProducer>> GetRangeAsync(int skip, int take,
        CancellationToken cancellationToken = default)
    {
        return await context.RatingsProducer.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(RatingsProducer entity, CancellationToken cancellationToken = default)
    {
        await context.RatingsProducer.AddAsync(entity, cancellationToken);
    }

    public async Task<RatingsProducer?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.RatingsProducer.FindAsync([id], cancellationToken);
    }

    public Task UpdateAsync(RatingsProducer entity, CancellationToken cancellationToken = default)
    {
        context.RatingsProducer.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(RatingsProducer entity, CancellationToken cancellationToken = default)
    {
        context.RatingsProducer.Remove(entity);
        
        return Task.CompletedTask;
    }
}