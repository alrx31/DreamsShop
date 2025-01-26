using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ProducerRepository(ApplicationDbContext context) : ICRUDRepository<Producer>
{
    public async Task<List<Producer>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.Producer
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await context.Producer.CountAsync(cancellationToken);
    }

    public async Task<List<Producer>> GetRangeAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await context.Producer.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Producer entity, CancellationToken cancellationToken = default)
    {
        await context.Producer.AddAsync(entity, cancellationToken);
    }

    public async Task<Producer?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Producer.FindAsync([id], cancellationToken);
    }

    public Task UpdateAsync(Producer entity, CancellationToken cancellationToken = default)
    {
        context.Producer.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Producer entity, CancellationToken cancellationToken = default)
    {
        context.Producer.Remove(entity);
        
        return Task.CompletedTask;
    }
}