using System.Linq.Expressions;

namespace Domain.IRepositories.Base;

public interface ISpetificationRepository<T>
{
    Task<List<K>> GetAsync<K>(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, K>>? selector = null,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default
        );
}
