using Domain.IRepositories;
using Domain.IServices;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.DI;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IConsumerUserRepository, ConsumerUserRepository>();
        services.AddScoped<IProducerUserRepository, ProducerUserRepository>();
        
        services.AddScoped<IPasswordManager, PasswordManager>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IHttpContextService, HttpContextService>();
        
        return services;
    }

    public static void ApplyDatabaseMigration(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();

        const int maxRetries = 10;
        int retryCount = 0;

        while (true)
        {
            try
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
                break;
            }
            catch (Exception ex)
            {
                retryCount++;
                if (retryCount >= maxRetries)
                {
                    throw new Exception("Exceeded max retry attempts to connect to DB", ex);
                }

                Console.WriteLine($"Retrying DB connection ({retryCount}/{maxRetries})...");
                Thread.Sleep(2000);
            }
        }
    }
}