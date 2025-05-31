namespace Domain.IRepositories.Base;

public interface IGetAllRepository<T> where T : class 
{
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
}