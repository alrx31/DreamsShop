using FluentAssertions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace Tests.UnitTests.Services;

public class CookieServiceTests
{
    private readonly DefaultHttpContext _httpContext = new();

    [Fact]
    public void SetCookie_ShouldWriteCookieToResponse()
    {
        var accessor = new HttpContextAccessor { HttpContext = _httpContext };
        var service = new CookieService(accessor);

        service.SetCookie("refresh", "token");

        _httpContext.Response.Headers["Set-Cookie"].ToString().Should().Contain("refresh=token");
    }

    [Fact]
    public void GetCookie_ShouldReturnExistingCookie()
    {
        _httpContext.Request.Headers["Cookie"] = "refresh=value";
        var accessor = new HttpContextAccessor { HttpContext = _httpContext };
        var service = new CookieService(accessor);

        var value = service.GetCookie("refresh");

        value.Should().Be("value");
    }

    [Fact]
    public void DeleteCookie_ShouldWriteDeleteHeader()
    {
        var accessor = new HttpContextAccessor { HttpContext = _httpContext };
        var service = new CookieService(accessor);

        service.DeleteCookie("refresh");

        _httpContext.Response.Headers["Set-Cookie"].ToString().Should().Contain("refresh=");
    }
}
