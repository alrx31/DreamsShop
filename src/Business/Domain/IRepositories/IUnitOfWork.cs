namespace Domain.IRepositories;

public interface IUnitOfWork
{
    public IDreamRepository DreamRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IDreamCategoryRepository DreamCategoryRepository { get; }
    public IUserDreamRepository UserDreamRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}