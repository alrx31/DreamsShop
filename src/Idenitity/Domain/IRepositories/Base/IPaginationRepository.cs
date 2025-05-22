using Domain.Entity;

namespace Domain.IRepositories;

public interface IPaginationRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    
    Task<List<T>> GetRangeAsync(int skip, int take, CancellationToken cancellationToken = default);
}