using BLL.IService;
using BLL.MappingProfiles;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.DI;

public static class BBLDependencies
{
    public static IServiceCollection AddBBLDependencies(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserService, UserService>();

        services.AddAutoMapper(typeof(UserMapperProfile));
        
        
        return services;
    }
}