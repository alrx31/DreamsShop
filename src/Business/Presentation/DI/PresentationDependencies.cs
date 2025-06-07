using System.Text;
using Domain.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared;

namespace Presentation.DI;

public static class PresentationDependencies
{
    public static IServiceCollection AddPresentationDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddJwtAuthorization(configuration);
        services.AddAuthorizationPolitics();
        
        return services;
    }

    private static IServiceCollection AddJwtAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection("Jwt");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            });
        services.AddAuthorization();
        
        return services;
    }

    private static IServiceCollection AddAuthorizationPolitics(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(Policies.DreamOperationsPolicy), policy =>
            {
                policy.RequireRole(nameof(Roles.Provider), nameof(Roles.Admin));
            });
        
        return services;
    }
}