using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ProducerUserRepository
    (ApplicationDbContext context): ICRUDRepository<ProducerUser>
{
    public async Task<List<ProducerUser>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.ProducerUser.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await context.ProducerUser.CountAsync(cancellationToken);
    }

    public async Task<List<ProducerUser>> GetRangeAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await context.ProducerUser.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ProducerUser entity, CancellationToken cancellationToken = default)
    {
        await context.ProducerUser.AddAsync(entity, cancellationToken);
    }

    public async Task<ProducerUser?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ProducerUser.FindAsync([id], cancellationToken);
    }

    public Task UpdateAsync(ProducerUser entity, CancellationToken cancellationToken = default)
    {
        context.ProducerUser.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ProducerUser entity, CancellationToken cancellationToken = default)
    {
        context.ProducerUser.Remove(entity);
        
        return Task.CompletedTask;
    }
}