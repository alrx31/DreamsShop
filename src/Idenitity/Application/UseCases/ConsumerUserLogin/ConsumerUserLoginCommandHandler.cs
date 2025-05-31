using System.Text.Json;
using Application.DTO;
using Domain.IRepositories;
using Domain.IServices;
using Domain.Model;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.ConsumerUserLogin;   

public class ConsumerUserLoginCommandHandler
    (
        IUnitOfWork unitOfWork,
        IPasswordManager passwordManager,
        IJwtService jwtService,
        IConfiguration configuration,
        ICookieService cookieService
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

        var refreshToken = cookieService.GetCookie(user.Id.ToString());

        if (refreshToken is null)
        {
            throw new UnauthorizedAccessException("Refresh token Not Found.");
        }

        if (refreshToken.Value is null)
        {
            var value = JsonSerializer.Serialize(new RefreshTokerCookieModel
            {
                Token = jwtService.GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("Jwt:RefreshTokenExpiresInDays"))
            });
        
            cookieService.SetCookie(new CookieModel
            {
                Key = user.Id.ToString(),
                Value = value
            });
        }
        else
        {
            var tokenModel = JsonSerializer.Deserialize<RefreshTokerCookieModel>(refreshToken.Value) ?? new RefreshTokerCookieModel();
            tokenModel.Token = jwtService.GenerateRefreshToken();
            tokenModel.Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("Jwt:RefreshTokenExpireDays"));
            refreshToken.Value = JsonSerializer.Serialize(tokenModel);
            cookieService.UpdateCookie(refreshToken);
        }
        
        return new ConsumerUserAuthResponseDto
        {
            AccessToken = token,
            UserData = user
        };
    }
}