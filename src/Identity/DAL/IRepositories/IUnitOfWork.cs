namespace DAL.IRepositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    
    IProducerRepository ProducerRepository { get; }
    
    IRefreshTokenRepository RefreshTokenRepository { get; }
    
    Task CompleteAsync();
}