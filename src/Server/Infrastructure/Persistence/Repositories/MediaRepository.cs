using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MediaRepository
    (ApplicationDbContext context) : IMediaRepository
{
    public async Task<List<Media>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.Media
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await context.Media.CountAsync(cancellationToken);
    }

    public async Task<List<Media>> GetRangeAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await context.Media
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Media entity, CancellationToken cancellationToken = default)
    {
        await context.Media.AddAsync(entity, cancellationToken);
    }

    public async Task<Media?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Media.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task UpdateAsync(Media entity, CancellationToken cancellationToken = default)
    {
        context.Media.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Media entity, CancellationToken cancellationToken = default)
    {
        context.Media.Remove(entity);
        
        return Task.CompletedTask;
    }
}