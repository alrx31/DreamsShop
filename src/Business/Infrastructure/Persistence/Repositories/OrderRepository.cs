using Domain.Entity;
using Domain.IRepositories;
using Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrderRepository(ApplicationDbContext context) : BaseRepository<Order>(context), IOrderRepository
{
    public Task<IQueryable<Order>> GetOrdersByUser(Guid userId, int skip, int take, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            Context.Orders
            .Where(x => x.UserId == userId)
            .Include(d => d.OrderDreams!)
                .ThenInclude(od => od.Dream)
            .Skip(skip)
            .Take(take)
            .AsQueryable()
            );
    }
}