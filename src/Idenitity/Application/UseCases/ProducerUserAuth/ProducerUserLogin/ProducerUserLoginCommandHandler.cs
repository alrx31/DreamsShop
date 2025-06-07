using System.Text.Json;
using Application.DTO.ProducerUser;
using Application.Exceptions;
using Domain.IRepositories;
using Domain.IServices;
using Domain.Model;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.ProducerUserAuth.ProducerUserLogin;

public class ProducerUserLoginCommandHandler(
    IUnitOfWork unitOfWork,
    IJwtService jwtService,
    ICookieService cookieService,
    IConfiguration configuration
    ) : IRequestHandler<ProducerUserLoginCommand, ProducerUserAuthResponseDto>
{
    public async Task<ProducerUserAuthResponseDto> Handle(ProducerUserLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.ProducerUserRepository.GetByEmailAsync(request.Dto.Email, cancellationToken);
        if (user is null) throw new UnauthorizedException("User not found.");
        
        cookieService.SetCookie(
            user.Id.ToString(),
            JsonSerializer.Serialize(new RefreshTokenCookieModel
            {
                Token = jwtService.GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("Jwt:RefreshTokenExpiresInDays"))
            })
            );

        return new ProducerUserAuthResponseDto
        {
            AccessToken = jwtService.GenerateJwtToken(user),
            UserData = user
        };
    }
}