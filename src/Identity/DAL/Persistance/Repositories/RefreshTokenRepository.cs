using DAL.Entities;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistance.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context) : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context = context;


    public async Task<RefreshTokenModel?> GetRefreshToken(Guid id)
    {
        return await _context.RefreshTokens.FindAsync(id); 
    }

    public async Task<RefreshTokenModel?> GetRefreshToken(string token)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task AddRefreshToken(RefreshTokenModel refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
    }

    public Task DeleteRefreshToken(RefreshTokenModel refreshToken)
    {
        _context.RefreshTokens.Remove(refreshToken);

        return Task.CompletedTask;
    }

    public Task UpdateRefreshToken(RefreshTokenModel refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);

        return Task.CompletedTask;
    }
}