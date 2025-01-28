using Domain.IRepositories;

namespace Infrastructure.Persistence.Repositories;

public class UnitOfWork(ApplicationDbContext context): IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}