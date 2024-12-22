using Domain.Entity;

namespace Domain.IRepositories;

public interface IOrderRepository : ICRUDRepository<Order>
{
    Task<List<Order>> GetOrdersByUserId(Guid userId);
}