using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface IUnitOfWork
{
    public IDreamRepository DreamRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IDreamCategoryRepository DreamCategoryRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}