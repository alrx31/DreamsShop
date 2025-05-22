using Application.MappingProfiles;
using Application.UseCases.CommandsHandlers;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(ConsumerUserMapperProfile));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConsumerUserRegisterCommandHandler).Assembly));
        
        //services.AddTransient<IValidator<ConsumerUserRegisterDtoValidation>>();
        
        return services;
    }
}