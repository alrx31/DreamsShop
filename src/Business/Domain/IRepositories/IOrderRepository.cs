using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface IOrderRepository : ICrudRepository<Order>
{
    Task<List<Order>> GetOrdersByUserId(Guid userId);
}