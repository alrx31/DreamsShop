using System.Security.Claims;
using DAL.Entities;

namespace DAL.IService;

public interface IJwtProvider
{
    string GenerateJwtToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetTokenPrincipal(string jwtToken);
}