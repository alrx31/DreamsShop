using Domain.Entity;
using Domain.IRepositories;
using Domain.IRepositories.Base;

namespace Infrastructure.Persistence;

public class UnitOfWork(
    ApplicationDbContext context,
    IDreamRepository dreamRepository
    ): IUnitOfWork
{
    public IDreamRepository DreamRepository { get; } = dreamRepository;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}