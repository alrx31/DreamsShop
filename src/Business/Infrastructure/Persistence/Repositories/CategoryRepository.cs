using Domain.Entity;
using Domain.IRepositories;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task AddAsync(Category entity, CancellationToken cancellationToken = default)
    {
        await context.Category.AddAsync(entity, cancellationToken);
    }

    public async Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Category.FindAsync([id], cancellationToken);
    }

    public Task UpdateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        context.Category.Update(entity: entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Category entity, CancellationToken cancellationToken = default)
    {
        context.Category.Remove(entity: entity);
        return Task.CompletedTask;
    }
}