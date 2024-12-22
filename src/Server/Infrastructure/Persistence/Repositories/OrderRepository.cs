using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Order>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Order
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Order.CountAsync(cancellationToken);
    }

    public async Task<List<Order>> GetRangeAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.Order
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Order entity, CancellationToken cancellationToken = default)
    {
        await _context.Order.AddAsync(entity, cancellationToken);
    }

    public async Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Order.FindAsync(id, cancellationToken);
    }

    public Task UpdateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        _context.Order.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Order entity, CancellationToken cancellationToken = default)
    {
        _context.Order.Remove(entity);
        
        return Task.CompletedTask;
    }

    public async Task<List<Order>> GetOrdersByUserId(Guid userId)
    {
        return await _context.Order
            .Where(x => x.Consumer_Id == userId)
            .ToListAsync();
    }
}