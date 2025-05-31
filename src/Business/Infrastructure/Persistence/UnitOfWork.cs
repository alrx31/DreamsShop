using Domain.Entity;
using Domain.IRepositories;
using Domain.IRepositories.Base;

namespace Infrastructure.Persistence;

public class UnitOfWork(
    ApplicationDbContext context,
    IDreamRepository dreamRepository,
    ICategoryRepository categoryRepository,
    IDreamCategoryRepository dreamCategoryRepository
    ): IUnitOfWork
{
    public IDreamRepository DreamRepository { get; } = dreamRepository;
    public ICategoryRepository CategoryRepository { get; } = categoryRepository;
    public IDreamCategoryRepository DreamCategoryRepository { get; } = dreamCategoryRepository;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}