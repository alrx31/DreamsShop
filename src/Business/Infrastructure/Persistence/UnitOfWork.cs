using Domain.Entity;
using Domain.IRepositories;
using Domain.IRepositories.Base;

namespace Infrastructure.Persistence;

public class UnitOfWork(
    ApplicationDbContext context,
    IDreamRepository dreamRepository,
    ICategoryRepository categoryRepository,
    IDreamCategoryRepository dreamCategoryRepository,
    IUserDreamRepository userDreamRepository): IUnitOfWork
{
    public IDreamRepository DreamRepository { get; } = dreamRepository;
    public ICategoryRepository CategoryRepository { get; } = categoryRepository;
    public IDreamCategoryRepository DreamCategoryRepository { get; } = dreamCategoryRepository;
    public IUserDreamRepository UserDreamRepository { get; } = userDreamRepository;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}