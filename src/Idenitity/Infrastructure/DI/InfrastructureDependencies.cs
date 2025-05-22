using Domain.IRepositories;
using Domain.IServices;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IConsumerUserRepository, ConsumerUserRepository>();
        services.AddScoped<IProducerUserRepository, ProducerUserRepository>();
        
        services.AddScoped<IPasswordManager, PasswordManager>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}