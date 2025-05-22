using System.Security.Claims;
using System.Security.Cryptography;
using Domain.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class JwtService(
    IConfiguration config)
    : IJwtService
{
    private readonly string _key = config["Jwt:Key"]!;


    public string GenerateJwtToken(string email, int userId)
    {
        throw new NotImplementedException();
    }

    
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        
        using (var numberGenerator = RandomNumberGenerator.Create())
        {
            numberGenerator.GetBytes(randomNumber);
        }
        
        return Convert.ToBase64String(randomNumber);
    }
    
    public ClaimsPrincipal? GetTokenPrincipal(string jwtToken)
    {
        throw new NotImplementedException();
    }
}