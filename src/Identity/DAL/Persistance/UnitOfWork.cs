using DAL.IRepositories;

namespace DAL.Persistance;

public class UnitOfWork
(ApplicationDbContext context, IUserRepository repository) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    public IUserRepository UserRepository { get; } = repository;

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
    
}