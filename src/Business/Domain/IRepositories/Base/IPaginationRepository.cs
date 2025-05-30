namespace Domain.IRepositories.Base;

public interface IPaginationRepository<T> where T : class
{
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    
    Task<List<T>> GetRangeAsync(int skip, int take, CancellationToken cancellationToken = default);
}