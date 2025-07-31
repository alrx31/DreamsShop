using Application.MappingProfiles;
using Application.UseCases.ConsumerUserAuth.ConsumerUserRegister;
using Application.UseCases.ProducerUserAuth.ProducerUserRegister;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Application.DI;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // from assembly
        services.AddAutoMapper(typeof(ConsumerUserMapperProfile));
        services.AddAutoMapper(typeof(ProducerUserMapperProfile));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConsumerUserRegisterCommandHandler).Assembly));

        services.AddFluentValidationAutoValidation();
        
        services.AddScoped<IValidator<ConsumerUserRegisterCommand>, ConsumerUserRegisterCommandValidator>();
        services.AddScoped<IValidator<ProducerUserRegisterCommand>, ProducerUserRegisterCommandValidator>();
        
        return services;
    }
}