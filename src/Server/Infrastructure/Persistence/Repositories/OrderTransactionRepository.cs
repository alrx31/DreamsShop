using Domain.Entity;
using Domain.IRepositories;

namespace Infrastructure.Persistence.Repositories;

public class OrderTransactionRepository(ApplicationDbContext context):IOrderTransactionRepository
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task AddAsync(OrderTransaction? orderTransaction, CancellationToken cancellationToken = default)
    {
        await _context.OrderTransaction.AddAsync(orderTransaction, cancellationToken);
    }

    public async Task<OrderTransaction?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.OrderTransaction.FindAsync(id, cancellationToken);
    }

    public Task<List<OrderTransaction?>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _context.OrderTransaction.ToListAsync(cancellationToken);
    }

    public Task UpdateAsync(OrderTransaction orderTransaction, CancellationToken cancellationToken = default)
    {
        _context.OrderTransaction.Update(orderTransaction);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(OrderTransaction orderTransaction, CancellationToken cancellationToken = default)
    {
        _context.OrderTransaction.Remove(orderTransaction);
        
        return Task.CompletedTask;
    }

    public async Task<List<OrderTransaction>> GetAllAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderTransaction.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.OrderTransaction.CountAsync(cancellationToken);
    }

    public async Task<List<OrderTransaction>> GetRangeAsync(int skip, int take,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderTransaction.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }
}