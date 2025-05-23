using Application.DTO;
using Application.UseCases.Commands;
using AutoMapper;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using MediatR;

namespace Application.UseCases.CommandsHandlers;

public class ConsumerUserLoginCommandHandler
    (
        IUnitOfWork unitOfWork,
        IPasswordManager passwordManager,
        IJwtService jwtService
    ): IRequestHandler<ConsumerUserLoginCommand, ConsumerUserAuthResponseDto>
{
    
    
    public async Task<ConsumerUserAuthResponseDto> Handle(ConsumerUserLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.ConsumerUserRepository
            .GetByEmailAsync(request.ConsumerUserLoginDto.Email, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("User Not Found.");
        }

        if (!passwordManager.Verify(user.Password, request.ConsumerUserLoginDto.Password))
        {
            throw new UnauthorizedAccessException("Wrong password.");
        }

        var token = jwtService.GenerateJwtToken(user);

        return new ConsumerUserAuthResponseDto
        {
            AccessToken = token,
            UserData = user
        };
    }
}