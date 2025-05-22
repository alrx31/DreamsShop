using System.Security.Claims;

namespace Domain.IServices;

public interface IJwtService
{
    string GenerateJwtToken(string email,int userId);
    
    string GenerateRefreshToken();
    
    ClaimsPrincipal? GetTokenPrincipal(string jwtToken);
}