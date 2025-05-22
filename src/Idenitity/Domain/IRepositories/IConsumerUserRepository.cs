using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface IConsumerUserRepository : ICrudRepository<ConsumerUser>
{
    Task<ConsumerUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}