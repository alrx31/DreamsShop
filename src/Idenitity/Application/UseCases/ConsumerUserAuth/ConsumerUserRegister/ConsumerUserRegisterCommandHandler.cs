using System.Text.Json;
using Application.Exceptions;
using Application.UseCases.ProducerUserAuth.ProducerUserRegister;
using AutoMapper;
using Domain.IRepositories;
using Domain.IServices;
using Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.ConsumerUserAuth.ConsumerUserRegister;

public class ConsumerUserRegisterCommandHandler(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IPasswordManager passwordManager,
    IJwtService jwtService,
    IConfiguration configuration,
    ICookieService cookieService,
    IValidator<ConsumerUserRegisterCommand> commandValidator
    ) : IRequestHandler<ConsumerUserRegisterCommand>
{
    private const byte AcceptablePasswordLevel = 255;
    public async Task Handle(ConsumerUserRegisterCommand request, CancellationToken cancellationToken)
    {
        var validateResult = await commandValidator.ValidateAsync(request, cancellationToken);

        if (!validateResult.IsValid)
        {
            throw new DataValidationException(validateResult.ToString());
        }
        
        var user = await unitOfWork.ConsumerUserRepository
            .GetByEmailAsync(request.Model.Email, cancellationToken);
        if (user is not null) throw new AlreadyExistException("This Email Is Already Registered.");
        
        var passwordLevel = passwordManager.CheckPassword(request.Model.Password);
        if (passwordLevel < AcceptablePasswordLevel)
        {
            throw new DataValidationException("Passwords Must Be More Complex");
        }

        await unitOfWork.ConsumerUserRepository
            .AddAsync(
                mapper.Map<Domain.Entity.ConsumerUser>(request.Model,
                    opt => opt.Items["PasswordHasher"] = passwordManager),
                cancellationToken
            );
    
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var registeredUser = await unitOfWork.ConsumerUserRepository.GetByEmailAsync(request.Model.Email, cancellationToken);
        
        var value = JsonSerializer.Serialize(new RefreshTokenCookieModel
        {
            Token = jwtService.GenerateRefreshToken(),
            Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("Jwt:RefreshTokenExpiresInDays"))
        });
        
        cookieService.SetCookie(new CookieModel
        {
            Key = registeredUser.Id.ToString(),
            Value = value
        });
    }
}