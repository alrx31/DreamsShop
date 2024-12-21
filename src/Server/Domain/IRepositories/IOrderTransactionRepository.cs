using Domain.Entity;

namespace Domain.IRepositories;

public interface IOrderTransactionRepository
{
    Task AddAsync(OrderTransaction? orderTransaction, CancellationToken cancellationToken = default);
    
    Task<OrderTransaction?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<List<OrderTransaction>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task UpdateAsync(OrderTransaction orderTransaction, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(OrderTransaction orderTransaction, CancellationToken cancellationToken = default);
}