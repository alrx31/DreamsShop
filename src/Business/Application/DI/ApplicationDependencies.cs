using Application.MappingProfiles;
using Application.UseCases.Dreams.DreamCreate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configuration;

namespace Application.DI;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {   
        // register in assembly (need only one)
        services.AddAutoMapper(typeof(DreamProfile));
        services.AddAutoMapper(typeof(CategoryProfile));
        services.AddAutoMapper(typeof(DreamCategoryProfile));
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DreamCreateCommandHandler).Assembly));
        
        services.Configure<BaseDreamImageConfiguration>(configuration.GetSection("Dream"));
        
        return services;
    }
}