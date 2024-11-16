using DAL.IRepositories;
using DAL.Persistance;
using DAL.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.DI;

public static class DALDependencies
{
    public static IServiceCollection AddDALDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            op=>
            {
                op.UseNpgsql(configuration.GetConnectionString("IdentityConnection"), 
                    b => b.MigrationsAssembly("API"));
            });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProducerRepository, ProducerRepository>();
        
        return services;
    }
}