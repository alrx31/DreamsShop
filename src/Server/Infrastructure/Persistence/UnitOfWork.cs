using Domain.Entity;
using Domain.IRepositories;
using Domain.IRepositories.Base;

namespace Infrastructure.Persistence;

public class UnitOfWork(
    ApplicationDbContext context,
    IConsumerUserRepository consumerUserRepository,
    ICategoryRepository categoryRepository,
    IDreamInCategoryRepository dreamInCategoryRepository,
    IDreamInOrderRepository dreamInOrderRepository,
    IOrderRepository orderRepository,
    IDreamRepository dreamRepository,
    IMediaRepository mediaRepository,
    IProducerRepository producerRepository,
    IProducerUserRepository producerUserRepository,
    IRatingsDreamsRepository ratingsDreamsRepository,
    IRatingsProducerRepository ratingsProducerRepository,
    IOrderTransactionRepository orderTransactionRepository): IUnitOfWork
{
    public IConsumerUserRepository ConsumerUserRepository { get; } = consumerUserRepository;
    public ICategoryRepository CategoryRepository { get; } = categoryRepository;
    public IDreamInCategoryRepository DreamInCategoryRepository { get; } = dreamInCategoryRepository;
    public IDreamInOrderRepository DreamInOrderRepository { get; } = dreamInOrderRepository;
    public IOrderRepository OrderRepository { get; } = orderRepository;
    public IDreamRepository DreamRepository { get; } = dreamRepository;
    public IMediaRepository MediaRepository { get; } = mediaRepository;
    public IProducerRepository ProducerRepository { get; } = producerRepository;
    public IProducerUserRepository ProducerUserRepository { get; } = producerUserRepository;
    public IRatingsDreamsRepository RatingsDreamsRepository { get; } = ratingsDreamsRepository;
    public IRatingsProducerRepository RatingsProducerRepository { get; } = ratingsProducerRepository;
    public IOrderTransactionRepository OrderTransactionRepository { get; } = orderTransactionRepository;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}