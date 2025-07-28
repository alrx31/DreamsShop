using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface IProducerUserRepository : ICrudRepository<ProducerUser>
{
    Task<ProducerUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}