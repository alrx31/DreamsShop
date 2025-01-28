using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface IUnitOfWork
{
    public IConsumerUserRepository ConsumerUserRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IDreamInCategoryRepository DreamInCategoryRepository { get; }
    public IDreamInOrderRepository DreamInOrderRepository { get; }
    public IOrderRepository OrderRepository { get; }
    public IDreamRepository DreamRepository { get; }
    public IMediaRepository MediaRepository { get; }
    public IProducerRepository ProducerRepository { get; }
    public IProducerUserRepository ProducerUserRepository { get; }
    public IRatingsDreamsRepository RatingsDreamsRepository { get; }
    public IRatingsProducerRepository RatingsProducerRepository { get; }
    public IOrderTransactionRepository OrderTransactionRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}