using System.Security.Claims;

namespace Domain.IService;

public interface IHttpContextService
{
    List<Claim>? GetUserClaims();
    
    Guid? GetCurrentUserId();
}