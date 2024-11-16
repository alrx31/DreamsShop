using DAL.Entities;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistance.Repositories;

public class UserRepository
    (ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> GetUser(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u=>u.Id == id);
    }

    public async Task<User?> GetUser(string email)
    {   
        return await _context.Users.FirstOrDefaultAsync(u=>u.Email == email);
    }

    public Task DeleteUser(User user)
    {
        _context.Users.Remove(user);
        
        return Task.CompletedTask;
    }

    public Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }
}