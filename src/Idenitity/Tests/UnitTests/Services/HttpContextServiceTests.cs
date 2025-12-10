using System.Security.Claims;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace Tests.UnitTests.Services;

public class HttpContextServiceTests
{
    [Fact]
    public void GetCurrentUserId_ShouldReturnGuid_WhenClaimExists()
    {
        var userId = Guid.NewGuid();
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("uid", userId.ToString())
            }))
        };

        var service = new HttpContextService(new HttpContextAccessor { HttpContext = httpContext });

        service.GetCurrentUserId().Should().Be(userId);
    }

    [Fact]
    public void GetCurrentUserId_ShouldReturnNull_WhenClaimMissing()
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity())
        };

        var service = new HttpContextService(new HttpContextAccessor { HttpContext = httpContext });

        service.GetCurrentUserId().Should().BeNull();
    }
}
