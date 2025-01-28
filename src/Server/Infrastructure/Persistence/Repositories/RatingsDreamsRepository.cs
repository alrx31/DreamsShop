using Domain.Entity;
using Domain.IRepositories;
using Domain.IRepositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RatingsDreamsRepository(ApplicationDbContext context): IRatingsDreamsRepository
{
    public async Task<List<RatingsDreams>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.RatingsDreams.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await context.RatingsDreams.CountAsync(cancellationToken);
    }

    public async Task<List<RatingsDreams>> GetRangeAsync(int skip, int take,
        CancellationToken cancellationToken = default)
    {
        return await context.RatingsDreams.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(RatingsDreams entity, CancellationToken cancellationToken = default)
    {
        await context.RatingsDreams.AddAsync(entity, cancellationToken);
    }

    public async Task<RatingsDreams?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.RatingsDreams.FindAsync([id], cancellationToken);
    }

    public Task UpdateAsync(RatingsDreams entity, CancellationToken cancellationToken = default)
    {
        context.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(RatingsDreams entity, CancellationToken cancellationToken = default)
    {
        context.Remove(entity);
        
        return Task.CompletedTask;
    }
}