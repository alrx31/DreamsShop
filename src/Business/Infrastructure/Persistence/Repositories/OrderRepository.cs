using Domain.Entity;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

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
        return await context.Orders
            .Include(o => o.OrderDreams)
            .FirstOrDefaultAsync(o => o.OrderId == ids[0], cancellationToken);
    }

    public Task<IQueryable<Order>> GetOrdersByUser(Guid userId, int skip, int take, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            context.Orders
            .Where(x => x.UserId == userId)
            .Include(d => d.OrderDreams)
            .Skip(skip)
            .Take(take)
            .AsQueryable()
            );
    }

    public Task UpdateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        context.Orders.Update(entity);

        return Task.CompletedTask;
    }
}
