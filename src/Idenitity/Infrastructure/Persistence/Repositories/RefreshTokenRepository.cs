using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context) : IRefreshTokerRepository
{
    public async Task AddAsync(RefreshTokenModel entity, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(entity, cancellationToken);
    }

    public async Task<RefreshTokenModel?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.RefreshTokens.FindAsync(keyValues: [id], cancellationToken);
    }

    public Task UpdateAsync(RefreshTokenModel entity, CancellationToken cancellationToken = default)
    {
        context.RefreshTokens.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(RefreshTokenModel entity, CancellationToken cancellationToken = default)
    {
        context.RefreshTokens.Remove(entity);
        return Task.CompletedTask;
    }
}