using FluentAssertions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Tests.UnitTests.Services;

public class HttpContextServiceTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly HttpContextService _httpContextService;

    public HttpContextServiceTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _httpContextService = new HttpContextService(_httpContextAccessorMock.Object);
    }

    [Fact]
    public void GetCurrentUserId_ShouldReturnUserId_WhenUserIsAuthenticated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var claims = new List<Claim> { new("uid", userId.ToString()) };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _httpContextService.GetCurrentUserId();

        // Assert
        result.Should().Be(userId);
    }

    [Fact]
    public void GetCurrentUserId_ShouldReturnNull_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _httpContextService.GetCurrentUserId();

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public void GetCurrentUserId_ShouldReturnNull_WhenUidClaimIsMissing()
    {
        // Arrange
        var claims = new List<Claim> { new("name", "Test User") };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _httpContextService.GetCurrentUserId();

        // Assert
        result.Should().BeNull();
    }
}
