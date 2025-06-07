using System.Text.Json;
using Application.UseCases.ConsumerUserAuth.ConsumerUserRefreshAccessToken;
using Domain.IRepositories;
using Domain.IServices;
using Domain.Model;
using MediatR;

namespace Application.UseCases.ProducerUserAuth.ProducerUserRefreshAccessToken;

public class ProducerUserRefreshAccessTokenCommandHandler (
    IUnitOfWork unitOfWork,
    ICookieService cookieService,
    IHttpContextService httpContextService,
    IJwtService jwtService
    ): IRequestHandler<ProducerUserRefreshAccessTokenCommand, string>
{
    public async Task<string> Handle(ProducerUserRefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId() ?? throw new UnauthorizedAccessException("Invalid token.");
        if (string.IsNullOrWhiteSpace(userId.ToString())) throw new UnauthorizedAccessException("Invalid token.");

        var user = await unitOfWork.ProducerUserRepository.GetAsync(userId, cancellationToken);
        if(user is null) throw new UnauthorizedAccessException("User not found.");
        
        var cookie = cookieService.GetCookie(userId.ToString()!);
        if(string.IsNullOrWhiteSpace(cookie)) throw new UnauthorizedAccessException("Invalid token.");
        
        var refreshToken = JsonSerializer.Deserialize<RefreshTokenCookieModel>(cookie)!;
        if(refreshToken.Expires < DateTime.UtcNow) throw new UnauthorizedAccessException("Token has expired.");
        
        return jwtService.GenerateJwtToken(user);
    }
}