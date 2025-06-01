using Application.MappingProfiles;
using Application.UseCases.Dreams.DreamCreate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {   
        services.AddAutoMapper(typeof(DreamProfile));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DreamCreateCommandHandler).Assembly));
        
        return services;
    }
}