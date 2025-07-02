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

    public Task<IQueryable<Dream>> GetRangeAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(context.Dream
            .Include(x=>x.DreamCategories)
            .Skip(skip)
            .Take(take));
    }

    public async Task<Guid> AddAsync(Dream entity, CancellationToken cancellationToken = default)
    {
        return (await context.Dream.AddAsync(entity, cancellationToken))
            .Entity.DreamId;
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

    public Task<IQueryable<Dream>> GetRangeAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(context.Dream
            .Join(context.UserDream,
                dream => dream.DreamId,
                userDream => userDream.DreamId,
                (dream, userDream) => dream));
    }
}