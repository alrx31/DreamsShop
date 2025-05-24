namespace Domain.IRepositories;

public interface IUnitOfWork
{
    public IConsumerUserRepository ConsumerUserRepository { get; }
    public IProducerUserRepository ProducerUserRepository { get; }
    public IRefreshTokerRepository RefreshTokerRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default);
}