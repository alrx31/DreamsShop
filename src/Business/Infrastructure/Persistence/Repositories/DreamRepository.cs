using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class DreamRepository(ApplicationDbContext context) : IDreamRepository
{
    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await context.Dream.CountAsync(cancellationToken);
    }

    public async Task<List<Dream>> GetRangeAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await context.Dream
            .Skip(skip)
            .Take(take)
            //.Include(x=>x.Categories)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Dream entity, CancellationToken cancellationToken = default)
    {
        await context.Dream.AddAsync(entity, cancellationToken);
    }

    public async Task<Dream?> GetAsync(Guid[] ids,CancellationToken cancellationToken = default)
    {
        return await context.Dream.FindAsync([ids[0]], cancellationToken);
    }

    public Task UpdateAsync(Dream entity, CancellationToken cancellationToken = default)
    {
        context.Dream.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Dream entity, CancellationToken cancellationToken = default)
    {
        context.Dream.Remove(entity);
        
        return Task.CompletedTask;
    }
}