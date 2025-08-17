using Domain.IRepositories;

namespace Infrastructure.Persistence;

public class UnitOfWork(
    ApplicationDbContext context,
    IConsumerUserRepository consumerUserRepository,
    IProducerUserRepository producerUserRepository,
    IProducerRepository producerRepository
    ): IUnitOfWork
{
    public IConsumerUserRepository ConsumerUserRepository { get; } = consumerUserRepository;
    public IProducerUserRepository ProducerUserRepository { get; } = producerUserRepository;
    public IProducerRepository ProducerRepository { get; } = producerRepository;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}