using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Category?>> GetAllAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _context.Category.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Category.CountAsync(cancellationToken);
    }

    public async Task<List<Category>> GetRangeAsync(int skip, int take,
        CancellationToken cancellationToken = default)
    {
        return await _context.Category.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Category entity, CancellationToken cancellationToken = default)
    {
        await _context.Category.AddAsync(entity, cancellationToken);
    }

    public async Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Category.FindAsync(id, cancellationToken);
    }

    public Task UpdateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        _context.Category.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Category entity, CancellationToken cancellationToken = default)
    {
        _context.Category.Remove(entity);
        
        return Task.CompletedTask;
    }
}