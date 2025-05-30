namespace Domain.IRepositories;

public interface IUnitOfWork
{
    public IConsumerUserRepository ConsumerUserRepository { get; }
    public IProducerUserRepository ProducerUserRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}