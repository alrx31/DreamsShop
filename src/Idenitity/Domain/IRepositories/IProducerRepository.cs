using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface IProducerRepository : ICrudRepository<Producer>
{
    Task<Producer?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
}
