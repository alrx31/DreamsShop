namespace Domain.IRepositories.Base;

public interface ICrudRepository<T>  where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<T?> GetAsync(Guid[] ids, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}