using Domain.Entity;
using Domain.IRepositories;

namespace Infrastructure.Persistence.Repositories;

public class OrderDreamRepository(ApplicationDbContext context) : IOrderDreamRepository
{
    public async Task<Guid> AddAsync(OrderDream entity, CancellationToken cancellationToken = default)
    {
        await context.OrderDreams.AddAsync(entity, cancellationToken);

        return Guid.Empty;
    }

    public Task DeleteAsync(OrderDream entity, CancellationToken cancellationToken = default)
    {
        context.Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<OrderDream?> GetAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        return await context.OrderDreams.FindAsync([ids], cancellationToken);
    }

    public Task UpdateAsync(OrderDream entity, CancellationToken cancellationToken = default)
    {
        context.OrderDreams.Update(entity);

        return Task.CompletedTask;
    }
}
