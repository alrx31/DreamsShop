using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ConsumerUserRepository(ApplicationDbContext context) : IConsumerUserRepository
{
    public async Task<List<ConsumerUser>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.ConsumerUser.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);     
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await context.ConsumerUser.CountAsync(cancellationToken);
    }

    public async Task<List<ConsumerUser>> GetRangeAsync(int skip, int take,
        CancellationToken cancellationToken = default)
    {
        return await context.ConsumerUser.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ConsumerUser entity, CancellationToken cancellationToken = default)
    {
        await context.ConsumerUser.AddAsync(entity, cancellationToken);
    }

    public async Task<ConsumerUser?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ConsumerUser.FindAsync([id], cancellationToken);
    }

    public Task UpdateAsync(ConsumerUser entity, CancellationToken cancellationToken = default)
    {
        context.ConsumerUser.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ConsumerUser entity, CancellationToken cancellationToken = default)
    {
        context.ConsumerUser.Remove(entity);
        
        return Task.CompletedTask;
    }

    public async Task<ConsumerUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.ConsumerUser.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }
}