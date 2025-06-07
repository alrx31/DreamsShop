using System.Text.Json;
using Domain.IRepositories;
using Domain.IServices;
using Domain.Model;
using MediatR;

namespace Application.UseCases.ConsumerUserAuth.ConsumerUserRefreshAccessToken;

public class ConsumerUserRefreshAccessTokenCommandHandler (
    IUnitOfWork unitOfWork,
    ICookieService cookieService,
    IHttpContextService httpContextService,
    IJwtService jwtService
    ): IRequestHandler<ConsumerUserRefreshAccessTokenCommand, string>
{
    public async Task<string> Handle(ConsumerUserRefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId() ?? throw new UnauthorizedAccessException("Invalid token.");
        if (string.IsNullOrWhiteSpace(userId.ToString())) throw new UnauthorizedAccessException("Invalid token.");

        var user = await unitOfWork.ConsumerUserRepository.GetAsync(userId, cancellationToken);
        if(user is null) throw new UnauthorizedAccessException("User not found.");
        
        var cookie = cookieService.GetCookie(userId.ToString()!);
        if(string.IsNullOrWhiteSpace(cookie)) throw new UnauthorizedAccessException("Invalid token.");
        
        var refreshToken = JsonSerializer.Deserialize<RefreshTokenCookieModel>(cookie)!;
        if(refreshToken.Expires < DateTime.UtcNow) throw new UnauthorizedAccessException("Token has expired.");
        
        return jwtService.GenerateJwtToken(user);
    }
}