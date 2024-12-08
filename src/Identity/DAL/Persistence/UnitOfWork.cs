using DAL.IRepositories;

namespace DAL.Persistence;

public class UnitOfWork
(
    ApplicationDbContext context,
    IUserRepository userRepository,
    IProviderRepository providerRepository,
    IRefreshTokenRepository refreshTokenRepository
    ) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    public IUserRepository UserRepository { get; } = userRepository;
    public IProviderRepository ProviderRepository { get; } = providerRepository;
    public IRefreshTokenRepository RefreshTokenRepository { get; } = refreshTokenRepository;

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
    
}