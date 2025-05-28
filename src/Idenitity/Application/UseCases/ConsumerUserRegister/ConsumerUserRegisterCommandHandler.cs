using Application.Exceptions;
using AutoMapper;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using Domain.Model;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.ConsumerUserRegister;

public class ConsumerUserRegisterCommandHandler(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IPasswordManager passwordManager,
    IJwtService jwtService,
    IConfiguration configuration 
    ) : IRequestHandler<ConsumerUserRegisterCommand>
{
    private const byte AcceptablePasswordLevel = 255;
    public async Task Handle(ConsumerUserRegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.ConsumerUserRepository
            .GetByEmailAsync(request.Model.Email, cancellationToken);
        if (user is not null) throw new AlreadyExistException("This Email Is Already Registered.");
        
        var passwordLevel = passwordManager.CheckPassword(request.Model.Password);
        if (passwordLevel < AcceptablePasswordLevel)
        {
            throw new DataValidationException("Passwords Must Be More Complex");
        }

        await unitOfWork.ExecuteInTransactionAsync(async (token) =>
        {
            await unitOfWork.ConsumerUserRepository
                .AddAsync(
                    mapper.Map<ConsumerUser>(request.Model,
                        opt => opt.Items["PasswordHasher"] = passwordManager),
                    token
                );
        
            await unitOfWork.SaveChangesAsync(token);
            var registeredUser = await unitOfWork.ConsumerUserRepository.GetByEmailAsync(request.Model.Email, token);
            
            await unitOfWork.RefreshTokerRepository.AddAsync(new RefreshTokenModel
            {
                UserId = registeredUser.Id,
                RefreshToken = jwtService.GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("Jwt:RefreshTokenExpiresInMinutes"))
            }, token);
        
            await unitOfWork.SaveChangesAsync(token);
        }, cancellationToken);
    }
}