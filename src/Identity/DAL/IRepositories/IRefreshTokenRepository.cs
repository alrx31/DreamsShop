using DAL.Entities;

namespace DAL.IRepositories;

public interface IRefreshTokenRepository
{
    Task<RefreshTokenModel?> GetRefreshToken(Guid id);
    
    Task<RefreshTokenModel?> GetRefreshToken(string token);
    
    Task AddRefreshToken(RefreshTokenModel refreshToken);
    
    Task DeleteRefreshToken(RefreshTokenModel refreshToken);

    Task UpdateRefreshToken(RefreshTokenModel refreshToken);
}