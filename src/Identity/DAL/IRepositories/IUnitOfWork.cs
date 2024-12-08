namespace DAL.IRepositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    
    IProviderRepository ProviderRepository { get; }
    
    IRefreshTokenRepository RefreshTokenRepository { get; }
    
    Task CompleteAsync();
}