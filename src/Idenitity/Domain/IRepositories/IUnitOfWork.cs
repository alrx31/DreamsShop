namespace Domain.IRepositories;

public interface IUnitOfWork
{
    public IConsumerUserRepository ConsumerUserRepository { get; }
    public IProducerUserRepository ProducerUserRepository { get; }
    public IProducerRepository ProducerRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}