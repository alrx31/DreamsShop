using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entity;
using Domain.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class JwtService(
    IConfiguration config)
    : IJwtService
{
    private readonly IConfigurationSection _jwtSettings = config.GetSection("Jwt")!;
    
    public string GenerateJwtToken(IHasClaims user)
    {
         var claims = new[]
           {
               new Claim("uid", user.Id.ToString()),
               new Claim("rol", user.Role.ToString()) 
           };

           var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings["Key"]!));
           var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

           var token = new JwtSecurityToken(
               issuer: _jwtSettings["Issuer"],
               audience: _jwtSettings["Audience"],
               claims: claims,
               expires: DateTime.Now.AddMinutes(double.Parse(_jwtSettings["ExpiresInMinutes"]!)),
               signingCredentials: creds);

           return new JwtSecurityTokenHandler().WriteToken(token);
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
}