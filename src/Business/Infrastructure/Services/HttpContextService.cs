using System.Security.Claims;
using Domain.IService;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class HttpContextService(
    IHttpContextAccessor contextAccessor
    ) : IHttpContextService
{
    public List<Claim>? GetUserClaims()
    {
        return contextAccessor.HttpContext?.User.Claims.ToList();
    }

    public Guid? GetCurrentUserId()
    {
        var value = GetUserClaims()?.FirstOrDefault(x => x.Type == "uid")?.Value;
        return value is null ? null : Guid.Parse(value);
    }
}