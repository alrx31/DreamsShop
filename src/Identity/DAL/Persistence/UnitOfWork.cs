using DAL.IRepositories;

namespace DAL.Persistence;

public class UnitOfWork
(
    ApplicationDbContext context,
    IUserRepository userRepository,
    IProducerRepository producerRepository,
    IRefreshTokenRepository refreshTokenRepository
    ) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    public IUserRepository UserRepository { get; } = userRepository;
    public IProducerRepository ProducerRepository { get; } = producerRepository;
    public IRefreshTokenRepository RefreshTokenRepository { get; } = refreshTokenRepository;

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
    
}