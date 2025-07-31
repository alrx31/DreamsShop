using System.Security.Claims;
using Domain.Entity;

namespace Domain.IServices;

public interface IJwtService
{
    string GenerateJwtToken(IHasClaims claims);
    
    string GenerateRefreshToken();
}