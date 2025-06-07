using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task AddAsync(Category entity, CancellationToken cancellationToken = default)
    {
        await context.Category.AddAsync(entity, cancellationToken);
    }

    public async Task<Category?> GetAsync(Guid[] ids,CancellationToken cancellationToken = default)
    {
        return await context.Category.FindAsync([ids[0]], cancellationToken);
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

    public Task<IQueryable<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IQueryable<Category>>(context.Category);
    }
}