using Domain.Entity;
using Domain.IRepositories;

namespace Infrastructure.Persistence.Repositories;

public class UserDreamRepository(ApplicationDbContext context) : IUserDreamRepository
{
    public async Task AddAsync(UserDream entity, CancellationToken cancellationToken = default)
    {
        await context.UserDream.AddAsync(entity, cancellationToken);
    }

    public async Task<UserDream?> GetAsync(Guid[] ids,CancellationToken cancellationToken = default)
    {
        return await context.UserDream.FindAsync([ids], cancellationToken);
    }

    public Task UpdateAsync(UserDream entity, CancellationToken cancellationToken = default)
    {
        context.UserDream.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(UserDream entity, CancellationToken cancellationToken = default)
    {
        context.UserDream.Remove(entity);
        return Task.CompletedTask;
    }
}