using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<IQueryable<Order>> GetOrdersByUser(Guid userId, int skip, int take, CancellationToken cancellationToken); 
}
