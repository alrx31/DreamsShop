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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProviderService, ProviderService>();
        
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<RegisterUserDTOValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserDTOValidator>();

        services.AddAutoMapper(typeof(UserMapperProfile));
        
        
        return services;
    }
}