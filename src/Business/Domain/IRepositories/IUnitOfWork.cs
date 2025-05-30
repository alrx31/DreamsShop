using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface IUnitOfWork
{
    public IDreamRepository DreamRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}