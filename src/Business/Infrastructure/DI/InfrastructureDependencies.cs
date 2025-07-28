using Domain.IRepositories;
using Domain.IService;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Configuration;

namespace Infrastructure.DI;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDatabases(configuration)
            .AddServices(configuration)
            .AddRepositories();
    }

    private static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.Configure<MinioConfiguration>(configuration.GetSection("Minio"));

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<IHttpContextService, HttpContextService>()
            .AddScoped<IFileStorageService, FileStorageService>()
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            })
            .AddScoped(typeof(ICacheService<,>), typeof(RedisCacheService<,>));
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IDreamRepository, DreamRepository>()
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<IDreamCategoryRepository, DreamCategoryRepository>()
            .AddScoped<IOrderDreamRepository, OrderDreamRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()

            .AddScoped<IUnitOfWork, UnitOfWork>();
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