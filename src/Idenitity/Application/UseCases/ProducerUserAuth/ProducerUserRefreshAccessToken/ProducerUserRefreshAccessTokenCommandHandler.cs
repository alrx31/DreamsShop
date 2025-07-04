using System.Text.Json;
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
        var userId = httpContextService.GetCurrentUserId();
        if (!userId.HasValue || userId == Guid.Empty) throw new UnauthorizedAccessException("Invalid token.");

        var user = await unitOfWork.ProducerUserRepository.GetAsync(userId.Value, cancellationToken);
        if(user is null) throw new UnauthorizedAccessException("User not found.");
        
        var cookie = cookieService.GetCookie(userId.Value.ToString());
        if(string.IsNullOrWhiteSpace(cookie)) throw new UnauthorizedAccessException("Invalid token.");
        
        var refreshToken = JsonSerializer.Deserialize<RefreshTokenCookieModel>(cookie)!;
        if(refreshToken.Expires < DateTime.UtcNow) throw new UnauthorizedAccessException("Token has expired.");
        
        return jwtService.GenerateJwtToken(user);
    }
}