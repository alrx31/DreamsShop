using DAL.Entities;

namespace DAL.IRepositories;

public interface IUserRepository
{
    Task AddUser(User user);
    
    Task<User?> GetUser(Guid id);
    
    Task<User?> GetUser(string email);

    Task DeleteUser(User user);

    Task UpdateUser(User user);
}