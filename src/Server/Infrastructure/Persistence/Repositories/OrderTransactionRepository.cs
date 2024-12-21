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

    public Task<List<OrderTransaction>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(OrderTransaction orderTransaction, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(OrderTransaction orderTransaction, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}