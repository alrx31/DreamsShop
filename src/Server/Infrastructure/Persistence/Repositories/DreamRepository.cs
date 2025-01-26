using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class DreamRepository(ApplicationDbContext context) : IDreamRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Dream>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Dream.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Dream.CountAsync(cancellationToken);
    }

    public async Task<List<Dream>> GetRangeAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.Dream.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Dream entity, CancellationToken cancellationToken = default)
    {
        await _context.Dream.AddAsync(entity, cancellationToken);
    }

    public async Task<Dream?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Dream.FindAsync([id], cancellationToken);
    }

    public Task UpdateAsync(Dream entity, CancellationToken cancellationToken = default)
    {
        _context.Dream.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Dream entity, CancellationToken cancellationToken = default)
    {
        _context.Dream.Remove(entity);
        
        return Task.CompletedTask;
    }
}