using Domain.Entity;
using Domain.IRepositories;
using Domain.IRepositories.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence;

public class UnitOfWork(
    ApplicationDbContext context,
    IConsumerUserRepository consumerUserRepository,
    IProducerUserRepository producerUserRepository,
    IRefreshTokerRepository refreshTokerRepository): IUnitOfWork
{
    public IConsumerUserRepository ConsumerUserRepository { get; } = consumerUserRepository;
    public IProducerUserRepository ProducerUserRepository { get; } = producerUserRepository;
    public IRefreshTokerRepository RefreshTokerRepository { get; } = refreshTokerRepository;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
    {
        if (context.Database.CurrentTransaction is not null)
        {
            await action(cancellationToken);
            return;
        }

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await action(cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

}