using Domain.Entity;
using Domain.IRepositories;

namespace Infrastructure.Persistence.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public async Task<Guid> AddAsync(Order entity, CancellationToken cancellationToken = default)
    {
        return (await context.AddAsync(entity, cancellationToken)).Entity.OrderId;
    }

    public Task DeleteAsync(Order entity, CancellationToken cancellationToken = default)
    {
        context.Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<Order?> GetAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        return await context.Orders.FindAsync([ids[0]], cancellationToken);
    }

    public Task<IQueryable<Order>> GetOrdersByUser(Guid userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(context.Orders.Where(x => x.UserId == userId));
    }

    public Task UpdateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        context.Orders.Update(entity);

        return Task.CompletedTask;
    }
}
