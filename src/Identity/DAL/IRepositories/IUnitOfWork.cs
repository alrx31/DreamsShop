namespace DAL.IRepositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    Task CompleteAsync();
}