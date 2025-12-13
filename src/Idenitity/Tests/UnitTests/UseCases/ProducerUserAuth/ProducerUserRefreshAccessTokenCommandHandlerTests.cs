using System.Text.Json;
using Application.UseCases.ProducerUserAuth.ProducerUserRefreshAccessToken;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using Domain.Model;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.ProducerUserAuth;

public class ProducerUserRefreshAccessTokenCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProducerUserRepository> _repository = new();
    private readonly Mock<ICookieService> _cookieService = new();
    private readonly Mock<IHttpContextService> _httpContextService = new();
    private readonly Mock<IJwtService> _jwtService = new();
    private readonly ProducerUserRefreshAccessTokenCommandHandler _handler;

    public ProducerUserRefreshAccessTokenCommandHandlerTests()
    {
        _unitOfWork.Setup(u => u.ProducerUserRepository).Returns(_repository.Object);
        _handler = new ProducerUserRefreshAccessTokenCommandHandler(
            _unitOfWork.Object,
            _cookieService.Object,
            _httpContextService.Object,
            _jwtService.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnNewToken_WhenRefreshValid()
    {
        var userId = Guid.NewGuid();
        var user = new ProducerUser
        {
            Id = userId,
            ProducerId = Guid.NewGuid(),
            Email = "producer@test.com",
            Name = "Producer",
            Password = "hashed",
            Role = Roles.Producer
        };

        _httpContextService.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _repository.Setup(r => r.GetAsync(userId, CancellationToken.None)).ReturnsAsync(user);
        _cookieService.Setup(c => c.GetCookie(userId.ToString()))
            .Returns(JsonSerializer.Serialize(new RefreshTokenCookieModel
            {
                Token = "refresh",
                Expires = DateTime.UtcNow.AddDays(1)
            }));
        _jwtService.Setup(j => j.GenerateJwtToken(user)).Returns("access");

        var result = await _handler.Handle(new ProducerUserRefreshAccessTokenCommand(), CancellationToken.None);

        result.Should().Be("access");
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserIdMissing()
    {
        _httpContextService.Setup(s => s.GetCurrentUserId()).Returns((Guid?)null);

        var act = async () => await _handler.Handle(new ProducerUserRefreshAccessTokenCommand(), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserNotFound()
    {
        var userId = Guid.NewGuid();
        _httpContextService.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _repository.Setup(r => r.GetAsync(userId, CancellationToken.None)).ReturnsAsync((ProducerUser?)null);

        var act = async () => await _handler.Handle(new ProducerUserRefreshAccessTokenCommand(), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenCookieMissing()
    {
        var userId = Guid.NewGuid();
        var user = new ProducerUser
        {
            Id = userId,
            ProducerId = Guid.NewGuid(),
            Email = "producer@test.com",
            Name = "Producer",
            Password = "hashed",
            Role = Roles.Producer
        };

        _httpContextService.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _repository.Setup(r => r.GetAsync(userId, CancellationToken.None)).ReturnsAsync(user);
        _cookieService.Setup(c => c.GetCookie(userId.ToString())).Returns(string.Empty);

        var act = async () => await _handler.Handle(new ProducerUserRefreshAccessTokenCommand(), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenCookieExpired()
    {
        var userId = Guid.NewGuid();
        var user = new ProducerUser
        {
            Id = userId,
            ProducerId = Guid.NewGuid(),
            Email = "producer@test.com",
            Name = "Producer",
            Password = "hashed",
            Role = Roles.Producer
        };

        _httpContextService.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _repository.Setup(r => r.GetAsync(userId, CancellationToken.None)).ReturnsAsync(user);
        _cookieService.Setup(c => c.GetCookie(userId.ToString()))
            .Returns(JsonSerializer.Serialize(new RefreshTokenCookieModel
            {
                Token = "refresh",
                Expires = DateTime.UtcNow.AddDays(-1)
            }));

        var act = async () => await _handler.Handle(new ProducerUserRefreshAccessTokenCommand(), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
