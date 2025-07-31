using Domain.IService;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class HttpContextService(
    IHttpContextAccessor contextAccessor
    ) : IHttpContextService
{
    public Guid? GetCurrentUserId()
    {
        var value = contextAccessor.HttpContext?.User.Claims.ToList().FirstOrDefault(x => x.Type == "uid")?.Value;
        return value is null ? null : Guid.Parse(value);
    }
}