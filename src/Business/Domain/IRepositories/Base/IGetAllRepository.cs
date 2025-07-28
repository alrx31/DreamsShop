namespace Domain.IRepositories.Base;

public interface IGetAllRepository<T> where T : class 
{
    Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken);
}