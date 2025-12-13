using System.IdentityModel.Tokens.Jwt;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Tests.UnitTests.Services;

public class JwtServiceTests
{
    private readonly JwtService _sut;

    public JwtServiceTests()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "supersecretkeysupersecretkeysupersecret",
            ["Jwt:ExpiresInMinutes"] = "60"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings!)
            .Build();

        _sut = new JwtService(configuration);
    }

    [Fact]
    public void GenerateJwtToken_ShouldContainUserClaims()
    {
        var user = new ConsumerUser
        {
            Id = Guid.NewGuid(),
            Email = "user@test.com",
            Name = "Test User",
            Password = "hashed",
            Role = Roles.Consumer
        };

        var token = _sut.GenerateJwtToken(user);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        jwt.Claims.First(c => c.Type == "uid").Value.Should().Be(user.Id.ToString());
        jwt.Claims.First(c => c.Type == "rol").Value.Should().Be(user.Role.ToString());
        jwt.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnBase64String()
    {
        var token = _sut.GenerateRefreshToken();

        token.Should().NotBeNullOrWhiteSpace();
        Convert.FromBase64String(token).Length.Should().BeGreaterThan(0);
    }
}
