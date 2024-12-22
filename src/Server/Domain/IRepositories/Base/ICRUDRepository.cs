namespace Domain.IRepositories;

public interface ICRUDRepository<T> : IPaginationRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    
    Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}