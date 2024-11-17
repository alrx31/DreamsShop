using BLL.IService;
using BLL.MappingProfiles;
using BLL.Services;
using BLL.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.DI;

public static class BBLDependencies
{
    public static IServiceCollection AddBBLDependencies(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<RegisterUserDTOValidator>(); // Register all validators in the assembly

        services.AddAutoMapper(typeof(UserMapperProfile));
        
        
        return services;
    }
}