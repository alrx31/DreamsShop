using Domain.IRepositories;

namespace Infrastructure.Persistence;

public class UnitOfWork(
    ApplicationDbContext context,
    IDreamRepository dreamRepository,
    ICategoryRepository categoryRepository,
    IDreamCategoryRepository dreamCategoryRepository,
    IOrderDreamRepository orderDreamRepository,
    IOrderRepository orderRepository
    ): IUnitOfWork
{
    public IDreamRepository DreamRepository { get; } = dreamRepository;
    public ICategoryRepository CategoryRepository { get; } = categoryRepository;
    public IDreamCategoryRepository DreamCategoryRepository { get; } = dreamCategoryRepository;
    public IOrderDreamRepository OrderDreamRepository { get; } = orderDreamRepository;
    public IOrderRepository OrderRepository { get; } = orderRepository;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}